using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class QuickLinksController : UIMenuController
{
    private const string MainMenuScene = "IntroScene";
    private const string SettingsScene = "SettingsScene";

    private GameObject back;
    private GameObject settings;
    private GameObject sound;

    private Sprite soundOn, soundOff;

    // Start is called before the first frame update
    void Start()
    {
        CurrentMenu = this;
        back = GameObject.FindGameObjectWithTag("Back");
        if (SceneManager.GetActiveScene().name == MainMenuScene)
        {
            back.SetActive(false);
        } else
        {
            back.SetActive(true);
        }

        settings = GameObject.FindGameObjectWithTag("Settings");
        if (SceneManager.GetActiveScene().name == SettingsScene)
        {
            settings.SetActive(false);
        }
        else
        {
            settings.SetActive(true);
        }

        sound = GameObject.FindGameObjectWithTag("Sound");
        var soundButton = sound.GetComponent<Button>();
        soundOn = Resources.Load<Sprite>("Icons/sound_on");
        soundOff = Resources.Load<Sprite>("Icons/sound_off");

        var isMute = true;

        if (AudioManager.instance)
        {
            isMute = AudioManager.instance.GetMute();
        }
         
        if (isMute)
        {
            soundButton.GetComponent<Image>().sprite = soundOff;
        }
        else
        {
            soundButton.GetComponent<Image>().sprite = soundOn;
        }
        soundButton.onClick.AddListener(() =>
        {
            AudioManager.StaticToggleMute();
            var isMute2 = AudioManager.instance.GetMute();
            if (isMute2)
            {
                soundButton.GetComponent<Image>().sprite = soundOff;
            } else
            {
                soundButton.GetComponent<Image>().sprite = soundOn;
            }
        });
        AnimatedStart();
    }

    public void Back()
    {
        LoadScene(MainMenuScene);
    }

    public void GoToSettings()
    {
        LoadScene(SettingsScene);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
