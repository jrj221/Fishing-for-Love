using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class MainMenuManager : MonoBehaviour
{
    private UIDocument _document;
    private Button _startButton;
    
    private void Awake()
    {
        _document = GetComponent<UIDocument>();
        _startButton = _document.rootVisualElement.Q("StartButton") as Button;
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
        _document.rootVisualElement.style.display = DisplayStyle.None;
        GameManager.Instance.StartGame();
    }
}
