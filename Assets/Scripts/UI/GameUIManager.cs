using System.Globalization;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class GameUIManager : UIManger
{
    public float GameLength { private get; set; }
    [SerializeField] private SpriteRenderer _hook;
    [SerializeField] private SpriteRenderer _heart;
    [SerializeField] private SpriteRenderer _progressBar;
    private Label _timer;
    private Label _heartCounter;
    private Label _endNotice;
    private Label _gameStartCountdown;
    private float _timeLeft;
    private int _heartCount;
    public static GameUIManager Instance { get; private set; }
    
    protected override void Awake()
    {
        base.Awake();
        Instance = this;
        _timer = GetElement<Label>("Timer");
        _heartCounter = GetElement<Label>("HeartCounter");
        _endNotice = GetElement<Label>("EndNotice");
        _gameStartCountdown = GetElement<Label>("GameStartCountdown");
        ResetTimeLeft();
    }

    protected override void Start()
    {
        base.Start();
        HideUI();
        HideElement(_endNotice);
        HideElement(_gameStartCountdown);
    }

    private void Update()
    {
        if (!GameManager.Instance.GameStarted || GameManager.Instance.GameIsOver) return;
        Countdown();
        UpdateTimer();
    }

    public void ResetTimeLeft()
    {
        _timeLeft = GameLength;
    }

    private void Countdown()
    {
        _timeLeft -= Time.deltaTime;
        if (_timeLeft <= 0)
        {
            ShowElement(_endNotice);
            Helpers.Instance.Delay(3f, () => { HideElement(_endNotice); });
            GameManager.Instance.EndGame(_heartCount);
        }
    }

    public void GameStartCountdown()
    {
        _hook.enabled = false;
        _heart.enabled = false;
        _progressBar.enabled = false;
        _gameStartCountdown.text = "3";
        ShowElement(_gameStartCountdown);
        Helpers.Instance.Delay(1f, () => { _gameStartCountdown.text = "2"; });
        Helpers.Instance.Delay(2f, () => { _gameStartCountdown.text = "1"; });
        Helpers.Instance.Delay(3f, () =>
        {
            HideElement(_gameStartCountdown);
            _hook.enabled = true;
            _heart.enabled = true;
            _progressBar.enabled = true;
        });
    }

    private void UpdateTimer()
    {
        _timer.text = Mathf.Round(_timeLeft).ToString(CultureInfo.InvariantCulture) + "s"; // something about how it converts numbers to string
    }

    public void IncrementHeartCounter()
    {
        _heartCount++;
        _heartCounter.text = _heartCount.ToString(CultureInfo.InvariantCulture);
    }

    public void ResetHeartCounter()
    {
        _heartCount = 0;
        _heartCounter.text = "0";
    }
    
}
