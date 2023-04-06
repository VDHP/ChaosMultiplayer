using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public event Action OnStateChanged;
    public event Action OnLocalPausedGame;
    public event Action OnLocalUnPausedGame;
    public event Action OnLocalPlayerReadyChanged;
    public event Action OnMultiplayerGamePaused;
    public event Action OnMultiplayerGameUnPaused;
    public static GameManager Instance { get; private set; }
    enum State
    {
        WaitingToStart,
        CountDownToStart,
        GamePlaying,
        GameOver,
    }
    [SerializeField] NetworkVariable<float> countDownToStartTimer = new NetworkVariable<float>(3f);
    [SerializeField] float gamePlayingTimerMax = 10f;

    NetworkVariable<float> gamePlayingTimer = new NetworkVariable<float>(0f);
    NetworkVariable<State> state = new NetworkVariable<State>(State.WaitingToStart);
    NetworkVariable<bool> isGamePaused = new NetworkVariable<bool>(false);

    Dictionary<ulong, bool> playerReadyDictionary = new Dictionary<ulong, bool>();
    Dictionary<ulong, bool> playerPausedDictionary = new Dictionary<ulong, bool>();
    

    bool isLocalGamePause;
    bool isLocalPlayerReady;


    public override void OnNetworkSpawn()   
    {
        state.OnValueChanged += State_OnValueChanged;
        isGamePaused.OnValueChanged += IsGamePaused_OnValueChanged;
    }

    private void IsGamePaused_OnValueChanged(bool previousValue, bool newValue)
    {
        if(isGamePaused.Value)
        {
            Time.timeScale = 0f;
            OnMultiplayerGamePaused?.Invoke();
        }
        else
        {
            Time.timeScale = 1f;
            OnMultiplayerGameUnPaused?.Invoke();
        }
    }

    private void State_OnValueChanged(State previousValue, State newValue)
    {
        OnStateChanged?.Invoke();
    }

    private void Awake()
    {
        Instance = this;

    }
    private void Start()
    {
        PlayerInput.Instance.OnPauseAction += Instance_OnPauseAction;
        PlayerInput.Instance.OnInteractAction += Instance_OnInteractAction;
    }

    private void Instance_OnInteractAction(object sender, EventArgs e)
    {
        if(state.Value == State.WaitingToStart)
        {
            isLocalPlayerReady = true;
            OnLocalPlayerReadyChanged?.Invoke();
            SetPlayerReadyServerRpc();

        }
    }
    [ServerRpc(RequireOwnership =false)]
    void SetPlayerReadyServerRpc(ServerRpcParams serverRpcParams = default)
    {
        playerReadyDictionary[serverRpcParams.Receive.SenderClientId] = true;

        bool allClientReady = true;
        foreach (ulong clientID in NetworkManager.Singleton.ConnectedClientsIds)
        {
            if (!playerReadyDictionary.ContainsKey(clientID) || !playerReadyDictionary[clientID])
            {
                /// this  player is not ready
                allClientReady = false;
                break;
            }
        }
        if(allClientReady)
        {
            state.Value = State.CountDownToStart;
        }
    }

    private void Instance_OnPauseAction()
    {
        TogglePauseGame();
    }

    private void Update()
    {
        if (!IsServer) return;
        switch (state.Value)
        {
            case State.WaitingToStart:
                break;
            case State.CountDownToStart:
                countDownToStartTimer.Value -= Time.deltaTime;
                if (countDownToStartTimer.Value < 0)
                {
                    state.Value = State.GamePlaying;
                    gamePlayingTimer.Value = gamePlayingTimerMax;
                }
                break;
            case State.GamePlaying:
                gamePlayingTimer.Value -= Time.deltaTime;
                if (gamePlayingTimer.Value < 0)
                {
                    state.Value = State.GameOver;
                }
                break;
            case State.GameOver:
                break;
        }
    }
    public bool IsLocalPlayerReady()
    {
        return isLocalPlayerReady;
    }
    public bool IsGamePlaying()
    {
        return state.Value == State.GamePlaying;
    }   
    public bool IsStartCountDown()
    {
        return state.Value == State.CountDownToStart;
    }
    public int GetStartCountDownTimer()
    {
        return Mathf.CeilToInt(countDownToStartTimer.Value);
    }
    public bool IsGameOver()
    {
        return state.Value == State.GameOver;
    }
    public float GetGamePlayingTimerNormalized()
    {
        return 1-(gamePlayingTimer.Value / gamePlayingTimerMax);
    }
    public void TogglePauseGame()
    {
        isLocalGamePause = !isLocalGamePause;
        if (isLocalGamePause)
        {
            PauseGameServerRpc();
            OnLocalPausedGame?.Invoke();
        }
        else
        {
            UnPauseGameServerRpc();
            OnLocalUnPausedGame?.Invoke();
        }
    }
    [ServerRpc(RequireOwnership =false)]
    void PauseGameServerRpc(ServerRpcParams serverRpcParams = default)
    {
        playerPausedDictionary[serverRpcParams.Receive.SenderClientId] = true;

        TestGamePausedState();
    }
    [ServerRpc(RequireOwnership =false)]
    void UnPauseGameServerRpc(ServerRpcParams serverRpcParams = default)
    {
        playerPausedDictionary[serverRpcParams.Receive.SenderClientId] = false;

        TestGamePausedState();
    }
    void TestGamePausedState()
    {
        foreach(ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            if(playerPausedDictionary.ContainsKey(clientId)&& playerPausedDictionary[clientId])
            {
                // this player is paused 
                isGamePaused.Value = true;
                return;
            }
        }
        // all player unpaused
        isGamePaused.Value = false;
    }
}
