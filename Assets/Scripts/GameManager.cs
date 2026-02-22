using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float _goodEndingMinHearts;
    [SerializeField] private float _midEndingMinHearts;
    public static GameManager Instance { get; private set; }
    private void Start()
    {
        Instance = this;
        Time.timeScale = 0;
    }

    public void StartGame()
    {
        Time.timeScale = 1;
        GameUIManager.Instance.ShowUI();
    }

    public void EndGame(float heartCount)
    {
        Time.timeScale = 0;
        GameUIManager.Instance.HideUI();
        if (heartCount >= _goodEndingMinHearts)
        {
            EndingUIManager.Instance.ShowEnding(EndingUIManager.Ending.Good);
        } else if (heartCount >= _midEndingMinHearts)
        {
            EndingUIManager.Instance.ShowEnding(EndingUIManager.Ending.Mid);
        }
        else
        {
            EndingUIManager.Instance.ShowEnding(EndingUIManager.Ending.Bad);
        }
    }
}
