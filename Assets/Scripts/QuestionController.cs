using UnityEngine;

public class QuestionController : MonoBehaviour
{
    public QuestionnaireQuestion question;

    // Start is called before the first frame update
    void Start()
    {
        var text = GetComponentInChildren<LocalizedText>();
        text.SetupKey(question.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
