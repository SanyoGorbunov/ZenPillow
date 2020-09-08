using UnityEngine;
using UnityEngine.UI;

public class SaveRateController : MonoBehaviour
{
    public void OnClick()
    {
        var rate = GameObject.FindGameObjectWithTag("Rate");
        int value = int.Parse(rate.GetComponent<Text>().text);

        if (GameStateManager.Instance.IsGameStarted)
        {
            GameStateManager.Instance.SaveGame(value);
            UIMenuController.StaticLoadScene("IntroScene");
        }
        else
        {
            GameStateManager.Instance.RateBeforeGame(value);
            UIMenuController.StaticLoadScene("TimerScene");
        }
    }

    public void OnRateChange(float value)
    {
        var rate = GameObject.FindGameObjectWithTag("Rate");
        if (rate != null)
        {
            rate.GetComponent<Text>().text = ((int)value).ToString();
        }
    }
}
