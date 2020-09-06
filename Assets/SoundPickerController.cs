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

        var currentIndex = GetCurrentSoundIndex();
        dropdown.SetValueWithoutNotify(currentIndex);
    }

    public void OnValueChanged(int index)
    {
        AudioManager.instance.Change(_sounds[index]);
    }

    // Update is called once per frame

    void Update()
    {
        
    }

    private int GetCurrentSoundIndex()
    {
        var currentSound = AudioManager.instance.GetCurrentBackgroundSound();
        var currentIndex = 0;
        for (int i = 1; i < _sounds.Length; i++)
        {
            if (_sounds[i] == currentSound)
            {
                currentIndex = i;
                break;
            }
        }

        return currentIndex;
    }
}
