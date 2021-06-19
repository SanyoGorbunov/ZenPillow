using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Input = InputWrapper.Input;
using Utils;
public class BreathingController : MonoBehaviour
{
    public DisplayTimerController displayTimerController;

    public BreathingCircleController breathingCircleController;
    public DropletsController dropletsController;
    public InstructionsController instructionsController;
    public BreathLineController lineController;

    public float pauseDuration = 6.0f;

    private bool _isInhale, _isOver;

    public bool isHorizontal = false, _isInstruction;

    // Start is called before the first frame update
    void Start()
    {
        CheckScreenOrientation();
        instructionsController.Reset();

        var isGamePlayed = GameStateManager.Instance.HasPlayedSelectedGame();

        if (!isGamePlayed)
        {
            //StartCoroutine(nameof(Instructions));
            _isOver = false;
            _isInstruction = true;
            instructionsController.SetStartTutorialActive(true);
            breathingCircleController.SetCircleVisibility(false);
        }
        else
        {
            StartTimer();
            lineController.Launch();
            //StartCoroutine(nameof(Play));
            _isOver = false;
            instructionsController.SetStartTutorialActive(false);
            breathingCircleController.SetCircleVisibility(true);
            Inhale();
        }
    }

    IEnumerator Instructions()
    {
        _isOver = false;
        _isInstruction = true;
        instructionsController.SetStartTutorialActive(true);
        breathingCircleController.SetCircleVisibility(false);
        yield return new WaitForSecondsPaused(0.1f);
    }

    IEnumerator Wait()
    {
        yield return new WaitForSecondsPaused(pauseDuration);
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
        StartCoroutine(nameof(Wait));
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

    void CheckScreenOrientation()
    {
        isHorizontal = (Screen.width > Screen.height);
        breathingCircleController.SetVisibility(!isHorizontal);
        instructionsController.RotateScreen(isHorizontal);
        dropletsController.SetVisibility(!isHorizontal);

        if (!_isInstruction)
        {
            lineController.SetVisibility(isHorizontal);
        }
        else
        {
            lineController.SetVisibility(false);
        }
        
    }
    private bool isPaused = false;

    private void checkIsPaused()
    {
        bool temp = displayTimerController.isPaused();
        if (temp != isPaused)
        {
            isPaused = temp;
            dropletsController.SetIsPaused(isPaused);
        }
    }
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

        if (Input.touchCount > 0 && EventSystem.current.currentSelectedGameObject == null && _isInstruction)
        {
            StartTimer();
            _isInstruction = false;
            instructionsController.SetStartTutorialActive(false);
            lineController.Launch();
            breathingCircleController.SetCircleVisibility(true);
            if (isHorizontal)
            {
                lineController.SetVisibility(true);
                breathingCircleController.SetVisibility(false);
            }


            Inhale();
        }
    }
}
