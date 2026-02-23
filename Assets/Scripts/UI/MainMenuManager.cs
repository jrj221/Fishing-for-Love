using System;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class MainMenuManager : UIManger
{
    private Label _title;
    private Button _startButton;
    private Button _easyDifficultyButton;
    private Button _mediumDifficultyButton;
    private Button _hardDifficultyButton;
    private Label _difficultySelectTitle;
    private Button _tutorialButton;
    private Label _tutorialTitle;
    private Label _tutorialText;
    public static MainMenuManager Instance { get; private set; }
    
    protected override void Awake()
    {
        base.Awake();
        Instance = this;
        _title = GetElement<Label>("Title");
        _tutorialTitle = GetElement<Label>("TutorialTitle");
        _tutorialText = GetElement<Label>("TutorialText");
        _startButton = GetElement<Button>("StartButton");
        _easyDifficultyButton = GetElement<Button>("EasyDifficultyButton");
        _mediumDifficultyButton = GetElement<Button>("MediumDifficultyButton");
        _hardDifficultyButton = GetElement<Button>("HardDifficultyButton");
        _difficultySelectTitle = GetElement<Label>("DifficultySelectTitle");
        _tutorialButton = GetElement<Button>("TutorialButton");
    }
    
    protected override void Start()
    {
        base.Start();
        HideElement(_tutorialText);
        HideElement(_tutorialTitle);
        HideElement(_easyDifficultyButton);
        HideElement(_mediumDifficultyButton);
        HideElement(_hardDifficultyButton);
        HideElement(_difficultySelectTitle);
        _tutorialText.text = "You are a mere fisherman, trying to woo the girl of your dreams by <i>Fishing for Love</i> in the <b>Sea of Hearts</b>. " +
                             "Get enough, and you may be successful in winning her over. " +
                             "Hold <b>SPACE</b> to reel in your hook, otherwise it will fall on its own. " +
                             "Keep your hook on a heart for long enough, and you'll capture it. " +
                             "Capture as many as possible before time runs out!";
    }
    
    private void OnEnable()
    {
        _startButton.RegisterCallback<ClickEvent>(DifficultySelect);
        _tutorialButton.RegisterCallback<ClickEvent>(Tutorial);
        _easyDifficultyButton.RegisterCallback<ClickEvent>(Easy);
        _mediumDifficultyButton.RegisterCallback<ClickEvent>(Medium);
        _hardDifficultyButton.RegisterCallback<ClickEvent>(Hard);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _startButton.UnregisterCallback<ClickEvent>(DifficultySelect);
        _tutorialButton.UnregisterCallback<ClickEvent>(Tutorial);
        _easyDifficultyButton.UnregisterCallback<ClickEvent>(Easy);
        _mediumDifficultyButton.UnregisterCallback<ClickEvent>(Medium);
        _hardDifficultyButton.UnregisterCallback<ClickEvent>(Hard);
    }

    private void StartGame(float gameLength, float heartSpeed, float heartDelaySpeed)
    {
        HideElement(_easyDifficultyButton);
        HideElement(_mediumDifficultyButton);
        HideElement(_hardDifficultyButton);
        HideElement(_difficultySelectTitle);
        GameManager.Instance.StartGame(gameLength,  heartSpeed, heartDelaySpeed);
    }

    private void DifficultySelect(ClickEvent evt)
    {
        HideElement(_title);
        HideElement(_tutorialButton);
        HideElement(_tutorialTitle);
        HideElement(_tutorialText);
        HideElement(_startButton);
        ShowElement(_easyDifficultyButton);
        ShowElement(_mediumDifficultyButton);
        ShowElement(_hardDifficultyButton);
        ShowElement(_difficultySelectTitle);
    }

    private void Easy(ClickEvent evt)
    {
        StartGame(90f, 2f, 1f);
    }
    
    private void Medium(ClickEvent evt)
    {
        StartGame(60f, 4f, .7f);
    }
    
    private void Hard(ClickEvent evt)
    {
        StartGame(45f, 6f, .2f);
    }

    private void Tutorial(ClickEvent evt)
    {
        HideElement(_title);
        HideElement(_tutorialButton);
        ShowElement(_tutorialTitle);
        ShowElement(_tutorialText);
    }
}
