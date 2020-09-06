using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SoundPickerController : MonoBehaviour
{
    private string[] _sounds;

    // Start is called before the first frame update
    void Start()
    {
        var dropdown = gameObject.GetComponent<Dropdown>();
        _sounds = AudioManager.instance.GetBackgroundSounds();
        dropdown.options = _sounds.Select(sound => new Dropdown.OptionData(sound)).ToList();
    }

    public void OnValueChanged(int index)
    {
        AudioManager.instance.Change(_sounds[index]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
