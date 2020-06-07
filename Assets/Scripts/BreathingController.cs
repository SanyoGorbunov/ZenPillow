﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BreathingController : MonoBehaviour
{
    public BreathingCircleController breathingCircleController;
    public DropletsController dropletsController;

    private bool _isInhale, _isOver;

    // Start is called before the first frame update
    void Start()
    {
        var isGamePlayed = GameStateManager.Instance.HasPlayedSelectedGame();

        StartCoroutine(nameof(Timer));
        if (!isGamePlayed)
        {
            ShowInstructions();
        }
        else
        {
            StartCoroutine(nameof(Play));
        }
    }

    IEnumerator Play()
    {
        _isOver = false;
        yield return new WaitForSeconds(1.0f);
        Inhale();
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(6);
        PauseEnd();
    }

    IEnumerator Timer()
    {
        var gameLength = GameStateManager.Instance.GetTimeLengthInMins();
        yield return new WaitForSeconds(gameLength * 60);
        _isOver = true;
    }

    void ShowInstructions()
    {
        Debug.LogWarning(DateTime.UtcNow);
        StartCoroutine(nameof(Play));
    }

    void Inhale()
    {
        _isInhale = true;
        breathingCircleController.Scale(true, PauseStart);
    }

    void Exhale()
    {
        _isInhale = false;
        breathingCircleController.Scale(false, PauseStart);
    }

    void PauseStart()
    {
        breathingCircleController.Minify(PauseGame);
    }

    void PauseGame()
    {
        breathingCircleController.SetMove(true);
        dropletsController.Create();
        StartCoroutine("Wait");
    }

    void PauseEnd()
    {
        breathingCircleController.SetMove(false);
        dropletsController.DestroyDroplets();
        Action action = Inhale;
        if (_isInhale)
        {
            action = Exhale;
        }
        if (!_isInhale && _isOver)
        {
            GameOver();
            return;
        }
        breathingCircleController.Maxify(action);
    }

    void GameOver()
    {
        SceneManager.LoadScene("RateScene");
    }

    // Update is called once per frame
    void Update()
    {
    }
}
