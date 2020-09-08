using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MainMenuController : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
    }

    public void SelectPractice()
    {
        UIMenuController.StaticLoadScene("SelectPracticeScene");
    }

    public void OpenQuestionnaire()
    {
        UIMenuController.StaticLoadScene("Questionnaire");
    }

    public void OpenFavoriteRoom()
    {
        var practice = GameStateManager.Instance.GetFavoritePractice();
        GameStateManager.Instance.SelectPractice(practice);

        UIMenuController.StaticLoadScene("RateScene");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
