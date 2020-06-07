using UnityEngine;

public class SettingsController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RemoveHistory()
    {
        SaveSystem.Remove();
    }
}
