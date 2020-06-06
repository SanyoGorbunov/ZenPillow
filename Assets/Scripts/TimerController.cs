using UnityEngine;

public class TimerController : MonoBehaviour
{
    public int minutes;

    // Start is called before the first frame update
    void Start()
    {
        var localizedText = GetComponentInChildren<LocalizedText>();
        if (localizedText == null)
        {
            Debug.LogError("Can't find Localized Text");
        } else
        {
            localizedText.SetupKey(minutes + " min");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        GameStateManager.Instance.SelectTime(minutes);

        foreach (TimerController timer in transform.parent.GetComponentsInChildren<TimerController>())
        {
            timer.transform.localScale = new Vector3(0.6f, 0.6f, 1.0f);
        }

        var timerPlayerController = transform.parent.GetComponentInChildren<TimerPlayController>();
        if (timerPlayerController != null)
        {
            timerPlayerController.Activate();
        }
        
        transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    }
}
