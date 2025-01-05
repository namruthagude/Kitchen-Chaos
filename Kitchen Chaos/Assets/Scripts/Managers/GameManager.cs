using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public event EventHandler OnStateChanged;
    public event EventHandler OnGamePaused;
    public event EventHandler OnGameResumed;
    private enum State
    {
        WaitingToStart,
        CountingToStart,
        GamePlaying,
        GameOver
    }

    private State state;
    private float countDownToStartTimer = 1f;
    private float gamePlayingTimer;
    private float gamePlayingTimerMax = 300f;
    private bool IsGamePaused = false;

    private void Awake()
    {
        Instance = this;
        state = State.WaitingToStart;
    }

    private void Start()
    {
        GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;
        GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;

        //Debug to trigger game to start automatically
        state = State.CountingToStart;
        OnStateChanged?.Invoke(this, EventArgs.Empty);
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        if (state == State.WaitingToStart)
        {
            state = State.CountingToStart;
            OnStateChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    private void GameInput_OnPauseAction(object sender, EventArgs e)
    {
        TogglePauseGame();
    }

    private void Update()
    {
        switch (state)
        {
            case State.WaitingToStart:
                
                break;
            case State.CountingToStart:
                countDownToStartTimer -= Time.deltaTime;
                if (countDownToStartTimer <= 0f)
                {
                    state = State.GamePlaying;
                    gamePlayingTimer = gamePlayingTimerMax;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GamePlaying:
                gamePlayingTimer -= Time.deltaTime;
                if (gamePlayingTimer <= 0f)
                {                  
                    state = State.GameOver;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GameOver:               
                break;
        }

        Debug.Log(state);
    }

    public bool IsGamePlaying()
    {
        return state == State.GamePlaying;
    }

    public bool IsCountDownToStateActive()
    {
        return state == State.CountingToStart;
    }

    public float GetCountDownTimer()
    {
        return countDownToStartTimer;
    }

    public bool IsGameOver()
    {
        return state == State.GameOver;
    }

    public float GamePlayingTimerNormalized()
    {
        return 1 - (gamePlayingTimer/gamePlayingTimerMax);
    }

    public void TogglePauseGame()
    {
        IsGamePaused = !IsGamePaused;
        if (IsGamePaused)
        {
            Time.timeScale = 0;
            OnGamePaused?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            Time.timeScale = 1;
            OnGameResumed?.Invoke(this, EventArgs.Empty);
        }
    }
}
