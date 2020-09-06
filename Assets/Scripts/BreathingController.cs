﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Input = InputWrapper.Input;

public class BreathingController : MonoBehaviour
{
    public BreathingCircleController breathingCircleController;
    public DropletsController dropletsController;
    public InstructionsController instructionsController;
    public DisplayTimerController displayTimerController;

    private bool _isInhale, _isOver, _isInstruction;

    // Start is called before the first frame update
    void Start()
    {
        instructionsController.Reset();

        var isGamePlayed = GameStateManager.Instance.HasPlayedSelectedGame();

        StartTimer();
        if (!isGamePlayed)
        {
            StartCoroutine(nameof(Instructions));
        }
        else
        {
            StartCoroutine(nameof(Play));
        }
    }

    IEnumerator Play()
    {
        _isOver = false;
        instructionsController.SetInstructionsVisibility(false);
        breathingCircleController.SetCircleVisibility(true);
        yield return new WaitForSeconds(1.0f);
        Inhale();
    }

    IEnumerator Instructions()
    {
        _isOver = false;
        _isInstruction = true;
        instructionsController.SetInstructionsVisibility(true);
        breathingCircleController.SetCircleVisibility(false);
        yield return new WaitForSeconds(0.1f);
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(6);
        PauseEnd();
    }

    void StartTimer()
    {
        var gameLength = GameStateManager.Instance.GetAdjustedTimeLengthInSecs();
        displayTimerController.Activate(gameLength, () => { _isOver = true; });
    }

    void Inhale()
    {
        _isInhale = true;
        instructionsController.ShowInhale();
        breathingCircleController.Scale(true, PauseStart);
    }

    void Exhale()
    {
        _isInhale = false;
        instructionsController.ShowExhale();
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
        instructionsController.ShowHold();
        StartCoroutine("Wait");
    }

    void PauseEnd()
    {
        breathingCircleController.SetMove(false);
        dropletsController.DestroyDroplets();
        instructionsController.Reset();
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
        UIMenuController.StaticLoadScene("RateScene");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0 && EventSystem.current.currentSelectedGameObject == null && _isInstruction)
        {
            _isInstruction = false;
            instructionsController.SetInstructionsVisibility(false);
            breathingCircleController.SetCircleVisibility(true);
            Inhale();
        }
    }
}
