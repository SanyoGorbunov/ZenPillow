using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class BreathingCircleController : MonoBehaviour
{
    private const float TimeInSecs = 6.0f;

    private GameObject _innerCircle;

    // Start is called before the first frame update
    void Start()
    {
        _innerCircle = GameObject.FindGameObjectWithTag("InnerCircle");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Scale(bool isUpscaling, Action<bool> callback)
    {
        StartCoroutine(ScaleOverTime(isUpscaling, callback));
    }

    IEnumerator ScaleOverTime(bool isUpscaling, Action<bool> callback)
    {
        Vector3 originalScale = isUpscaling ? new Vector3(0.2f, 0.2f, 1.0f) : new Vector3(1.0f, 1.0f, 1.0f);
        Vector3 destinationScale = !isUpscaling ? new Vector3(0.2f, 0.2f, 1.0f) : new Vector3(1.0f, 1.0f, 1.0f);

        float currentTime = 0.0f;

        do
        {
            _innerCircle.transform.localScale = Vector3.Lerp(originalScale, destinationScale, currentTime / TimeInSecs);
            currentTime += Time.deltaTime;
            yield return null;
        } while (currentTime <= TimeInSecs);

        callback.Invoke(isUpscaling);
    }
}