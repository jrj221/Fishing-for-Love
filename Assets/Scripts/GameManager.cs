using UnityEngine;

public class GameManager : MonoBehaviour
{
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

    public void EndGame()
    {
        GameUIManager.Instance.HideUI();
        
    }
}
