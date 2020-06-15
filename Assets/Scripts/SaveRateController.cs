using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveRateController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        var rate = GameObject.FindGameObjectWithTag("Rate");
        int value = int.Parse(rate.GetComponent<Text>().text);

        GameStateManager.Instance.SaveGame(value);
        UIMenuController.StaticLoadScene("IntroScene");
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
