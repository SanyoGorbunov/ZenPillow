using System.Collections;
using UnityEngine;
using Utils;

public class InstructionsPressController : MonoBehaviour
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
    private bool isStartTutorialActive;

    public void RotateScreen(bool toHorizontal)
    {
		
        if (isHorizontal != toHorizontal)
        {
            isHorizontal = toHorizontal;
            SetStartTutorialActive(isStartTutorialActive);
        }      
    }

    private void RotateTextToHorizontal(bool toHorizontal)
    {
		/*
        if (toHorizontal)
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
        }*/
    }

    public void SetStartTutorialActive(bool isActive)
    {
        exhaleText.SetActive(false);
        inhaleText.SetActive(false);
        isStartTutorialActive = isActive;

        bubble.SetActive(isActive);
        RotateTextToHorizontal(isHorizontal);
    }

    public void ShowInhale()
    {
        exhaleText.SetActive(false);
        inhaleText.SetActive(true);
        /*
        Reset();
        StartCoroutine(nameof(Inhale));
		*/
    }

    public void ShowExhale()
    {
        exhaleText.SetActive(true);
        inhaleText.SetActive(false);
        /*
        Reset();
        StartCoroutine(nameof(Exhale));*/
    }

    public void ShowHold()
    {
		/*
        Reset();
        holdIcon.SetActive(!isHorizontal);
        StartCoroutine(nameof(Hold));*/
    }

    IEnumerator Inhale()
    {
		
        //inhaleText.SetActive(true);
        yield return new WaitForSecondsPaused(2.0f);
        //inhaleText.SetActive(false);*/
    }

    IEnumerator Exhale()
    {
        //exhaleText.SetActive(true);
        yield return new WaitForSecondsPaused(2.0f);
        //exhaleText.SetActive(false);
    }

    IEnumerator Hold()
    {
        //holdText.SetActive(true);
        yield return new WaitForSecondsPaused(2.0f);
        //holdText.SetActive(false);
    }

    public void Reset()
    {
		/*
        inhaleText.SetActive(false);
        exhaleText.SetActive(false);
        holdIcon.SetActive(false);
        holdText.SetActive(false);
		*/
    }
}
