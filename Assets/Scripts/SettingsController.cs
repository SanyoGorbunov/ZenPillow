using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    public Toggle useSoftColorsToggle;
    public Toggle showMoreSheepToggle;
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
            var playerData = SaveSystem.Load();
            useSoftColorsToggle.isOn = playerData.useSoftColors;
            showMoreSheepToggle.isOn = playerData.showMoreSheep;
            isInitialized = true;
        }
    }

    public void ChangeUseSoftColors(bool useSoftColors)
    {
        SaveSystem.SaveSoftColors(useSoftColors);
    }

    public void ChangeMoreSheep(bool showMoreSheep)
    {
        SaveSystem.SaveMoreSheep(showMoreSheep);
    }

    public void RemoveHistory()
    {
        SaveSystem.Remove();

        var locale = Mgl.I18n.MapSystemLanguage(Application.systemLanguage);
        Mgl.I18n.SetLocale(locale);
        SaveSystem.SaveLocale(locale);

        isInitialized = false;
    }
}
