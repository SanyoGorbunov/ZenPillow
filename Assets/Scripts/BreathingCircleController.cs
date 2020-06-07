using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using Input = InputWrapper.Input;

public class BreathingCircleController : MonoBehaviour
{
    private const float TimeBreathingInSecs = 6.0f;
    private const float TimePauseInSecs = 1.0f;

    private GameObject _innerCircle;

    private Vector3 previousScale, previousPosition;
    private bool _canMove;
    private Vector2 touchStartPos;

    // Start is called before the first frame update
    void Start()
    {
        _innerCircle = GameObject.FindGameObjectWithTag("InnerCircle");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0 && EventSystem.current.currentSelectedGameObject == null && _canMove)
        {
            var touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    touchStartPos = touch.position;
                    break;
                case TouchPhase.Moved:
                    var currentPosition = touch.position;
                    var delta = currentPosition - touchStartPos;
                    var xToSet = delta.x;
                    if (xToSet > 46) { xToSet = 46; }
                    else if (xToSet < -46) { xToSet = -46; }

                    transform.localPosition = new Vector3(xToSet, -70f, 0f);
                    break;
                default:
                    break;
            }
        }
    }

    public void SetMove(bool canMove)
    {
        _canMove = canMove;
    }

    public void Scale(bool isUpscaling, Action callback)
    {
        StartCoroutine(ScaleOverTime(isUpscaling, callback));
    }

    public void Minify(Action callback)
    {
        StartCoroutine(ToMiddle(callback));
    }

    public void Maxify(Action callback)
    {
        StartCoroutine(ToTop(callback));
    }

    IEnumerator ScaleOverTime(bool isUpscaling, Action callback)
    {
        Vector3 originalScale = isUpscaling ? new Vector3(0.0f, 0.0f, 1.0f) : new Vector3(1.0f, 1.0f, 1.0f);
        Vector3 destinationScale = !isUpscaling ? new Vector3(0.0f, 0.0f, 1.0f) : new Vector3(1.0f, 1.0f, 1.0f);

        Vector3 originalPosition = isUpscaling ? new Vector3(0f, 0f, 0f) : new Vector3(0f, 60f, 0f);
        Vector3 destinationPosition = !isUpscaling ? new Vector3(0f, 0f, 0f) : new Vector3(0f, 60f, 0f);

        float currentTime = 0.0f;

        do
        {
            _innerCircle.transform.localScale = Vector3.Lerp(originalScale, destinationScale, currentTime / TimeBreathingInSecs);
            gameObject.transform.localPosition = Vector3.Lerp(originalPosition, destinationPosition, currentTime / TimeBreathingInSecs);
            currentTime += Time.deltaTime;
            yield return null;
        } while (currentTime <= TimeBreathingInSecs);

        callback.Invoke();
    }

    IEnumerator ToMiddle(Action callback)
    {
        Vector3 originalScale = transform.localScale;
        previousScale = originalScale;
        Vector3 destinationScale = new Vector3(0.5f, 0.5f, 1.0f);

        Vector3 originalPosition = transform.localPosition;
        previousPosition = originalPosition;
        Vector3 destinationPosition = new Vector3(0.0f, -70.0f, 0.0f);

        float currentTime = 0.0f;

        do
        {
            gameObject.transform.localScale = Vector3.Lerp(originalScale, destinationScale, currentTime / TimePauseInSecs);
            gameObject.transform.localPosition = Vector3.Lerp(originalPosition, destinationPosition, currentTime / TimePauseInSecs);
            currentTime += Time.deltaTime;
            yield return null;
        } while (currentTime <= TimePauseInSecs);

        callback.Invoke();
    }

    IEnumerator ToTop(Action callback)
    {
        Vector3 originalScale = transform.localScale;
        Vector3 destinationScale = previousScale;

        Vector3 originalPosition = transform.localPosition;
        Vector3 destinationPosition = previousPosition;

        float currentTime = 0.0f;

        do
        {
            gameObject.transform.localScale = Vector3.Lerp(originalScale, destinationScale, currentTime / TimePauseInSecs);
            gameObject.transform.localPosition = Vector3.Lerp(originalPosition, destinationPosition, currentTime / TimePauseInSecs);
            currentTime += Time.deltaTime;
            yield return null;
        } while (currentTime <= TimePauseInSecs);

        callback.Invoke();
    }
}