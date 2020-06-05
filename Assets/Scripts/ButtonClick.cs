using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ButtonClick : MonoBehaviour
{
    private string playerName = "Player";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SavePlayerData(string text)
    {
        PlayerData data = new PlayerData();
        data.stressLevel = 24;
        data.lastMessage = text;

        SaveSystem.Save(data, playerName);
    }

    public void LoadPlayerData()
    {
        PlayerData data = SaveSystem.Load(playerName);

        //Debug.Log("Stress level = "+data.stressLevel);
        //Debug.Log("Last message = " + data.lastMessage);

        Text log = GameObject.Find("Log_Text").GetComponent<Text>();

        log.text += "Stress level = " + data.stressLevel;
        log.text += "Last message = " + data.lastMessage;

    }

    public void RemovePlayerData()
    {
        SaveSystem.Remove(playerName);
    }
}
