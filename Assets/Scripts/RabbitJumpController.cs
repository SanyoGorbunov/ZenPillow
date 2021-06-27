using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RabbitJumpController : MonoBehaviour
{
    private bool _isOver;

    public DotsSpawner dotSpawner;
    public RabbitController rabbitController;
    public GameObject transitionOverlay;
    public DisplayTimerController displayTimerController;

    public GameObject tutorial;

    private Material overlayMat = null;

    private int CarrotCountLeft = 0;

    private bool readyToGenerate = false;
    private bool isHorizontal;

    // Start is called before the first frame update
    void Start()
    {
        CarrotCountLeft = 5;

        isHorizontal = Camera.main.aspect >= 1.0f;
        dotSpawner.GenerateLevel(CarrotCountLeft);

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

        dotSpawner.GenerateLevel(CarrotCountLeft);
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
            dotSpawner.GenerateLevel(CarrotCountLeft);
            StartCoroutine(AnimateAlpha(1, 0, 0.2f));
        }
    }


    public void PickUpCarrot()
    {
        CarrotCountLeft--;
        if (CarrotCountLeft == 0)
        {
            CarrotCountLeft = 5;
            readyToGenerate = false;
            StartCoroutine(AnimateAlphaForGeneration(0, 1, 0.2f));
            //StartCoroutine(TransitLevel());
        }
    }

    void StartTimer()
    {
        var gameLength = GameStateManager.Instance.GetAdjustedTimeLengthInSecs();
        displayTimerController.Activate(gameLength, () =>
        {
            _isOver = true;
            StartCoroutine(AnimateAlphaForEndLevel(0, 1, 0.5f));
        });
    }

    void GameOver()
    {
        UIMenuController.StaticLoadScene("RateScene");
    }

    // Update is called once per frame
    void Update()
    {
        bool newIsHorizontal = Camera.main.aspect >= 1.0f;
        if (newIsHorizontal != isHorizontal)
        {
            isHorizontal = newIsHorizontal;
            dotSpawner.Rotate(isHorizontal);
        }
    }
}
