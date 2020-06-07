using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorDotController : MonoBehaviour
{

    public bool hasCarrot = false;
    private GameObject carrot = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        carrot = transform.Find("Carrot").gameObject;
    }

    public void showCarrot()
    {
        carrot.SetActive(true);
        hasCarrot = true;
    }

    public void hideCarrot()
    {
        carrot.SetActive(false);
        hasCarrot = false;
    }

    public void setSize(float scale)
    {
        this.transform.localScale = new Vector3(scale, 0.1f, scale);

        carrot.transform.localScale = new Vector3(0.2f/ scale, 0.2f / scale, 0.2f / scale);
    }
    public void OnClick()
    {
        RabbitController rabbit = FindObjectOfType<RabbitController>();
        rabbit.JumpTo(this);
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
