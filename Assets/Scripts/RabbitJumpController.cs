using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RabbitJumpController : MonoBehaviour
{
    private bool _isOver;

    public DotsSpawner dotSpawner;
    public RabbitController rabbitController;

    private int CarrotCountLeft = 0;

    // Start is called before the first frame update
    void Start()
    {
        CarrotCountLeft = 5;

        dotSpawner.GenerateLevel(CarrotCountLeft);

        StartCoroutine(Finish());
    }

    public void PickUpCarrot()
    {
        CarrotCountLeft--;
        if (CarrotCountLeft == 0)
        {
            CarrotCountLeft = 5;
            dotSpawner.GenerateLevel(CarrotCountLeft);
        }
    }



    IEnumerator Finish()
    {
        var gameLength = GameStateManager.Instance.GetTimeLengthInMins();
        yield return new WaitForSeconds(60* gameLength);
        _isOver = true;
        GameOver();
    }

    void GameOver()
    {
        SceneManager.LoadScene("RateScene");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
