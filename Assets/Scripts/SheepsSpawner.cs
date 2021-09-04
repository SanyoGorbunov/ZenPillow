using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepsSpawner : MonoBehaviour
{
    public GameObject SheepPrefab;

    //private int selectedColor;

    private Color selected = new Color(248.0f / 255.0f, 243 / 255.0f, 192 / 255.0f);

    private Color[] colors = { new Color(237 / 255.0f, 94 / 255.0f, 121 / 255.0f), new Color(132 / 255.0f, 126 / 255.0f, 160 / 255.0f), new Color(150 / 255.0f, 176 / 255.0f, 186 / 255.0f) };

    private List<SheepController> SheepList = new List<SheepController>();

    private float centerX = 0.0f;
    private float centerY = 0.0f;
    // Start is called before the first frame update
    void Start() { }

    public void GenerateLevel(bool isSimplified)
    {
        bool isHorizontal = Camera.main.aspect >= 1.0f;
        GenerateSheeps(isSimplified, isHorizontal);
    }

    List<int> GenerateRandomNumberList(int sheepCount)
    {
        List<int> list = new List<int>();

        List<int> tempList = new List<int>();

        for (int i = 1; i < sheepCount + 1; i++)
        {
            tempList.Add(i);
        }

        for (int i = 0; i < sheepCount; i++)
        {
            int RandomIndex = Random.Range(0, tempList.Count-1);

            int actualNumber = tempList[RandomIndex];

            tempList.Remove(tempList[RandomIndex]);

            list.Add(actualNumber);
        }

        return list;
    }

    void GenerateSheeps(bool isSimplified, bool isHorizontal)
    {
        removeAllSheeps();

        int sheepCount = isSimplified ? 7 : 24;
        int rowCount = isSimplified ? 3 : 7;
        int colEvenCount = isSimplified ? 2 : 3;
        int colOddCount = isSimplified ? 3 : 4;
        bool useRandomSize = !isSimplified;

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

        List<int> indices = GenerateRandomNumberList(sheepCount);

        for (int i = 0; i < rowCount; i++)
        {
            if (i % 2 != 0)
            {
                for (int j = 0; j < colOddCount; j++)
                {
                    GameObject dot = Instantiate(SheepPrefab);
                    float randX = Random.Range(0.0f, 0.10f) - 0.05f;
                    float randY = Random.Range(0.0f, 0.10f) - 0.05f;
                    dot.transform.position = new Vector3(startX + j * offset + randX, 0, startY + i * yRowOffset + randY);
                    SheepController controller = dot.GetComponent<SheepController>();
                    if (useRandomSize)
                    {
                        float randomSize = Random.Range(0.8f, 1.2f);
                        controller.setSize(randomSize);
                    }

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
                for (int j = 0; j < colEvenCount; j++)
                {
                    GameObject dot = Instantiate(SheepPrefab);
                    float randX = Random.Range(0.0f, 0.10f) - 0.05f;
                    float randY = Random.Range(0.0f, 0.10f) - 0.05f;
                    dot.transform.position = new Vector3(startX + j * offset + rowXoffset + randX, 0, startY + i * yRowOffset + randY);
                    SheepController controller = dot.GetComponent<SheepController>();
                    if (useRandomSize)
                    {
                        float randomSize = Random.Range(0.8f, 1.2f);
                        controller.setSize(randomSize);
                    }

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
        camera.orthographicSize = GetOrthographicSize(isSimplified, isHorizontal);

        Vector3 oldPos = camera.gameObject.transform.position;

        oldPos.x = x;
        oldPos.z = y;
        camera.gameObject.transform.position = oldPos;

        centerX = x;
        centerY = y;
    }

    public void Rotate(bool isSimplified, bool isHorizontal)
    {
        float x = centerX;
        float y = centerY;

        foreach (var dot in SheepList)
        {
            if(dot!=null)
            { 
                Vector3 temp = dot.gameObject.transform.position;
                temp.x = dot.gameObject.transform.position.z;
                temp.z = dot.gameObject.transform.position.x;
                dot.gameObject.transform.position = temp;
            }
        }

        var tempX = x;
        x = y;
        y = tempX;

        Camera camera = FindObjectOfType<Camera>();
        camera.orthographicSize = GetOrthographicSize(isSimplified, isHorizontal);

        Vector3 oldPos = camera.gameObject.transform.position;

        oldPos.x = x;
        oldPos.z = y;
        camera.gameObject.transform.position = oldPos;

        centerX = x;
        centerY = y;
    }

    float GetOrthographicSize(bool isSimplified, bool isHorizontal)
    {
        if (isSimplified)
        {
            if (isHorizontal)
            {
                return 2.5f;
            }

            return 3.5f;
        }

        if (isHorizontal)
        {
            return 3.5f;
        }

        return 5.0f;
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
