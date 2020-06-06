using System;
using System.Collections;
using UnityEngine;

public class BreathingController : MonoBehaviour
{
    public BreathingCircleController breathingCircleController;

    private bool isInhale;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(nameof(Play));
    }

    IEnumerator Play()
    {
        yield return new WaitForSeconds(1.0f);
        Inhale();
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
        StartCoroutine("Wait");
    }

    void PauseEnd()
    {
        breathingCircleController.SetMove(false);
        Action action = Inhale;
        if (isInhale)
        {
            action = Exhale;
        }
        breathingCircleController.Maxify(action);
    }

    IEnumerator Wait() {
        yield return new WaitForSeconds(6);
        PauseEnd();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
