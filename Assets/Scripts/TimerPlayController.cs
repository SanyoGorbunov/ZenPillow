using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TimerPlayController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Activate()
    {
        var button = GetComponent<Button>();
        if (button != null && !button.interactable)
        {
            button.interactable = true;
        }
    }

    public void OnClick()
    {
        var selectedPractice = GameStateManager.Instance.GetPractice();
        switch (selectedPractice)
        {
            case Practice.Breathing:
                SceneManager.LoadScene("BreathingScene");
                break;
            case Practice.Collecting:
                SceneManager.LoadScene("RabbitJump");
                break;
            case Practice.Counting:
                break;
        }
    }
}
