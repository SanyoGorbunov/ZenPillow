using System.Collections;
using UnityEngine;

public class BreathingController : MonoBehaviour
{
    public BreathingCircleController breathingCircleController;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(nameof(Play));
    }

    IEnumerator Play()
    {
        yield return new WaitForSeconds(1.0f);
        Inhale();
    }

    void Inhale()
    {
        breathingCircleController.Scale(true, isUpscaling => Exhale());
    }

    void Exhale()
    {
        breathingCircleController.Scale(false, isUpscaling => Inhale());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
