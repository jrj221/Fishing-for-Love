using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class MainMenuManager : UIManger
{
    private UIDocument _document;
    private Button _startButton;
    
    protected override void Awake()
    {
        base.Awake();
        _startButton = GetElement<Button>("StartButton");
    }

    private void OnEnable()
    {
        _startButton.RegisterCallback<ClickEvent>(StartGame);
    }

    private void OnDisable()
    {
        _startButton.UnregisterCallback<ClickEvent>(StartGame);
    }

    private void StartGame(ClickEvent e)
    {
        HideUI();
        GameManager.Instance.StartGame();
    }
}
