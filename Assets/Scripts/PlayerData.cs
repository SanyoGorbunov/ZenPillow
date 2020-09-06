using System;

[System.Serializable]
public class PlayerData
{
    public PlayerRecord[] records;
    public string sound;
}

[System.Serializable]
public class PlayerRecord
{
    public DateTime timestamp;
    public int length;
    public int practice;
    public int rate;
}
