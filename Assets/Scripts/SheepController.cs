using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SheepController : MonoBehaviour
{

    public bool hasCarrot = false;
    private GameObject numberText = null;

    public int number = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        numberText = transform.Find("Canvas").Find("Number").gameObject;
    }

    public void showCarrot()
    {
        if (numberText) numberText.SetActive(true);
        hasCarrot = true;
    }

    public void setNumber(int number)
    {
        this.numberText.GetComponent<Text>().text = number.ToString();
        this.number = number;
    }

    public void hideCarrot()
    {
        //if (numberText) numberText.SetActive(false);
        hasCarrot = false;
    }

    public void setSize(float scale)
    {
        this.transform.localScale = new Vector3(scale, 0.1f, scale);

        //if(numberText) numberText.transform.localScale = new Vector3(0.1f/ scale, 0.1f / scale, 0.1f / scale);
    }
    public void OnClick()
    {
        CountSheepsController contr = FindObjectOfType<CountSheepsController>();
        if (contr.NextNumber == number)
        {
            contr.ClickSheep();
            Destroy();
        }
    }

    public void ActivateHint()
    {
        StartCoroutine(AnimateSizeAndRot(transform.localScale, transform.localScale * 1.1f, 0.4f));
    }

    public void Enlarge()
    {
        StartCoroutine(EnlargeSize(transform.localScale, transform.localScale * 1.5f, 2.5f));
    }

    bool isDestroyed = false;

    public void Destroy()
    {
        if (isDestroyed)
        { 
            return;
        }
        StopAllCoroutines();
        Destroy(gameObject);
        isDestroyed = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private IEnumerator AnimateSizeAndRot(Vector3 startValue, Vector3 endValue, float duration)
    {
        float elapsedTime = 0;
        float ratio = elapsedTime / duration;
        while (ratio < 1f)
        {
            elapsedTime += Time.deltaTime;
            ratio = elapsedTime / duration;

            Vector3 alpha = startValue + (endValue - startValue) * ratio;

            transform.localScale = alpha;

            yield return null;
        }

        StartCoroutine(AnimateSizeAndRot(endValue, startValue, duration));
    }

    private IEnumerator EnlargeSize(Vector3 startValue, Vector3 endValue, float duration)
    {
        float elapsedTime = 0;
        float ratio = elapsedTime / duration;
        while (ratio < 1f)
        {
            elapsedTime += Time.deltaTime;
            ratio = elapsedTime / duration;

            Vector3 alpha = startValue + (endValue - startValue) * ratio;

            transform.localScale = alpha;

            yield return null;
        }
    }
}
