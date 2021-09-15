using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    public Toggle useSoftColorsToggle;
    private bool isInitialized;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isInitialized)
        {
            useSoftColorsToggle.isOn = SaveSystem.Load().useSoftColors;
            isInitialized = true;
        }
    }

    public void ChangeUseSoftColors(bool useSoftColors)
    {
        SaveSystem.SaveSoftColors(useSoftColors);
    }

    public void RemoveHistory()
    {
        SaveSystem.Remove();

        var locale = Mgl.I18n.MapSystemLanguage(Application.systemLanguage);
        Mgl.I18n.SetLocale(locale);
        SaveSystem.SaveLocale(locale);

        useSoftColorsToggle.isOn = false;
    }
}
