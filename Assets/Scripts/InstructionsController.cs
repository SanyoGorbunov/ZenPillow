using System.Collections;
using UnityEngine;

public class InstructionsController : MonoBehaviour
{
    public GameObject bubble;

    public GameObject inhaleText;
    public GameObject exhaleText;
    public GameObject holdIcon;

    public float inhaleTextVerticalXPos = 20.0f;
    public float inhaleTextHorizontalXPos = 0.0f;

    public float inhaleTextVerticalYPos = 90.7f;

    public float exhaleTextVerticalXPos = -20.0f;
    public float exhaleTextHorizontalXPos = 00.0f;

    public void SetVisibility(bool isVisible)
    {
        bubble.GetComponent<CanvasRenderer>().SetAlpha(isVisible ? 1 : 0);
        //inhaleText.GetComponent<CanvasRenderer>().SetAlpha(isVisible ? 1 : 0);
        SetTextIsHorizontal(!isVisible);

        //exhaleText.GetComponent<CanvasRenderer>().SetAlpha(isVisible ? 1 : 0);
        holdIcon.GetComponent<CanvasRenderer>().SetAlpha(isVisible ? 1 : 0);
    }

    private void SetTextIsHorizontal(bool isHorizontal)
    {
        if (isHorizontal)
        {
            UnityEngine.UI.Text text = inhaleText.GetComponent<UnityEngine.UI.Text>();
            text.horizontalOverflow = HorizontalWrapMode.Overflow;
            Vector3 pos = inhaleText.transform.localPosition;
            pos.x = inhaleTextHorizontalXPos;
            pos.y = exhaleText.transform.localPosition.y;
            inhaleText.transform.localPosition = pos;
            text = exhaleText.GetComponent<UnityEngine.UI.Text>();
            text.horizontalOverflow = HorizontalWrapMode.Overflow;
            pos = exhaleText.transform.localPosition;
            pos.x = exhaleTextHorizontalXPos;
            exhaleText.transform.localPosition = pos;
        }
        else
        {
            UnityEngine.UI.Text text = inhaleText.GetComponent<UnityEngine.UI.Text>();
            text.horizontalOverflow = HorizontalWrapMode.Wrap;
            Vector3 pos = inhaleText.transform.localPosition;
            pos.x = inhaleTextVerticalXPos;
            pos.y = inhaleTextVerticalYPos;
            inhaleText.transform.localPosition = pos;
            text = exhaleText.GetComponent<UnityEngine.UI.Text>();
            text.horizontalOverflow = HorizontalWrapMode.Wrap;
            pos = exhaleText.transform.localPosition;
            pos.x = exhaleTextVerticalXPos;
            exhaleText.transform.localPosition = pos;
        }
    }

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
        StartCoroutine(nameof(Inhale));
    }

    public void ShowExhale()
    {
        Reset();
        StartCoroutine(nameof(Exhale));
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
