using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PlayerData
{
    public int stressLevel;
    public  int zenLevel;
    public int epicLevel;
    public string lastMessage;

    public PlayerData()
    {
        stressLevel = 0;
        zenLevel = 100;
        epicLevel = 69;
        lastMessage = "I am epic!";
    }
}
