using System;
using UnityEngine;
using UnityEngine.UI;

public class DisplayTimerController : MonoBehaviour
{
    private float _remainingTime;
    private bool _isActive;
    private Action _onElapsed;

    private Text _timerText;

    // Start is called before the first frame update
    void Start()
    {
        _timerText = gameObject.GetComponent<Text>();
        if (!_isActive)
        {
            gameObject.GetComponent<CanvasRenderer>().SetAlpha(0);
        }
    }

    public void Activate(float remainingTime, Action onElapsed)
    {
        _remainingTime = remainingTime;
        _onElapsed = onElapsed;
        _isActive = true;
        gameObject.GetComponent<CanvasRenderer>().SetAlpha(1);
    }

    // Update is called once per frame
    void Update()
    {
        if (_isActive)
        {
            _remainingTime -= Time.deltaTime;

            if (_remainingTime < 0.0f)
            {
                _isActive = false;
                _onElapsed();
                return;
            }

            float minutes = Mathf.FloorToInt(_remainingTime / 60);
            float seconds = Mathf.FloorToInt(_remainingTime % 60);
            _timerText.text = string.Format("{0}:{1:00}", minutes, seconds);
        }
    }

    public void Pause()
    {
        _isActive = false;
    }

    public void Continue()
    {
        _isActive = true;
    }
}
