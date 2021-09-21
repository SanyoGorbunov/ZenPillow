using System;

[System.Serializable]
public class PlayerData
{
    public PlayerRecord[] records;
    public string sound;
    public string locale;
    public bool useSoftColors = true;
    public bool showMoreSheep;
}

[System.Serializable]
public class PlayerRecord
{
    public DateTime timestamp;
    public int length;
    public int practice;
    public int rate;
    public int rateBeforeGame;
}
