using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class QuickLinksWithPauseController : UIMenuController
{
    public GameObject pauseOverlay;
    public GameObject pausePanel;
    public bool enablePauseFromStart = false;

    private const string MainMenuScene = "IntroScene";
    private const string SettingsScene = "SettingsScene";

    private GameObject back;
    private GameObject settings;
    private GameObject sound;

    private Sprite soundOn, soundOff;

    DisplayTimerController displayTimerController;

    bool isHorizontal;

    private Vector3 verticalPauseScale = new Vector3(0.8f, 0.8f, 0.8f);
    private Vector3 horizontalPauseScale = new Vector3(1.0f, 1.0f, 1.0f);

    public void SetIsHorizontal(bool isHorizontal)
    {
        this.isHorizontal = isHorizontal;
        if (pausePanel.activeInHierarchy)
        {
            pausePanel.transform.localScale = isHorizontal ? horizontalPauseScale : verticalPauseScale;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        displayTimerController = FindObjectOfType<DisplayTimerController>();
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
        if (SceneManager.GetActiveScene().name == SettingsScene || enablePauseFromStart)
        {
            settings.SetActive(false);
        }
        else
        {
            settings.SetActive(true);
        }

        settings.GetComponent<Button>().onClick.AddListener(() =>
        {
            OpenPauseMenu();
        });

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
        pauseOverlay.SetActive(false);
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

    public void OpenPauseMenu()
    {
        settings.SetActive(false);
        displayTimerController.Pause();
        pauseOverlay.SetActive(true);
        pausePanel.transform.localScale = pausePanel.transform.localScale = isHorizontal ? horizontalPauseScale : verticalPauseScale;
        StartCoroutine(AnimateAlpha(0, 1, 0.3f, null));
        //pauseOverlay.GetComponent<CanvasGroup>().alpha = 0.5f;
    }

    public void DisplayPause()
    {
        settings.SetActive(true);
    }

    public void Continue()
    {
        StartCoroutine(AnimateAlpha(1, 0, 0.1f, new del(()=>{
            pauseOverlay.SetActive(false);
            settings.SetActive(true);
            displayTimerController.Continue();
        })));
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected IEnumerator AnimateAlpha(float startValue, float endValue, float duration, del action)
    {
        float elapsedTime = 0;
        float ratio = elapsedTime / duration;
        while (ratio < 1f)
        {
            elapsedTime += Time.deltaTime;
            ratio = elapsedTime / duration;

            float alpha = startValue + (endValue - startValue) * ratio;

            pauseOverlay.GetComponent<CanvasGroup>().alpha = alpha;

            yield return null;
        }

        if (action != null)
        {
            action();
        }
    }
}
