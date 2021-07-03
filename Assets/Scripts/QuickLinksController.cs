using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class QuickLinksController : UIMenuController
{
    private const string MainMenuScene = "IntroScene";
    private const string SettingsScene = "SettingsScene";
    private const string SelectPracticeScene = "SelectPracticeScene";
    private const string RateScene = "RateScene";
    private const string QuestionnaireScene = "Questionnaire";

    private GameObject back;
    private GameObject settings;
    private GameObject sound;

    private Sprite soundOn, soundOff;

    // Start is called before the first frame update
    void Start()
    {
        var SceneName = SceneManager.GetActiveScene().name;
        CurrentMenu = this;
        back = GameObject.FindGameObjectWithTag("Back");
        if (SceneName == MainMenuScene)
        {
            back.SetActive(false);
        } else
        {
            back.SetActive(true);
        }

        settings = GameObject.FindGameObjectWithTag("Settings");
        if (SceneName == SettingsScene || SceneName == SelectPracticeScene || SceneName == RateScene || SceneName == QuestionnaireScene)
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

        if (SceneManager.GetActiveScene().name == MainMenuScene || SceneName == SelectPracticeScene || SceneName == RateScene || SceneName == QuestionnaireScene)
        {
            sound.SetActive(false);
        }
        else
        {
            sound.SetActive(true);
        }

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
