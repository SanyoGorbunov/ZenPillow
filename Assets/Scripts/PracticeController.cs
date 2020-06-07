using UnityEngine;
using UnityEngine.SceneManagement;

public class PracticeController : MonoBehaviour
{
    public Practice practice;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        GameStateManager.Instance.SelectPractice(practice);
        SceneManager.LoadScene("TimerScene");
    }
}
