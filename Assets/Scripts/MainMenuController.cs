using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MainMenuController : MonoBehaviour
{
    public GameObject SelectPracticeButton;
    public GameObject AnswerQuestionsButton;
    public GameObject FavoritePracticeButton;

    private float HideButtonYOffset = 20.0f;
    // Start is called before the first frame update
    void Start()
    {
        var playerData = SaveSystem.Load();
        var savedLocale = playerData.locale;

        if (FavoritePracticeButton != null)
        {
            if (playerData.records == null || playerData.records.Length == 0)
            {
                RelayoutMenu();
            }
        }

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

    private void RelayoutMenu()
    {
        if (FavoritePracticeButton)
        {
            FavoritePracticeButton.SetActive(false);
        }

        if (SelectPracticeButton && AnswerQuestionsButton)
        {
            Transform tempTransform = SelectPracticeButton.transform;
            SelectPracticeButton.transform.localPosition = new Vector2(tempTransform.localPosition.x, tempTransform.localPosition.y - HideButtonYOffset);
            tempTransform = AnswerQuestionsButton.transform;
            AnswerQuestionsButton.transform.localPosition = new Vector2(tempTransform.localPosition.x, tempTransform.localPosition.y - HideButtonYOffset);
        }
    }
}
