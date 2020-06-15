using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ToggleGroupController : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPlay()
    {
        var answers = new Dictionary<QuestionnaireQuestion, bool>();
        foreach (var question in GetComponentsInChildren<QuestionController>())
        {
            var toggle = question.GetComponent<Toggle>();
            answers.Add(question.question, toggle.isOn);
        }

        var practice = GameStateManager.Instance.GetQuestionnairePractice(answers);
        GameStateManager.Instance.SelectPractice(practice);
        UIMenuController.StaticLoadScene("TimerScene");
    }
}