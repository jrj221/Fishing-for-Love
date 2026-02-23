using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int _goodEndingMinHearts;
    [SerializeField] private int _midEndingMinHearts;
    public bool GameIsOver { get; private set; }
    public static GameManager Instance { get; private set; }
    
    private void Start()
    {
        Instance = this;
        Time.timeScale = 0;
    }

    public void StartGame()
    {
        MainMenuManager.Instance.HideUI();
        Time.timeScale = 1;
        GameUIManager.Instance.ShowUI();
    }

    public void EndGame(int heartCount)
    {
        Time.timeScale = 0;
        GameIsOver = true;
        // Delayed so you see the Time's Up message for a second
        Helpers.Instance.Delay(3f, () =>
        {
            EndingUIManager.Ending ending = DetermineEnding(heartCount);
            GameUIManager.Instance.HideUI();
            EndingUIManager.Instance.ShowEnding(ending, heartCount);
        });
        
    }

    private EndingUIManager.Ending DetermineEnding(int heartCount)
    {
        if (heartCount >= _goodEndingMinHearts)
        {
            return EndingUIManager.Ending.Good;
        } else if (heartCount >= _midEndingMinHearts)
        {
            return EndingUIManager.Ending.Mid;
        }
        else
        {
            return EndingUIManager.Ending.Bad;
        }
    }
}
