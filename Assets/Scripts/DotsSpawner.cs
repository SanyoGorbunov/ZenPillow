using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotsSpawner : MonoBehaviour
{
    public GameObject DotPrefab;
    // Start is called before the first frame update
    void Start()
    {
        GenerateDots();
        
    }

    void GenerateDots()
    {
        int rowCount = 7;
        float offset = 1.5f;

        float yRowOffset = offset * Mathf.Sqrt(3) / 2;

        float startX = -2.5f;
        float startY = -3.7f;
        float rowXoffset = 0.75f;

        for (int i = 0; i < rowCount; i++)
        {
            if (i % 2 == 0)
            {
                for (int j = 0; j < 4; j++)
                {
                    GameObject dot = Instantiate(DotPrefab);
                    float randX = Random.Range(0.0f, 0.2f) - 0.1f;
                    float randY = Random.Range(0.0f, 0.2f) - 0.1f;
                    dot.transform.position = new Vector3(startX + j * offset + randX, 0, startY + i * yRowOffset + randY);
                    ColorDotController controller = dot.GetComponent<ColorDotController>();
                    float randomSize = Random.Range(0.8f, 1.2f);
                    controller.setSize(randomSize);
                }
            }
            else
            {
                for (int j = 0; j < 3; j++)
                {
                    GameObject dot = Instantiate(DotPrefab);
                    float randX = Random.Range(0.0f, 0.2f) - 0.1f;
                    float randY = Random.Range(0.0f, 0.2f) - 0.1f;
                    dot.transform.position = new Vector3(startX + j * offset + rowXoffset + randX, 0, startY + i * yRowOffset + randY);
                    ColorDotController controller = dot.GetComponent<ColorDotController>();
                    float randomSize = Random.Range(0.8f, 1.2f);
                    controller.setSize(randomSize);
                }
            }
        }
    }

    void removeAllDots()
    {
        ColorDotController[] array = FindObjectsOfType<ColorDotController>();

        foreach (ColorDotController controller in array)
        {
            controller.Destroy();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
