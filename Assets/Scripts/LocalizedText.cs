using UnityEngine;
using UnityEngine.UI;

public class LocalizedText : MonoBehaviour
{
    public string Key;

    // Start is called before the first frame update
    void Start()
    {
        if (Key != null)
        {
            SetupKey(Key);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        Mgl.I18n.LocaleSet -= SetText;
    }

    public void SetupKey(string key)
    {
        Key = key;
        SetText();
        Mgl.I18n.LocaleSet += SetText;
    }

    void SetText()
    {
        var textObject = gameObject.GetComponent<Text>();
        if (textObject != null)
        {
            textObject.text = Mgl.I18n.Instance.__(Key);
        }
    }
}
