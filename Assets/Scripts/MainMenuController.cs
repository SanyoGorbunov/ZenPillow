using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MainMenuController : MonoBehaviour
{
    public GameObject overlay;

    public delegate void del();

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(AnimateAlpha(1, 0, 0.5f, new del(() => { overlay.SetActive(false); })));
    }

    private void LoadScene(string SceneName)
    {
        overlay.SetActive(true);
        StartCoroutine(AnimateAlpha(0, 1, 0.5f, new del(() => { SceneManager.LoadScene(SceneName); })));
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

    private IEnumerator AnimateAlpha(float startValue, float endValue, float duration, del action)
    {
        float elapsedTime = 0;
        float ratio = elapsedTime / duration;
        while (ratio < 1f)
        {
            elapsedTime += Time.deltaTime;
            ratio = elapsedTime / duration;

            float alpha = startValue + (endValue - startValue) * ratio;

            Color temp = Color.white;
            temp.a = alpha;

            overlay.GetComponent<Image>().color = temp;

            yield return null;
        }

        if (action != null)
        {
            action();
        }        
    }
}
