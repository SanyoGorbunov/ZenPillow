﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Input = InputWrapper.Input;
using Utils;
public class BreathingPressController : MonoBehaviour
{
    public DisplayTimerController displayTimerController;

    public InstructionsPressController instructionsPressController;

    public GameObject Cloud;
    public GameObject UICenter;

    public float pauseDuration = 6.0f;

    private bool _isInhale, _isOver;

    public bool isHorizontal = false, _isInstruction;

    public const float StartCloudScale = 0.6f;
    public float minCloudScale = 0.3f;
    public float maxCloudScale = 1.0f;

    public float estimatedBreathTime = 4.0f;

    private const float MeasurementDuration = 30f;

    private List<InhaleExhalePair> inhaleExhaleParams;

    // Start is called before the first frame update
    void Start()
    {
        inhaleExhaleParams = new List<InhaleExhalePair>();

        CheckScreenOrientation();
        instructionsPressController.Reset();
        Cloud.transform.localScale = new Vector3(StartCloudScale, StartCloudScale, StartCloudScale);

        var isGamePlayed = GameStateManager.Instance.HasPlayedSelectedGame();

        if (!isGamePlayed)
        {
            _isOver = false;
            _isInstruction = true;
            instructionsPressController.SetStartTutorialActive(true);
        }
        else
        {
            StartTimer();
            _isOver = false;
            instructionsPressController.SetStartTutorialActive(false);
            Inhale();
        }
    }

    IEnumerator Instructions()
    {
        _isOver = false;
        _isInstruction = true;
        instructionsPressController.SetStartTutorialActive(true);
        yield return new WaitForSecondsPaused(0.1f);
    }

    public float CloudMoveToCenterTime = 0.2f;

    IEnumerator MoveCloudToCenter()
    {
        //Vector3 originalScale = transform.localScale;
        //Vector3 destinationScale = previousScale;

        Vector3 originalPosition = Cloud.transform.localPosition;
        Vector3 destinationPosition = UICenter.transform.localPosition;

        float currentTime = 0.0f;

        do
        {
            if (!DisplayTimerController.isPausedStatic())
            {
                Cloud.transform.localPosition = Vector3.Lerp(originalPosition, destinationPosition, currentTime / CloudMoveToCenterTime);
                currentTime += Time.deltaTime;
            }
            yield return null;
        } while (currentTime <= CloudMoveToCenterTime);
    }

    IEnumerator Wait()
    {
        yield return new WaitForSecondsPaused(pauseDuration);
        PauseEnd();
    }

    void StartTimer()
    {
        displayTimerController.Activate(MeasurementDuration, () => { GameOver(); });
    }

    void Inhale()
    {
        _isInhale = true;
        instructionsPressController.ShowInhale();
    }

    void Exhale()
    {
        _isInhale = false;
        instructionsPressController.ShowExhale();
    }

    void PauseStart()
    {
    }

    void PauseGame()
    {
        instructionsPressController.ShowHold();
        StartCoroutine(nameof(Wait));
    }

    void PauseEnd()
    {
        instructionsPressController.Reset();
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
    }

    void GameOver()
    {
        //UIMenuController.StaticLoadScene("RateScene");
        GameStateManager.Instance.setActiveBreathParams(new BreathingParams(inhaleExhaleParams));
        UIMenuController.StaticLoadScene("BreathingScene");
    }

    void CheckScreenOrientation()
    {
        isHorizontal = (Screen.width > Screen.height);
        instructionsPressController.RotateScreen(isHorizontal);
        
    }
    private bool isPaused = false;

    private void checkIsPaused()
    {
        bool temp = displayTimerController.isPaused();
        if (temp != isPaused)
        {
            isPaused = temp;
        }
    }

    bool isExpanding = true;

    private float MaxInhaleTime = 0.0f;
    private float MaxExhaleTime = 0.0f;
    private InhaleExhalePair currentInhaleExhalePair;

    // Update is called once per frame
    void Update()
    {
        if (isHorizontal && Screen.width < Screen.height)
        {
            CheckScreenOrientation();
        }
        else if (!isHorizontal && Screen.width > Screen.height)
        {
            CheckScreenOrientation();
        }

        checkIsPaused();

        if (_isInstruction)
        {
            if (Input.touchCount > 0 && EventSystem.current.currentSelectedGameObject == null)
            {
                StartTimer();
                _isInstruction = false;
                instructionsPressController.SetStartTutorialActive(false);
                instructionsPressController.ShowInhale();
            }
        }
        else 
        {
            if (Input.touchCount > 0)
            {
                if(!isExpanding)
                {
                    MaxExhaleTime = LastScaleChangeTimer > MaxExhaleTime ? LastScaleChangeTimer : MaxExhaleTime;
                    currentInhaleExhalePair.ExhaleDuration = LastScaleChangeTimer;
                    inhaleExhaleParams.Add(currentInhaleExhalePair);
                    LastScaleChangeTimer = 0.0f;
                    LastCloudScale = Cloud.transform.localScale.x;
                    isExpanding = true;
                    instructionsPressController.ShowInhale();
                }
            }
            else
            {
                if (isExpanding)
                {
                    MaxInhaleTime = LastScaleChangeTimer > MaxInhaleTime ? LastScaleChangeTimer : MaxInhaleTime;
                    currentInhaleExhalePair = new InhaleExhalePair { InhaleDuration = LastScaleChangeTimer };
                    LastScaleChangeTimer = 0.0f;
                    LastCloudScale = Cloud.transform.localScale.x;
                    isExpanding = false;
                    instructionsPressController.ShowExhale();
                }
            }

            UpdateCloudSize();
        }
    }

    private float LastCloudScale = StartCloudScale;
    private float LastScaleChangeTimer = 0.0f;

    private void UpdateCloudSize()
    {
        LastScaleChangeTimer += Time.deltaTime;
        float alpha = LastScaleChangeTimer / estimatedBreathTime;
        float nextSize = isExpanding ? maxCloudScale : minCloudScale;

        float tempScale = alpha * (nextSize - LastCloudScale) + LastCloudScale;

        tempScale = Mathf.Clamp(tempScale, minCloudScale, maxCloudScale);

        Cloud.transform.localScale = new Vector3(tempScale, tempScale, tempScale);
    }
}
