using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SelectPractice()
    {
        SceneManager.LoadScene("SelectPracticeScene");
    }

    public void OpenFavoriteRoom()
    {
        var practice = GameStateManager.Instance.GetFavoritePractice();
        GameStateManager.Instance.SelectPractice(practice);
        SceneManager.LoadScene("TimerScene");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
