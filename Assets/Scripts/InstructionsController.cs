using System.Collections;
using UnityEngine;

public class InstructionsController : MonoBehaviour
{
    public GameObject bubble;
    public GameObject instructionsHorizontal;

    public GameObject inhaleText;
    public GameObject exhaleText;
    public GameObject holdText;
    public GameObject holdIcon;

    public float textHorizontalXPos = 0.0f;

    public float textVerticalXPos = 0.0f;
    public float textVerticalYPos = -140.0f;

    private bool isHorizontal;
    private bool isVisible = true;
    private bool isStartTutorialActive;

    public void SetVisibility(bool isVisible)
    {
        this.isVisible = isVisible;

        if (isVisible)
        {
            if (isHorizontal)
            {
                bubble.SetActive(false);
                instructionsHorizontal.SetActive(true);
                //inhaleText.GetComponent<CanvasRenderer>().SetAlpha(isVisible ? 1 : 0);
                SetTextIsHorizontal(true);

                //exhaleText.GetComponent<CanvasRenderer>().SetAlpha(isVisible ? 1 : 0);
                holdIcon.GetComponent<CanvasRenderer>().SetAlpha(0);
            }
            else
            {
                bubble.SetActive(isStartTutorialActive);
                instructionsHorizontal.SetActive(false);
                //inhaleText.GetComponent<CanvasRenderer>().SetAlpha(isVisible ? 1 : 0);
                SetTextIsHorizontal(false);

                //exhaleText.GetComponent<CanvasRenderer>().SetAlpha(isVisible ? 1 : 0);
                holdIcon.GetComponent<CanvasRenderer>().SetAlpha(1);
            }
        }
    }

    public void SetHorizontalMode(bool isHorizontal)
    {
        if (this.isHorizontal != isHorizontal)
        {
            this.isHorizontal = isHorizontal;

            if (isVisible)
            {
                if (isHorizontal)
                {
                    bubble.SetActive(false);
                    instructionsHorizontal.SetActive(isStartTutorialActive? true : false);
                    //inhaleText.GetComponent<CanvasRenderer>().SetAlpha(isVisible ? 1 : 0);
                    SetTextIsHorizontal(true);

                    //exhaleText.GetComponent<CanvasRenderer>().SetAlpha(isVisible ? 1 : 0);
                    holdIcon.GetComponent<CanvasRenderer>().SetAlpha(0);
                }
                else
                {
                    bubble.SetActive(isStartTutorialActive);
                    instructionsHorizontal.SetActive(false);
                    //inhaleText.GetComponent<CanvasRenderer>().SetAlpha(isVisible ? 1 : 0);
                    SetTextIsHorizontal(false);

                    //exhaleText.GetComponent<CanvasRenderer>().SetAlpha(isVisible ? 1 : 0);
                    holdIcon.GetComponent<CanvasRenderer>().SetAlpha(1);
                }
            }            
        }        
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
        isStartTutorialActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetInstructionsVisibility(bool isVisible)
    {
        isStartTutorialActive = isVisible;

        if (isVisible)
        {
            bubble.SetActive(!isHorizontal);
            instructionsHorizontal.SetActive(isHorizontal);
        }
        else
        {
            bubble.SetActive(false);
            instructionsHorizontal.SetActive(false);
        }
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
