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

    private bool _isInhale, _isOver, _isInstruction;

    // Start is called before the first frame update
    void Start()
    {
        var isGamePlayed = false; // GameStateManager.Instance.HasPlayedSelectedGame();

        StartCoroutine(nameof(Timer));
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

    IEnumerator Timer()
    {
        var gameLength = GameStateManager.Instance.GetTimeLengthInMins();
        yield return new WaitForSeconds(gameLength * 60);
        _isOver = true;
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
        if (Input.touchCount > 0 && EventSystem.current.currentSelectedGameObject == null && _isInstruction)
        {
            _isInstruction = false;
            instructionsController.SetInstructionsVisibility(false);
            breathingCircleController.SetCircleVisibility(true);
            Inhale();
        }
    }
}
