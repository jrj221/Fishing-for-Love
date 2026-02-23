using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class MainMenuManager : UIManger
{
    private Label _title;
    private Button _startButton;
    private Button _tutorialButton;
    private Label _tutorialTitle;
    private Label _tutorialText;
    private EventCallback<ClickEvent> _startGameCallback;
    public static MainMenuManager Instance { get; private set; }
    
    protected override void Awake()
    {
        base.Awake();
        Instance = this;
        _startGameCallback = (evt) => GameManager.Instance.StartGame();
        _title = GetElement<Label>("Title");
        _tutorialTitle = GetElement<Label>("TutorialTitle");
        _tutorialText = GetElement<Label>("TutorialText");
        _startButton = GetElement<Button>("StartButton");
        _tutorialButton = GetElement<Button>("TutorialButton");
    }
    
    private void Start()
    {
        HideElement(_tutorialText);
        HideElement(_tutorialTitle);
        _tutorialText.text = "You are a mere fisherman, trying to woo the girl of your dreams by <i>Fishing for Love</i> in the <b>Sea of Hearts</b>. " +
                             "Get enough, and you may be successful in winning her over. " +
                             "Hold <b>SPACE</b> to reel in your hook, otherwise it will fall on its own. " +
                             "Keep your hook on a heart for long enough, and you'll capture it. " +
                             "Capture as many as possible before time runs out!";
    }
    
    private void OnEnable()
    {
        _startButton.RegisterCallback(_startGameCallback);
        _tutorialButton.RegisterCallback<ClickEvent>(Tutorial);
    }

    private void OnDisable()
    {
        _startButton.UnregisterCallback(_startGameCallback);
        _tutorialButton.UnregisterCallback<ClickEvent>(Tutorial);
    }

    private void Tutorial(ClickEvent evt)
    {
        HideElement(_title);
        HideElement(_tutorialButton);
        ShowElement(_tutorialTitle);
        ShowElement(_tutorialText);
    }
}
