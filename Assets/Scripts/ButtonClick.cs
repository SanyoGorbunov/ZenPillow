using System;
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
        PlayerRecord record = new PlayerRecord();
        record.practice = 0;
        record.timestamp = DateTime.UtcNow;

        SaveSystem.Save(record);
    }

    public void LoadPlayerData()
    {
        PlayerData data = SaveSystem.Load();

        //Debug.Log("Stress level = "+data.stressLevel);
        //Debug.Log("Last message = " + data.lastMessage);

        Text log = GameObject.Find("Log_Text").GetComponent<Text>();

        log.text += "Stress level = " + data.records[0].practice;
        log.text += "Last message = " + data.records[0].timestamp;

    }

    public void RemovePlayerData()
    {
        SaveSystem.Remove();
    }
}
