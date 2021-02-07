using System;
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
    public BreathLineController lineController;
    public GameObject smallCloud;

    public bool isHorizontal = false;

    public float pauseDuration = 6.0f;

    private bool _isInhale, _isOver, _isInstruction;

    // Start is called before the first frame update
    void Start()
    {
        checkScreenOrientation();
        instructionsController.Reset();

        var isGamePlayed = GameStateManager.Instance.HasPlayedSelectedGame();

        if (!isGamePlayed)
        {
            StartCoroutine(nameof(Instructions));
        }
        else
        {
            StartTimer();
            //StartCoroutine(nameof(Play));
            _isOver = false;
            instructionsController.SetInstructionsVisibility(false);
            breathingCircleController.SetCircleVisibility(true);
            Inhale();
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
        yield return new WaitForSeconds(pauseDuration);
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

    void checkScreenOrientation()
    {
        isHorizontal = (Screen.width > Screen.height);
        breathingCircleController.SetVisibility(!isHorizontal);
        instructionsController.SetVisibility(!isHorizontal);
        dropletsController.SetVisibility(!isHorizontal);
        lineController.gameObject.GetComponent<CanvasRenderer>().SetAlpha(isHorizontal ? 1 : 0);
        smallCloud.gameObject.GetComponent<CanvasRenderer>().SetAlpha(isHorizontal ? 1 : 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (isHorizontal && Screen.width < Screen.height)
        {
            checkScreenOrientation();
        }
        else if (!isHorizontal && Screen.width > Screen.height)
        {
            checkScreenOrientation();
        }

        if (Input.touchCount > 0 && EventSystem.current.currentSelectedGameObject == null && _isInstruction)
        {
            StartTimer();
            _isInstruction = false;
            instructionsController.SetInstructionsVisibility(false);
            breathingCircleController.SetCircleVisibility(true);
            Inhale();
        }
    }
}
