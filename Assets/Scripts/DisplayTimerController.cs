using UnityEngine;
using UnityEngine.UI;

public class DisplayTimerController : MonoBehaviour
{
    private float _currentTime;

    private Text _timerText;

    // Start is called before the first frame update
    void Start()
    {
        _currentTime = 0.0f;
        _timerText = gameObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        _currentTime += Time.deltaTime;

        float minutes = Mathf.FloorToInt(_currentTime / 60);
        float seconds = Mathf.FloorToInt(_currentTime % 60);
        _timerText.text = string.Format("{0}:{1:00}", minutes, seconds);
    }
}
