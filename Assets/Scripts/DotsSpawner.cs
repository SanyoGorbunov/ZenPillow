using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotsSpawner : MonoBehaviour
{
    public GameObject DotPrefab;
    public GameObject RabbitPawn;
    public GameObject CarrotPrefab;

    //private int selectedColor;

    private Color selected = new Color(248.0f / 255.0f, 243 / 255.0f, 192 / 255.0f);

    private Color[] colors = { new Color(237 / 255.0f, 94 / 255.0f, 121 / 255.0f), new Color(132 / 255.0f, 126 / 255.0f, 160 / 255.0f), new Color(150 / 255.0f, 176 / 255.0f, 186 / 255.0f) };

    private List<ColorDotController> DotList = new List<ColorDotController>();
    // Start is called before the first frame update
    void Start()
    {
        //GenerateDots();
        //GenerateColor();

        //GenerateCarrots(5);
    }

    public void GenerateLevel(int carrotCount)
    {
        removeAllDots();

        bool isHorizontal = Camera.main.aspect >= 1.0f;

        GenerateDots(isHorizontal);
        GenerateColor();

        GenerateCarrots(carrotCount);
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
            int id = Random.Range(0, temp.Count);

            int number = temp[id];

            temp.Remove(temp[id]);

            //GameObject obj = Instantiate(CarrotPrefab);
            DotList[number].showCarrot();


            var renderer = DotList[number].gameObject.GetComponent<Renderer>();
            //Random.Range(0, 3);
            renderer.material.SetColor("_Color", selected);

            //tempPos.y += offset;
            //obj.transform.position = tempPos;
        }

        int idLast = Random.Range(0, temp.Count);

        int numberLast = temp[idLast];

        var rendererLast = DotList[numberLast].gameObject.GetComponent<Renderer>();
        //Random.Range(0, 3);
        rendererLast.material.SetColor("_Color", selected);

        PlaceRabbit(DotList[numberLast].gameObject.transform.position);
    }

    void PlaceRabbit(Vector3 pos)
    {
        RabbitPawn.transform.position = pos;
    }

    void GenerateColor()
    {
        //List<Color> availableColors = new List<Color>(colors);
        
        //int selectedColor = Random.Range(0, colors.Length);

        //availableColors.Remove(colors[selectedColor]);

        foreach(ColorDotController controller in DotList)
        {
            var renderer = controller.gameObject.GetComponent<Renderer>();
            //Random.Range(0, 3);
            int id = Random.Range(0, colors.Length);

            Color temp = colors[id];

            renderer.material.SetColor("_Color", temp);
        }
    }

    private float centerX = 0.0f;
    private float centerY = 0.0f;

    void GenerateDots(bool isHorizontal)
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
                    //controller.setSize(randomSize);
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

        if (isHorizontal)
        {
            foreach (var dot in DotList)
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
        else
        {
            camera.orthographicSize = 5.0f;
        }

        Vector3 oldPos = camera.gameObject.transform.position;

        oldPos.x = x;
        oldPos.z = y;
        camera.gameObject.transform.position = oldPos;

        RabbitPawn.transform.position = DotList[DotList.Count - 1].transform.position;

        

        oldPos.y = 0.0f;

        RabbitPawn.transform.forward = oldPos - RabbitPawn.transform.position;

        centerX = x;
        centerY = y;
    }

    private Vector3 RotateVector(Vector3 vector)
    {
        Vector3 newVector = vector;
        float temp = newVector.x;
        newVector.x = newVector.z;
        newVector.z = temp;
        return newVector;
    }

    public void Rotate(bool isHorizontal)
    {
        float x = centerX;
        float y = centerY;

        foreach (var dot in DotList)
        {
            Vector3 temp = dot.gameObject.transform.position;
            temp.x = dot.gameObject.transform.position.z;
            temp.z = dot.gameObject.transform.position.x;
            dot.gameObject.transform.position = temp;
        }

        var tempX = x;
        x = y;
        y = tempX;

        Camera camera = FindObjectOfType<Camera>();

        if (isHorizontal)
        {
            camera.orthographicSize = 3.5f;
        }
        else
        {
            camera.orthographicSize = 5.0f;
        }

        Vector3 oldPos = camera.gameObject.transform.position;

        oldPos.x = x;
        oldPos.z = y;
        camera.gameObject.transform.position = oldPos;

        RabbitPawn.transform.position = RotateVector(RabbitPawn.transform.position);
        RabbitPawn.transform.forward = RotateVector(RabbitPawn.transform.forward);

        centerX = x;
        centerY = y;
    }

    void removeAllDots()
    {
        foreach (ColorDotController controller in DotList)
        {
            controller.Destroy();
        }

        DotList.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
