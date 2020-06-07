using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BreathingController : MonoBehaviour
{
    public BreathingCircleController breathingCircleController;
    public DropletsController dropletsController;

    private bool isInhale, _isOver;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(nameof(Play));
        StartCoroutine(nameof(Finish));
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

    IEnumerator Finish()
    {
        var gameLength = GameStateManager.Instance.GetTimeLengthInMins();
        yield return new WaitForSeconds(gameLength * 60);
        _isOver = true;
    }

    void Inhale()
    {
        isInhale = true;
        breathingCircleController.Scale(true, PauseStart);
    }

    void Exhale()
    {
        isInhale = false;
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
        if (isInhale)
        {
            action = Exhale;
        }
        if (!isInhale && _isOver)
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
