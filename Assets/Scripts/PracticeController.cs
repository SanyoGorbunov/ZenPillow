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

        switch (PracticeEnum)
        {
            case Practice.Counting:
            case Practice.SimplifiedCounting:
                PracticeEnum = SaveSystem.Load().showMoreSheep ? Practice.Counting : Practice.SimplifiedCounting;
                break;
        }

        GameStateManager.Instance.SelectPractice(PracticeEnum);
        UIMenuController.StaticLoadScene("RateScene");
    }
}
