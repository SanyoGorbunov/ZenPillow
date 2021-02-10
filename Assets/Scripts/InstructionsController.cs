using System.Collections;
using UnityEngine;

public class InstructionsController : MonoBehaviour
{
    public GameObject bubble;

    public GameObject inhaleText;
    public GameObject exhaleText;
    public GameObject holdText;
    public GameObject holdIcon;

    public float textHorizontalXPos = 0.0f;

    public float textVerticalXPos = 0.0f;
    public float textVerticalYPos = -140.0f;

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
            pos.x = textHorizontalXPos;
            pos.y = exhaleText.transform.localPosition.y;
            inhaleText.transform.localPosition = pos;

            text = exhaleText.GetComponent<UnityEngine.UI.Text>();
            text.horizontalOverflow = HorizontalWrapMode.Overflow;
            pos = exhaleText.transform.localPosition;
            pos.x = textHorizontalXPos;
            exhaleText.transform.localPosition = pos;

            text = holdText.GetComponent<UnityEngine.UI.Text>();
            text.horizontalOverflow = HorizontalWrapMode.Overflow;
            pos = holdText.transform.localPosition;
            pos.x = textHorizontalXPos;
            holdText.transform.localPosition = pos;
        }
        else
        {
            UnityEngine.UI.Text text = inhaleText.GetComponent<UnityEngine.UI.Text>();
            text.horizontalOverflow = HorizontalWrapMode.Wrap;
            Vector3 pos = inhaleText.transform.localPosition;
            pos.x = textVerticalXPos;
            pos.y = textVerticalYPos;
            inhaleText.transform.localPosition = pos;

            text = exhaleText.GetComponent<UnityEngine.UI.Text>();
            text.horizontalOverflow = HorizontalWrapMode.Wrap;
            pos = exhaleText.transform.localPosition;
            pos.x = textVerticalXPos;
            pos.y = textVerticalYPos;
            exhaleText.transform.localPosition = pos;

            text = holdText.GetComponent<UnityEngine.UI.Text>();
            text.horizontalOverflow = HorizontalWrapMode.Wrap;
            pos = holdText.transform.localPosition;
            pos.x = textVerticalXPos;
            pos.y = textVerticalYPos;
            holdText.transform.localPosition = pos;
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

    public void ShowHold(bool showHoldIcon)
    {
        Reset();
        if (showHoldIcon)
        {
            holdIcon.SetActive(true);
        }
        StartCoroutine(nameof(Hold));
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

    IEnumerator Hold()
    {
        holdText.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        holdText.SetActive(false);
    }

    public void Reset()
    {
        inhaleText.SetActive(false);
        exhaleText.SetActive(false);
        holdIcon.SetActive(false);
        holdText.SetActive(false);
    }
}
