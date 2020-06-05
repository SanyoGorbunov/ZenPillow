using UnityEngine;
using UnityEngine.UI;

public class LocaleSwitcher : MonoBehaviour
{
    public string Locale;

    // Start is called before the first frame update
    void Start()
    {
        var button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(() =>
            {
                if (Mgl.I18n.GetLocale() != Locale)
                {
                    Mgl.I18n.SetLocale(Locale);
                }
            });
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
