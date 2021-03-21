using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MainMenuController : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        var playerData = SaveSystem.Load();
        var savedLocale = playerData.locale;

        if (string.IsNullOrEmpty(savedLocale))
        {
            savedLocale = Mgl.I18n.MapSystemLanguage(Application.systemLanguage);
        }

        if (Mgl.I18n.GetLocale() != savedLocale)
        {
            Mgl.I18n.SetLocale(savedLocale);
            SaveSystem.SaveLocale(savedLocale);
        }
    }

    public void SelectPractice()
    {
        UIMenuController.StaticLoadScene("SelectPracticeScene");
    }

    public void OpenQuestionnaire()
    {
        UIMenuController.StaticLoadScene("Questionnaire");
    }

    public void OpenFavoritePractice()
    {
        var practice = GameStateManager.Instance.GetFavoritePractice();
        GameStateManager.Instance.SelectPractice(practice);

        UIMenuController.StaticLoadScene("RateScene");
    }
}
