using System;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class MainMenuManager : UIManger
{
    #region Difficulty References
    [Header("Easy Difficulty")]
    [SerializeField] private float _easyGameLength;
    [SerializeField] private float _easyHeartSpeed;
    [SerializeField] private float _easyHeartDelaySpeed;
    
    [Header("Medium Difficulty")]
    [SerializeField] private float _mediumGameLength;
    [SerializeField] private float _mediumHeartSpeed;
    [SerializeField] private float _mediumHeartDelaySpeed;
    
    [Header("Hard Difficulty")]
    [SerializeField] private float _hardGameLength;
    [SerializeField] private float _hardHeartSpeed;
    [SerializeField] private float _hardHeartDelaySpeed;
    #endregion
    
    #region UI Elements
    private Label _title;
    private Button _startButton;
    private Button _easyDifficultyButton;
    private Button _mediumDifficultyButton;
    private Button _hardDifficultyButton;
    private Label _difficultySelectTitle;
    private Button _tutorialButton;
    private Label _tutorialTitle;
    private Label _tutorialText;
    #endregion

    private EventCallback<ClickEvent> _easyDifficulty;
    private EventCallback<ClickEvent> _mediumDifficulty;
    private EventCallback<ClickEvent> _hardDifficulty;
    
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
        
        _easyDifficulty = (evt) => StartGame(_easyGameLength, _easyHeartSpeed, _easyHeartDelaySpeed);
        _mediumDifficulty = (evt) => StartGame(_mediumGameLength, _mediumHeartSpeed, _mediumHeartDelaySpeed);
        _hardDifficulty = (evt) => StartGame(_hardGameLength, _hardHeartSpeed, _hardHeartDelaySpeed);
    }
    
    protected override void Start()
    {
        base.Start();
        SetMainMenuState();
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
        _easyDifficultyButton.RegisterCallback(_easyDifficulty);
        _mediumDifficultyButton.RegisterCallback(_mediumDifficulty);
        _hardDifficultyButton.RegisterCallback(_hardDifficulty);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _startButton.UnregisterCallback<ClickEvent>(DifficultySelect);
        _tutorialButton.UnregisterCallback<ClickEvent>(Tutorial);
        _easyDifficultyButton.UnregisterCallback(_easyDifficulty);
        _mediumDifficultyButton.UnregisterCallback(_mediumDifficulty);
        _hardDifficultyButton.UnregisterCallback(_hardDifficulty);
    }

    private void StartGame(float gameLength, float heartSpeed, float heartDelaySpeed)
    {
        // SetMainMenuState(); // should this be done here?
        HideUI();
        GameManager.Instance.StartGame(gameLength,  heartSpeed, heartDelaySpeed);
    }

    private void DifficultySelect(ClickEvent evt)
    {
        SetDifficultySelectState();
    }

    private void Tutorial(ClickEvent evt)
    {
        SetTutorialState();
    }

    public void SetMainMenuState()
    {
        foreach (VisualElement element in new VisualElement[] { _title, _startButton, _tutorialButton }) 
        {
            ShowElement(element);
        }
        foreach (VisualElement element in new VisualElement[] { _tutorialTitle, _tutorialText, _easyDifficultyButton, _mediumDifficultyButton, _hardDifficultyButton, _difficultySelectTitle }) 
        {
            HideElement(element);
        }
    }

    private void SetTutorialState()
    {
        foreach (VisualElement element in new VisualElement[] { _tutorialTitle, _tutorialText, _startButton }) 
        {
            ShowElement(element);
        }
        foreach (VisualElement element in new VisualElement[] { _title, _tutorialButton, _easyDifficultyButton, _mediumDifficultyButton, _hardDifficultyButton, _difficultySelectTitle }) 
        {
            HideElement(element);
        }
    }

    private void SetDifficultySelectState()
    {
        foreach (VisualElement element in new VisualElement[] { _easyDifficultyButton, _mediumDifficultyButton, _hardDifficultyButton, _difficultySelectTitle }) 
        {
            ShowElement(element);
        }
        foreach (VisualElement element in new VisualElement[] { _title, _tutorialButton, _startButton, _tutorialText, _tutorialTitle }) 
        {
            HideElement(element);
        }
    }
}
