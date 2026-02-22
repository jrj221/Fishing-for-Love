using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class MainMenuManager : UIManger
{
    private Button _startButton;
    private EventCallback<ClickEvent> _startGameCallback;
    public static MainMenuManager Instance { get; private set; }
    
    protected override void Awake()
    {
        base.Awake();
        Instance = this;
        _startGameCallback = (evt) => GameManager.Instance.StartGame();
        _startButton = GetElement<Button>("StartButton");
    }

    private void OnEnable()
    {
        _startButton.RegisterCallback(_startGameCallback);
    }

    private void OnDisable()
    {
        _startButton.UnregisterCallback(_startGameCallback);
    }
}
