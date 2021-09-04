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

    private int SheepCountLeft = 0;

    private bool readyToGenerate = false;
    public int NextNumber = 1;

    public bool IsSimplified = false;
    private bool isHorizontal;

    private int MaxSheepCount;

    // Start is called before the first frame update
    void Start()
    {
        MaxSheepCount = IsSimplified ? 7 : 24;

        SheepCountLeft = MaxSheepCount;

        isHorizontal = Camera.main.aspect >= 1.0f;

        sheepsSpawner.GenerateLevel(IsSimplified);

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

        ShouldEnlarge = IsSimplified;
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

        sheepsSpawner.GenerateLevel(IsSimplified);
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
            sheepsSpawner.GenerateLevel(IsSimplified);
            StartCoroutine(AnimateAlpha(1, 0, 0.2f));
        }
    }


    public void ClickSheep()
    {
        SheepCountLeft--;
        NextNumber++;

        if (IsSimplified)
        {
            ShouldEnlarge = true;
        }
        else
        {
            resetHintTimer();
        }

        if (SheepCountLeft == 0)
        {
            SheepCountLeft = MaxSheepCount;
            NextNumber = 1;
            readyToGenerate = false;
            StartCoroutine(AnimateAlphaForGeneration(0, 1, 0.2f));
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
    }

    void GameOver()
    {
        UIMenuController.StaticLoadScene("RateScene");
    }

    public float timerBeforeHint = 5.0f;
    public bool HintIsActive = false;
    public bool ShouldEnlarge;

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
            sheepsSpawner.Rotate(IsSimplified, isHorizontal);
        }
        if (!HintIsActive && !IsSimplified)
        { 
            timerBeforeHint -= Time.deltaTime;
            if (timerBeforeHint <= 0.0f)
            {
                sheepsSpawner.getSheepByNumber(NextNumber).ActivateHint();
                HintIsActive = true;
            }
        }

        if (ShouldEnlarge)
        {
            sheepsSpawner.getSheepByNumber(NextNumber).Enlarge();
            ShouldEnlarge = false;
        }
    }
}
