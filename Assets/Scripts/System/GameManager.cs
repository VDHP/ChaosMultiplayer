using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public event Action OnStateChanged;
    public event Action OnPausedGame;
    public event Action OnUnPausedGame;
    public static GameManager Instance { get; private set; }
    enum State
    {
        WaitingToStart,
        CountDownToStart,
        GamePlaying,
        GameOver,
    }
    State state;

    [SerializeField] float countDownToStartTimer = 3f;
    [SerializeField] float gamePlayingTimerMax = 10f;

    float gamePlayingTimer;
    bool isGamePause;

    private void Awake()
    {
        Instance = this;
        state = State.WaitingToStart;
        isGamePause = false;
    }
    private void Start()
    {
        PlayerInput.Instance.OnPauseAction += Instance_OnPauseAction;
        PlayerInput.Instance.OnInteractAction += Instance_OnInteractAction;
    }

    private void Instance_OnInteractAction(object sender, EventArgs e)
    {
        if(state== State.WaitingToStart)
        {
            state = State.CountDownToStart;
            OnStateChanged?.Invoke();
        }
    }

    private void Instance_OnPauseAction()
    {
        TogglePauseGame();
    }

    private void Update()
    {
        switch (state)
        {
            case State.WaitingToStart:
                break;
            case State.CountDownToStart:
                countDownToStartTimer -= Time.deltaTime;
                if (countDownToStartTimer < 0)
                {
                    state = State.GamePlaying;
                    gamePlayingTimer = gamePlayingTimerMax;
                    OnStateChanged?.Invoke();
                }
                break;
            case State.GamePlaying:
                gamePlayingTimer -= Time.deltaTime;
                if (gamePlayingTimer < 0)
                {
                    state = State.GameOver;
                    OnStateChanged?.Invoke();
                }
                break;
            case State.GameOver:
                break;
        }
    }
    public bool IsGamePlaying()
    {
        return state == State.GamePlaying;
    }   
    public bool IsStartCountDown()
    {
        return state == State.CountDownToStart;
    }
    public int GetStartCountDownTimer()
    {
        return Mathf.CeilToInt(countDownToStartTimer);
    }
    public bool IsGameOver()
    {
        return state == State.GameOver;
    }
    public float GetGamePlayingTimerNormalized()
    {
        return 1-(gamePlayingTimer / gamePlayingTimerMax);
    }
    public void TogglePauseGame()
    {
        isGamePause = !isGamePause;
        if (isGamePause)
        {
            Time.timeScale = 0f;

            OnPausedGame?.Invoke();
        }
        else
        {
            Time.timeScale = 1f;
            OnUnPausedGame?.Invoke();
        }
    }
}
