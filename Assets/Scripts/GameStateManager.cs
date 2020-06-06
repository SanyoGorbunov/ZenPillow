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
        return Practice.Breathing;
    }

    public void SelectTime(int minutes)
    {
        _gameMinutes = minutes;
    }

    public void StartGame()
    {
    }
}
