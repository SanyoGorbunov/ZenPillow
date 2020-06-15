using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MainMenuController : UIMenuController
{

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(AnimateAlpha(1, 0, 0.5f, new del(() => { overlay.SetActive(false); })));
    }

    public void SelectPractice()
    {
        LoadScene("SelectPracticeScene");
    }

    public void OpenQuestionnaire()
    {
        LoadScene("Questionnaire");
    }

    public void OpenFavoriteRoom()
    {
        var practice = GameStateManager.Instance.GetFavoritePractice();
        GameStateManager.Instance.SelectPractice(practice);

        LoadScene("TimerScene");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
