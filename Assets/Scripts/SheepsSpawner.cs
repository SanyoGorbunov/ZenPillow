using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepsSpawner : MonoBehaviour
{
    public GameObject SheepPrefab;
    public GameObject RabbitPawn;
    public GameObject CarrotPrefab;

    //private int selectedColor;

    private Color selected = new Color(248.0f / 255.0f, 243 / 255.0f, 192 / 255.0f);

    private Color[] colors = { new Color(237 / 255.0f, 94 / 255.0f, 121 / 255.0f), new Color(132 / 255.0f, 126 / 255.0f, 160 / 255.0f), new Color(150 / 255.0f, 176 / 255.0f, 186 / 255.0f) };

    private List<SheepController> SheepList = new List<SheepController>();
    // Start is called before the first frame update
    void Start()
    {
        //GenerateDots();
        //GenerateColor();

        //GenerateCarrots(5);
    }

    public void GenerateLevel(int carrotCount)
    {
        //removeAllSheeps();
        bool isHorizontal = Camera.main.aspect >= 1.0f;
        GenerateSheeps(isHorizontal);
        //GenerateColor();

        //GenerateCarrots(carrotCount);
    }

    List<int> GenerateRandomNumberList()
    {
        List<int> list = new List<int>();

        List<int> tempList = new List<int>();

        for (int i = 1; i < 25; i++)
        {
            tempList.Add(i);
        }

        for (int i = 0; i < 24; i++)
        {
            int RandomIndex = Random.Range(0, tempList.Count-1);

            int actualNumber = tempList[RandomIndex];

            tempList.Remove(tempList[RandomIndex]);

            list.Add(actualNumber);
        }

        return list;
    }

    void GenerateSheeps(bool isHorizontal)
    {
        removeAllSheeps();

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

        int number = 0;

        List<int> indices = GenerateRandomNumberList();

        for (int i = 0; i < rowCount; i++)
        {
            if (i % 2 != 0)
            {
                for (int j = 0; j < 4; j++)
                {
                    GameObject dot = Instantiate(SheepPrefab);
                    float randX = Random.Range(0.0f, 0.10f) - 0.05f;
                    float randY = Random.Range(0.0f, 0.10f) - 0.05f;
                    dot.transform.position = new Vector3(startX + j * offset + randX, 0, startY + i * yRowOffset + randY);
                    SheepController controller = dot.GetComponent<SheepController>();
                    float randomSize = Random.Range(0.8f, 1.2f);
                    controller.setSize(randomSize);
                    controller.setNumber(indices[number]);
                    number++;
                    SheepList.Add(controller);

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
                    GameObject dot = Instantiate(SheepPrefab);
                    float randX = Random.Range(0.0f, 0.10f) - 0.05f;
                    float randY = Random.Range(0.0f, 0.10f) - 0.05f;
                    dot.transform.position = new Vector3(startX + j * offset + rowXoffset + randX, 0, startY + i * yRowOffset + randY);
                    SheepController controller = dot.GetComponent<SheepController>();
                    float randomSize = Random.Range(0.8f, 1.2f);
                    controller.setSize(randomSize);
                    controller.setNumber(indices[number]);
                    number++;
                    SheepList.Add(controller);

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

        if (isHorizontal)
        {
            foreach (var dot in SheepList)
            {
                Vector3 temp = dot.gameObject.transform.position;
                temp.x = dot.gameObject.transform.position.z;
                temp.z = dot.gameObject.transform.position.x;
                dot.gameObject.transform.position = temp;
            }

            var tempX = x;
            x = y;
            y = tempX;
        }

        Camera camera = FindObjectOfType<Camera>();

        if (isHorizontal)
        {
            camera.orthographicSize = 3.5f;
        }

        Vector3 oldPos = camera.gameObject.transform.position;

        oldPos.x = x;
        oldPos.z = y;
        camera.gameObject.transform.position = oldPos;
    }

    void removeAllSheeps()
    {
        //SheepController[] array = FindObjectsOfType<SheepController>();

        foreach (SheepController controller in SheepList)
        {
            controller.Destroy();
        }

        SheepList.Clear();
    }

    public SheepController getSheepByNumber(int number)
    {
        foreach (SheepController controller in SheepList)
        {
            if (controller.number == number)
            {
                return controller;
            }
        }
        return null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
