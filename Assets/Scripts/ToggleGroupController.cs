using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleGroupController : MonoBehaviour
{
    Dictionary<int, bool> answers = new Dictionary<int, bool>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSelected(int id, bool isSelected)
    {
        answers.Add(id, isSelected);
    }

    public void checkResult()
    {
    }
}
