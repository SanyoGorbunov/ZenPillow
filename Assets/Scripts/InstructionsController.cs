using System.Collections;
using UnityEngine;

public class InstructionsController : MonoBehaviour
{
    public GameObject bubble;

    public GameObject inhaleText;
    public GameObject exhaleText;
    public GameObject holdIcon;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetInstructionsVisibility(bool isVisible)
    {
        bubble.SetActive(isVisible);
    }

    public void ShowInhale()
    {
        Reset();
        StartCoroutine("Inhale");
    }

    public void ShowExhale()
    {
        Reset();
        StartCoroutine("Exhale");
    }

    public void ShowHold()
    {
        Reset();
        holdIcon.SetActive(true);
    }

    IEnumerator Inhale()
    {
        inhaleText.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        inhaleText.SetActive(false);
    }

    IEnumerator Exhale()
    {
        exhaleText.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        exhaleText.SetActive(false);
    }

    public void Reset()
    {
        inhaleText.SetActive(false);
        exhaleText.SetActive(false);
        holdIcon.SetActive(false);
    }
}
