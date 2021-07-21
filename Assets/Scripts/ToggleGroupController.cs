using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleGroupController : MonoBehaviour
{
    private static Dictionary<string, ToggleCoordinates> verticalCoordinates = new Dictionary<string, ToggleCoordinates>()
    {
        {"Toggle", new ToggleCoordinates {X = -33f, Y = -4.2f, Width = 75f, Height = 20f}},
        {"Toggle (1)", new ToggleCoordinates {X = -0.7f, Y = -32.4f, Width = 110f, Height = 20f}},
        {"Toggle (2)", new ToggleCoordinates {X = 43f, Y = -4.3f, Width = 60f, Height = 25f}},
        {"Toggle (3)", new ToggleCoordinates {X = -0.8f, Y = -56.6f, Width = 100f, Height = 20f}},
        {"Toggle (4)", new ToggleCoordinates {X = -2.6f, Y = -80.3f, Width = 120f, Height = 25f}},
        {"Toggle (5)", new ToggleCoordinates {X = 0.5f, Y = -104.8f, Width = 70f, Height = 20f}},
        {"Toggle (6)", new ToggleCoordinates {X = 1.2f, Y = -130.2f, Width = 110f, Height = 25f}}
    };

    private static Dictionary<string, ToggleCoordinates> horizontalCoordinates = new Dictionary<string, ToggleCoordinates>()
    {
        {"Toggle", new ToggleCoordinates {X = -65.1f, Y = -4.2f, Width = 120f, Height = 20f}},
        {"Toggle (1)", new ToggleCoordinates {X = 5.6f, Y = -32.4f, Width = 220f, Height = 20f}},
        {"Toggle (2)", new ToggleCoordinates {X = 69.7f, Y = -4.3f, Width = 140f, Height = 25f}},
        {"Toggle (3)", new ToggleCoordinates {X = 8.5f, Y = -56.6f, Width = 200f, Height = 20f}},
        {"Toggle (4)", new ToggleCoordinates {X = -49.1f, Y = -96.2f, Width = 100f, Height = 50f}},
        {"Toggle (5)", new ToggleCoordinates {X = 58.3f, Y = -96.6f, Width = 80f, Height = 40f}},
        {"Toggle (6)", new ToggleCoordinates {X = 0.7f, Y = -138f, Width = 170f, Height = 25f}}
    };
    private const int verticalFontSize = 8, horizontalFontSize = 12;

    private bool isHorizontal;

    void Start()
    {
        isHorizontal = Screen.width > Screen.height;
        SetTogglePositions();
    }

    // Update is called once per frame
    void Update()
    {
        var newHorizontal = Screen.width > Screen.height;
        if (isHorizontal != newHorizontal)
        {
            isHorizontal = newHorizontal;
            SetTogglePositions();
        }
    }

    private void SetTogglePositions()
    {
        var toggleCoordinates = isHorizontal ? horizontalCoordinates : verticalCoordinates;
        var fontSize = isHorizontal ? horizontalFontSize : verticalFontSize;

        foreach (var question in GetComponentsInChildren<QuestionController>())
        {
            var toggleCoordinate = toggleCoordinates[question.name];
            var rectTransform = question.GetComponent<RectTransform>();
            rectTransform.localPosition = new Vector3(toggleCoordinate.X, toggleCoordinate.Y, 0f);
            rectTransform.sizeDelta = new Vector2(toggleCoordinate.Width, toggleCoordinate.Height);
            question.ChangeFontSize(fontSize);
        }
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
        UIMenuController.StaticLoadScene("RateScene");
    }
}

public class ToggleCoordinates
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Width { get; set; }
    public float Height { get; set; }
}