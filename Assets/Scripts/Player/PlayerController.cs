using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;
using Unity.Netcode;

public class PlayerController : NetworkBehaviour, IKitchenObjectParent
{
    [SerializeField] private float speed;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private LayerMask countersLayerMask;
    [SerializeField] private LayerMask collisionLayerMask;
    [SerializeField] Transform kitchentObjectHoldPoint;
    [SerializeField] List<Vector3> spawnPositionList;
    [SerializeField] PlayerVisual playerVisual;

    public static PlayerController LocalInstance { get; private set; }
    public static event Action OnAnyPlayerSpawned;
    public static event Action<PlayerController> OnAnyPlayerPickedUpSomeThing;


    public event Action OnPickUpSomeThing;
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }

    BaseCounter selectedCounter;
    bool isWalking;
    Vector3 lastInteract;
    KitchenObject kitchenObject;

    private void Start()
    {
        PlayerInput.Instance.OnInteractAction += PlayerInputVector_OnInteractAction;
        PlayerInput.Instance.OnInteractAlternate += GameInput_OnInteractAlternate;

        PlayerData playerData = KitchenObjectMultiplayer.Instance.GetPlayerDataFromFromClientID(OwnerClientId);
        playerVisual.SetPlayerColor(KitchenObjectMultiplayer.Instance.GetPlayerColor(playerData.colorID));
    }

    private void Singleton_OnClientDisconnectCallback(ulong clientID)
    {
        if(clientID == OwnerClientId && HasKitchenObject())
        {
            KitchenObject.DestroyKitchenObject(GetKitchenObject());
        }
    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            LocalInstance = this;
        }
        transform.position = spawnPositionList[(int)OwnerClientId];
        OnAnyPlayerSpawned?.Invoke();
            
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientDisconnectCallback += Singleton_OnClientDisconnectCallback;
        }
    }
    public static void ResetStaticData()
    {
        OnAnyPlayerSpawned = null;
        OnAnyPlayerPickedUpSomeThing = null;
    }

    private void GameInput_OnInteractAlternate()
    {
        if (!GameManager.Instance.IsGamePlaying()) return;
        if (selectedCounter != null && selectedCounter.TryGetComponent<CuttingCounter>(out CuttingCounter cuttingCounter))
        {
            selectedCounter.InteractAlternate(this);
        }
    }

    private void PlayerInputVector_OnInteractAction(object sender, System.EventArgs e)
    {
        if (!GameManager.Instance.IsGamePlaying()) return;
        if (selectedCounter != null)
        {
            if (selectedCounter is PlatesCounter && HasKitchenObject())
            {
                return;
            }
            else if (selectedCounter is ContainCounter )
            {
                if (HasKitchenObject())
                {
                    if(kitchenObject.GetKitchenObjectSO() != selectedCounter.GetComponent<ContainCounter>().GetKitchenObjectSO())
                    {
                        return;
                    }
                }
            }
            selectedCounter.Interact(this);
            OnAnyPlayerPickedUpSomeThing(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;
        HandleMovement();
        HandleInteract();
    }
    public bool IsWalking()
    {
        return isWalking;
    }
    void HandleMovement()
    {
        Vector2 playerInput = PlayerInput.Instance.GetInputPlayerNormalized();
        Vector3 moveDirection = new Vector3(playerInput.x, 0f, playerInput.y);

        float distanceMove = speed * Time.deltaTime;
        float playerWidth = .6f;
        bool canMove = !Physics.BoxCast(transform.position, Vector3.one * playerWidth, moveDirection, Quaternion.identity ,distanceMove, collisionLayerMask);
        // Check whether player move diagonally or not
        if (!canMove)
        {
            // Can't move towards moveDirection
            // Check whether player can move horizontally
            Vector3 moveDirectionX = new Vector3(moveDirection.x, 0f, 0f);
            if (moveDirectionX != Vector3.zero)
            {
                canMove = !Physics.BoxCast(transform.position, Vector3.one * playerWidth, moveDirectionX, Quaternion.identity, distanceMove, collisionLayerMask);
                if (canMove)
                {
                    moveDirection = moveDirectionX.normalized;
                }
                else // Can't move x direction
                {
                    // Check whether player can move vertically 
                    Vector3 moveDirectionZ = new Vector3(0f, 0f, moveDirection.z);
                    if (moveDirectionZ != Vector3.zero)
                    {
                        canMove = !Physics.BoxCast(transform.position, Vector3.one * playerWidth, moveDirectionZ, Quaternion.identity, distanceMove, collisionLayerMask);
                        if (canMove)
                        {
                            moveDirection = moveDirectionZ.normalized;
                        }
                    }
                }
            }
        }
        if (canMove)
        {
            transform.position += moveDirection * distanceMove;
        }
        /// Check whether player is walking 
        isWalking = moveDirection != Vector3.zero;
        transform.forward = Vector3.Slerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);
    }
    void HandleInteract()
    {
        Vector2 playerInput = PlayerInput.Instance.GetInputPlayerNormalized();
        Vector3 moveDirection = new Vector3(playerInput.x, 0f, playerInput.y);

        if (moveDirection != Vector3.zero)
        {
            /// take direct character facing to 
            Vector3 directFacing = Vector3.Slerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);
            lastInteract = directFacing;
        }
        float interactDistance = 2f;
        if (Physics.Raycast(transform.position, lastInteract, out RaycastHit raycastHit, interactDistance, countersLayerMask))
        {
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                if (baseCounter != selectedCounter)
                {
                    SetSelectedCounter(baseCounter);
                }
            }
            else
            {
                SetSelectedCounter(null);
            }
        }
        else
        {
            SetSelectedCounter(null);
        }
    }
    void SetSelectedCounter(BaseCounter newbaseCounter)
    {
        selectedCounter = newbaseCounter;

        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            selectedCounter = newbaseCounter
        });
    }
    public Transform GetKitchenFollowTransform()
    {
        return kitchentObjectHoldPoint;
    }
    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
    public void SetKitchenObject(KitchenObject kitchenObjectPr)
    {
        kitchenObject = kitchenObjectPr;

        if (kitchenObject != null)
        {
            OnPickUpSomeThing?.Invoke();
        }
    }
    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }
    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }
    public NetworkObject GetNetworkObject()
    {
        return NetworkObject;
    }
}
