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

    public void Destroy()
    {
        //transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
