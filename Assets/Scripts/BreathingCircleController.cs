using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using Input = InputWrapper.Input;

public class BreathingCircleController : MonoBehaviour
{
    public float TimeBreathingInSecs = 6.0f;
    private const float TimePauseInSecs = 0.5f;

    public GameObject _innerCircle;
    public GameObject _outerCircle;

    public GameObject _cloudImage;

    private Vector3 previousScale, previousPosition;
    private bool _canMove;
    private Vector2 touchStartPos;
    private float touchStartXPos;

    private float breathInTime = 4.0f;
    private float breathOutTime = 4.0f;
    private float breathInHoldTime = 4.0f;
    private float breathOutHoldTime = 4.0f;

    private int width;
    private int height;
    // Start is called before the first frame update
    void Start()
    {
        Utils.BreathingParams params1 = GameStateManager.Instance.getActiveBreathParams();

        if (params1 != null)
        {
            this.breathInTime = params1.breathInTime;
            this.breathOutTime = params1.breathOutTime;
            this.breathInHoldTime = params1.breathInHoldTime;
            this.breathOutHoldTime = params1.breathOutHoldTime;
        }

        breathInTime = breathInTime >= 2.0f ? breathInTime : 2.0f;
        breathOutTime = breathOutTime >= 2.0f ? breathOutTime : 2.0f;

        width = Screen.width;
        height = Screen.height;
    }

    public void SetVisibility(bool isVisible)
    {
        _innerCircle.GetComponent<CanvasRenderer>().SetAlpha(isVisible ? 1 : 0);
        _outerCircle.GetComponent<CanvasRenderer>().SetAlpha(isVisible ? 1 : 0);
        _cloudImage.GetComponent<CanvasRenderer>().SetAlpha(isVisible ? 1 : 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (_canMove && Input.touchCount > 0 && EventSystem.current.currentSelectedGameObject == null)
        {
            var touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    touchStartPos = touch.position;

                    touchStartXPos = transform.localPosition.x;

                    break;
                case TouchPhase.Moved:
                    var currentPosition = touch.position;
                    var delta = currentPosition - touchStartPos;
                    const float xref = 180.0f;
                    float scale = width / xref;

                    //delta

                    var xToSet = touchStartXPos + delta.x/ scale;
                    if (xToSet > 46) { xToSet = 46; }
                    else if (xToSet < -46) { xToSet = -46; }

                    transform.localPosition = new Vector3(xToSet, -65f, 0f);
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
        StartCoroutine(ScaleOnMovement(isUpscaling, callback));
    }

    public void Minify(Action callback)
    {
        StartCoroutine(ToBottom(callback));
    }

    public void Maxify(Action callback)
    {
        StartCoroutine(ToTop(callback));
    }

    IEnumerator ScaleOnMovement(bool isUpscaling, Action callback)
    {
        float ScaleTime = isUpscaling ? breathInTime : breathOutTime;

        Vector3 originalScale = isUpscaling ? new Vector3(0.0f, 0.0f, 1.0f) : new Vector3(1.0f, 1.0f, 1.0f);
        Vector3 destinationScale = !isUpscaling ? new Vector3(0.0f, 0.0f, 1.0f) : new Vector3(1.0f, 1.0f, 1.0f);

        Vector3 originalPosition = isUpscaling ? new Vector3(0f, 0f, 0f) : new Vector3(0f, 60f, 0f);
        Vector3 destinationPosition = !isUpscaling ? new Vector3(0f, 0f, 0f) : new Vector3(0f, 60f, 0f);

        float currentTime = 0.0f;

        do
        {
            if (!DisplayTimerController.isPausedStatic())
            { 
                _innerCircle.transform.localScale = Vector3.Lerp(originalScale, destinationScale, currentTime / ScaleTime);
                gameObject.transform.localPosition = Vector3.Lerp(originalPosition, destinationPosition, currentTime / ScaleTime);
                currentTime += Time.deltaTime;
            }
            yield return null;
        } while (currentTime <= ScaleTime);

        callback.Invoke();
    }

    IEnumerator ToBottom(Action callback)
    {
        Vector3 originalScale = transform.localScale;
        previousScale = originalScale;
        Vector3 destinationScale = new Vector3(0.5f, 0.5f, 1.0f);

        Vector3 originalPosition = transform.localPosition;
        previousPosition = originalPosition;
        Vector3 destinationPosition = new Vector3(0.0f, -65.0f, 0.0f);

        float currentTime = 0.0f;

        do
        {
            if (!DisplayTimerController.isPausedStatic())
            {
                gameObject.transform.localScale = Vector3.Lerp(originalScale, destinationScale, currentTime / TimePauseInSecs);
                gameObject.transform.localPosition = Vector3.Lerp(originalPosition, destinationPosition, currentTime / TimePauseInSecs);
                currentTime += Time.deltaTime;
            }
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
            if (!DisplayTimerController.isPausedStatic())
            {
                gameObject.transform.localScale = Vector3.Lerp(originalScale, destinationScale, currentTime / TimePauseInSecs);
                gameObject.transform.localPosition = Vector3.Lerp(originalPosition, destinationPosition, currentTime / TimePauseInSecs);
                currentTime += Time.deltaTime;
            }
            yield return null;
        } while (currentTime <= TimePauseInSecs);

        callback.Invoke();
    }

    public void SetCircleVisibility(bool isVisible)
    {
        _outerCircle.SetActive(isVisible);
    }
}