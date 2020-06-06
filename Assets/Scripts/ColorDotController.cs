using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorDotController : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void setSize(float scale)
    {
        this.transform.localScale = new Vector3(scale, 0.1f, scale);
    }
    public void OnClick()
    {
        Debug.Log("touch");
        RabbitController rabbit = FindObjectOfType<RabbitController>();
        rabbit.JumpTo(this);
    }

    public void Destroy()
    {
        //transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        //Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
