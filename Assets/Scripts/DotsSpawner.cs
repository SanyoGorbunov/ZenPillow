using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotsSpawner : MonoBehaviour
{
    public GameObject DotPrefab;
    public GameObject RabbitPawn;
    public GameObject CarrotPrefab;

    private Color[] colors = { new Color(248.0f/ 255.0f, 243 / 255.0f, 192 / 255.0f), new Color(237 / 255.0f, 94 / 255.0f, 121 / 255.0f), new Color(132 / 255.0f, 126 / 255.0f, 160 / 255.0f), new Color(150 / 255.0f, 176 / 255.0f, 186 / 255.0f) };

    private List<ColorDotController> DotList;
    // Start is called before the first frame update
    void Start()
    {
        DotList = new List<ColorDotController>();
        GenerateDots();
        GenerateColor();

        GenerateCarrots(5);
    }

    void GenerateCarrots(int count)
    {
        float offset = 0.1f;

        List<int> temp = new List<int>();

        for (int i = 0; i < DotList.Count; i++)
        {
            temp.Add(i);
        }

        for (int i = 0; i < count; i++)
        {
            int id = (int) Random.Range(0, temp.Count-1);

            temp.Remove(temp[id]);

            GameObject obj = Instantiate(CarrotPrefab);
            Vector3 tempPos = DotList[id].transform.position;
            tempPos.y += offset;
            obj.transform.position = tempPos;
        }
    }

    void PlaceRabbit(Vector3 pos)
    {
        RabbitPawn.transform.position = pos;
    }

    void GenerateColor()
    {
        foreach(ColorDotController controller in DotList)
        {
            var renderer = controller.gameObject.GetComponent<Renderer>();
            //Random.Range(0, 3);
            int id = (int)Random.Range(0, 3);
            renderer.material.SetColor("_Color", colors[id]);
        }
    }

    void GenerateDots()
    {
        int rowCount = 7;
        float offset = 1.3f;

        float yRowOffset = offset * Mathf.Sqrt(3) / 2;

        float startX = 0.5f-(offset*4)/2;
        float startY = -3.7f;
        float rowXoffset = 0.75f;

        float minX = float.MaxValue;
        float minY = float.MaxValue;

        float maxX = float.MinValue;
        float maxY = float.MinValue;

        for (int i = 0; i < rowCount; i++)
        {
            if (i % 2 != 0)
            {
                for (int j = 0; j < 4; j++)
                {
                    GameObject dot = Instantiate(DotPrefab);
                    float randX = Random.Range(0.0f, 0.10f) - 0.05f;
                    float randY = Random.Range(0.0f, 0.10f) - 0.05f;
                    dot.transform.position = new Vector3(startX + j * offset + randX, 0, startY + i * yRowOffset + randY);
                    ColorDotController controller = dot.GetComponent<ColorDotController>();
                    float randomSize = Random.Range(0.8f, 1.2f);
                    controller.setSize(randomSize);
                    DotList.Add(controller);

                    if (dot.transform.position.x - dot.transform.localScale.x < minX)
                    {
                        minX = dot.transform.position.x - dot.transform.localScale.x;
                    }

                    if (dot.transform.position.x + dot.transform.localScale.x > maxX)
                    {
                        maxX = dot.transform.position.x + dot.transform.localScale.x;
                    }

                    if (dot.transform.position.z - dot.transform.localScale.z < minY)
                    {
                        minY = dot.transform.position.z - dot.transform.localScale.z;
                    }

                    if (dot.transform.position.z + dot.transform.localScale.z > maxY)
                    {
                        maxY = dot.transform.position.z + dot.transform.localScale.z;
                    }
                }
            }
            else
            {
                for (int j = 0; j < 3; j++)
                {
                    GameObject dot = Instantiate(DotPrefab);
                    float randX = Random.Range(0.0f, 0.10f) - 0.05f;
                    float randY = Random.Range(0.0f, 0.10f) - 0.05f;
                    dot.transform.position = new Vector3(startX + j * offset + rowXoffset + randX, 0, startY + i * yRowOffset + randY);
                    ColorDotController controller = dot.GetComponent<ColorDotController>();
                    float randomSize = Random.Range(0.8f, 1.2f);
                    controller.setSize(randomSize);
                    DotList.Add(controller);

                    if (dot.transform.position.x - dot.transform.localScale.x < minX)
                    {
                        minX = dot.transform.position.x - dot.transform.localScale.x;
                    }

                    if (dot.transform.position.x + dot.transform.localScale.x > maxX)
                    {
                        maxX = dot.transform.position.x + dot.transform.localScale.x;
                    }

                    if (dot.transform.position.z - dot.transform.localScale.z < minY)
                    {
                        minY = dot.transform.position.z - dot.transform.localScale.z;
                    }

                    if (dot.transform.position.z + dot.transform.localScale.z > maxY)
                    {
                        maxY = dot.transform.position.z + dot.transform.localScale.z;
                    }
                }
            }
        }

        float x = (minX + maxX) / 2;
        float y = (minY + maxY) / 2;

        Vector3 oldPos = FindObjectOfType<Camera>().gameObject.transform.position;

        oldPos.x = x;
        oldPos.z = y;
        FindObjectOfType<Camera>().gameObject.transform.position = oldPos;

        RabbitPawn.transform.position = DotList[DotList.Count - 1].transform.position;

        oldPos.y = 0.0f;

        RabbitPawn.transform.forward = oldPos - RabbitPawn.transform.position;
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
