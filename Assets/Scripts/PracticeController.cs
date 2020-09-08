using UnityEngine;

public class PracticeController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick(int practice)
    {
        Practice PracticeEnum = (Practice)practice;

        GameStateManager.Instance.SelectPractice(PracticeEnum);
        UIMenuController.StaticLoadScene("RateScene");
    }
}
