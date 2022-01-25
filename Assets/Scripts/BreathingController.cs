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

    private bool _isOver;
    private BreathingState _breathingState;
    private BreathingParams _breathingParams;

    public bool isHorizontal = false, _isInstruction;

    // Start is called before the first frame update
    void Start()
    {
        CheckScreenOrientation();
        instructionsController.Reset();

        var isGamePlayed = GameStateManager.Instance.HasPlayedSelectedGame();
        _breathingParams = GameStateManager.Instance.getActiveBreathParams();

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

    IEnumerator Hold(float holdDuration, Action callback)
    {
        yield return new WaitForSeconds(holdDuration);
        callback.Invoke();
    }

    void StartTimer()
    {
        var gameLength = GameStateManager.Instance.GetAdjustedTimeLengthInSecs();
        displayTimerController.Activate(gameLength, () => { _isOver = true; });
    }

    private enum BreathingState
    {
        Inhale,
        InhaleHold,
        Exhale,
        ExhaleHold
    }

    void Inhale()
    {
        _breathingState = BreathingState.Inhale;
        instructionsController.ShowInhale();
        breathingCircleController.Scale(true, InhaleHold);
    }

    void InhaleHold()
    {
        _breathingState = BreathingState.InhaleHold;
        instructionsController.ShowHold();
        StartCoroutine(Hold(_breathingParams.breathInHoldTime, Exhale));
    }

    void Exhale()
    {
        _breathingState = BreathingState.Exhale;
        instructionsController.ShowExhale();
        breathingCircleController.Scale(false, ExhaleHold);
    }

    void ExhaleHold()
    {
        _breathingState = BreathingState.ExhaleHold;
        instructionsController.ShowHold();
        StartCoroutine(Hold(_breathingParams.breathOutHoldTime, Inhale));
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
        if (_breathingState == BreathingState.Inhale)
        {
            action = Exhale;
        }
        if (_breathingState == BreathingState.Exhale && _isOver)
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
