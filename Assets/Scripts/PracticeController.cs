using UnityEngine;
using UnityEngine.SceneManagement;

public class PracticeController : UIMenuController
{
    public Practice practice;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(AnimateAlpha(1, 0, 0.5f, new del(() => { overlay.SetActive(false); })));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick(int practice)
    {
        Practice PracticeEnum = (Practice)practice;

        GameStateManager.Instance.SelectPractice(PracticeEnum);
        LoadScene("TimerScene");
    }
}
