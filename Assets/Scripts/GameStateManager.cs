using System;

public class GameStateManager
{
    private static readonly GameStateManager _instance = new GameStateManager();
    public static GameStateManager Instance => _instance;

    private int _gameMinutes;
    private Practice _gamePractice;

    public void SelectPractice(Practice practice)
    {
        _gamePractice = practice;
    }

    public Practice GetPractice()
    {
        return _gamePractice;
    }

    public void SelectTime(int minutes)
    {
        _gameMinutes = minutes;
    }

    public int GetTimeLengthInMins()
    {
        return _gameMinutes;
    }

    public void SaveGame(int rate)
    {
        SaveSystem.Save(new PlayerRecord
        {
            practice = (int)_gamePractice,
            length = _gameMinutes,
            rate = rate,
            timestamp = DateTime.UtcNow
        });
    }

    public Practice GetFavoritePractice()
    {
        var data = SaveSystem.Load();

        int[] counts = new int[3];
        int[] sums = new int[3];
        foreach (var record in data.records)
        {
            counts[record.practice]++;
            sums[record.practice] += record.rate;
        }
        for (int i = 0; i < 3; i++)
        {
            if (counts[i] != 0)
            {
                sums[i] = sums[i] / counts[i];
            }
        }
        int argMax = 0;
        for (int i = 1; i < 3; i++)
        {
            if (sums[i] > sums[argMax])
            {
                argMax = i;
            }
        }
        return (Practice)argMax;
    }
}
