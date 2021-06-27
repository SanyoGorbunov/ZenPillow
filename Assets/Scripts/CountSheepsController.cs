using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CountSheepsController : MonoBehaviour
{
    private bool _isOver;

    public SheepsSpawner sheepsSpawner;
    public GameObject transitionOverlay;
    public GameObject tutorial;
    public DisplayTimerController _displayTimerController;

    private Material overlayMat = null;

    private QuickLinksWithPauseController quickLinksWithPause;

    private int SheepCountLeft = 0;

    private bool readyToGenerate = false;

    public int NextNumber = 1;
    private bool isHorizontal;

    // Start is called before the first frame update
    void Start()
    {
        quickLinksWithPause = FindObjectOfType<QuickLinksWithPauseController>();

        SheepCountLeft = 24;

        isHorizontal = Camera.main.aspect >= 1.0f;

        sheepsSpawner.GenerateLevel(SheepCountLeft);

        var renderer = transitionOverlay.GetComponent<Renderer>();

        overlayMat = renderer.material;

        if (GameStateManager.Instance.HasPlayedSelectedGame())
        {
            skipTutorial();
        }
    }

    public void skipTutorial()
    {
        tutorial.SetActive(false);

        StartCoroutine(AnimateAlpha(1.0f, 0.0f, 0.5f));

        StartTimer();
    }

    private IEnumerator AnimateAlpha(float startValue, float endValue, float duration)
    {
        float elapsedTime = 0;
        float ratio = elapsedTime / duration;
        while (ratio < 1f)
        {
            elapsedTime += Time.deltaTime;
            ratio = elapsedTime / duration;

            float alpha = startValue + (endValue - startValue) * ratio;

            overlayMat.SetFloat("_Opacity", alpha);

            yield return null;
        }
    }

    private IEnumerator AnimateAlphaForGeneration(float startValue, float endValue, float duration)
    {
        float elapsedTime = 0;
        float ratio = elapsedTime / duration;

        if (readyToGenerate == false)
        {
            while (ratio < 1f)
            {
                elapsedTime += Time.deltaTime;
                ratio = elapsedTime / duration;

                float alpha = startValue + (endValue - startValue) * ratio;

                overlayMat.SetFloat("_Opacity", alpha);

                yield return null;
            }
        }

        sheepsSpawner.GenerateLevel(SheepCountLeft);
        StartCoroutine(AnimateAlpha(1, 0, 0.2f));
    }

    private IEnumerator AnimateAlphaForEndLevel(float startValue, float endValue, float duration)
    {
        float elapsedTime = 0;
        float ratio = elapsedTime / duration;

        startValue = overlayMat.GetFloat("_Opacity");

        if (readyToGenerate == false)
        {
            while (ratio < 1f)
            {
                elapsedTime += Time.deltaTime;
                ratio = elapsedTime / duration;

                float alpha = startValue + (endValue - startValue) * ratio;

                overlayMat.SetFloat("_Opacity", alpha);

                yield return null;
            }
        }

        GameOver();
    }

    private IEnumerator TransitLevel()
    {
        if (readyToGenerate == false)
        {
            yield return null;
        }
        else
        {
            readyToGenerate = false;
            sheepsSpawner.GenerateLevel(SheepCountLeft);
            StartCoroutine(AnimateAlpha(1, 0, 0.2f));
        }
    }


    public void ClickSheep()
    {
        SheepCountLeft--;
        NextNumber++;
        resetHintTimer();
        if (SheepCountLeft == 0)
        {
            SheepCountLeft = 24;
            NextNumber = 1;
            readyToGenerate = false;
            StartCoroutine(AnimateAlphaForGeneration(0, 1, 0.2f));
            //StartCoroutine(TransitLevel());
        }
    }



    void StartTimer()
    {
        var gameLength = GameStateManager.Instance.GetAdjustedTimeLengthInSecs();

        _displayTimerController.Activate(gameLength, () =>
        {
            _isOver = true;
            StartCoroutine(AnimateAlphaForEndLevel(0, 1, 0.5f));
        });

        quickLinksWithPause.DisplayPause();
    }

    void GameOver()
    {
        UIMenuController.StaticLoadScene("RateScene");
    }

    public float timerBeforeHint = 5.0f;
    public bool HintIsActive = false;

    public void resetHintTimer()
    {
        timerBeforeHint = 5.0f;
        HintIsActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        bool newIsHorizontal = Camera.main.aspect >= 1.0f;
        if (newIsHorizontal != isHorizontal)
        {
            isHorizontal = newIsHorizontal;
            sheepsSpawner.Rotate(isHorizontal);
        }
        if (!HintIsActive)
        { 
            timerBeforeHint -= Time.deltaTime;
            if (timerBeforeHint <= 0.0f)
            {
                sheepsSpawner.getSheepByNumber(NextNumber).ActivateHint();
                HintIsActive = true;
            }
        }
    }
}
