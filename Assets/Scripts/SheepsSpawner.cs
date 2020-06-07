﻿using System.Collections;
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
        GenerateSheeps();
        //GenerateColor();

        //GenerateCarrots(carrotCount);
    }

    void GenerateRandomNumberList()
    {

    }

    void GenerateSheeps()
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

        int number = 1;

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
                    controller.setNumber(number);
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
                    controller.setNumber(number);
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

        Vector3 oldPos = FindObjectOfType<Camera>().gameObject.transform.position;

        oldPos.x = x;
        oldPos.z = y;
        FindObjectOfType<Camera>().gameObject.transform.position = oldPos;

        /*RabbitPawn.transform.position = SheepList[SheepList.Count - 1].transform.position;

        oldPos.y = 0.0f;

        RabbitPawn.transform.forward = oldPos - RabbitPawn.transform.position;*/
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

    // Update is called once per frame
    void Update()
    {
        
    }
}