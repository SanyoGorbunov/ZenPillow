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

    private Material overlayMat = null;

    private int CarrotCountLeft = 0;

    private bool readyToGenerate = false;

    // Start is called before the first frame update
    void Start()
    {
        CarrotCountLeft = 5;

        dotSpawner.GenerateLevel(CarrotCountLeft);

        var renderer = transitionOverlay.GetComponent<Renderer>();

        overlayMat = renderer.material;

        StartCoroutine(AnimateAlpha(1.0f, 0.0f, 0.5f));

        StartCoroutine(Finish());
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



    IEnumerator Finish()
    {
        var gameLength = GameStateManager.Instance.GetTimeLengthInMins();
        if (gameLength < 1.0f)
        {
            yield return new WaitForSeconds(10.0f);
        }
        else
        {
            yield return new WaitForSeconds(60 * gameLength);
        }
        
        _isOver = true;
        StartCoroutine(AnimateAlphaForEndLevel(0, 1, 0.5f));
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
