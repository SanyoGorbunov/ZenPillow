using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class UIMenuController : MonoBehaviour
{
    public GameObject overlay;

    public delegate void del();
    protected void LoadScene(string SceneName)
    {
        overlay.SetActive(true);
        StartCoroutine(AnimateAlpha(0, 1, 0.5f, new del(() => { SceneManager.LoadScene(SceneName); })));
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
