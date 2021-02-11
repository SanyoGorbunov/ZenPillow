using UnityEngine;
using UnityEngine.UI;

public class LocalizedText : MonoBehaviour
{
    public string Key;
    public string KeyHorizontal;

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

    public void SetupKey(string key)
    {
        Key = key;
        SetText();
    }

    public void Reload()
    {
        SetText();
    }

    void SetText()
    {
        if (gameObject != null)
        {
            var textObject = gameObject.GetComponent<Text>();
            if (textObject != null)
            {
                if (string.IsNullOrEmpty(KeyHorizontal))
                {
                    textObject.text = Mgl.I18n.Instance.__(Key);
                }
                else
                {
                    textObject.text = Screen.width > Screen.height
                        ? Mgl.I18n.Instance.__(KeyHorizontal)
                        : Mgl.I18n.Instance.__(Key);
                }
            }
        }
    }
}
