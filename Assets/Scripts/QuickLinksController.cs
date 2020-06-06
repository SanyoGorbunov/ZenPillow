using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class QuickLinksController : MonoBehaviour
{
    private const string MainMenuScene = "IntroScene";

    private GameObject back;
    private GameObject sound;

    // Start is called before the first frame update
    void Start()
    {
        back = GameObject.FindGameObjectWithTag("Back");
        if (SceneManager.GetActiveScene().name == MainMenuScene)
        {
            back.SetActive(false);
        } else
        {
            back.SetActive(true);
        }

        sound = GameObject.FindGameObjectWithTag("Sound");
        var soundButton = sound.GetComponent<Button>();
        soundButton.onClick.AddListener(() =>
        {
            AudioManager.StaticToggleMute();
        });
    }

    public void Back()
    {
        SceneManager.LoadScene(MainMenuScene);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
