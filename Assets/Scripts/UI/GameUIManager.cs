using System.Globalization;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class GameUIManager : UIManger
{
    [SerializeField] private float _gameLength;
    private Label _timer;
    private Label _heartCounter;
    private float _timeLeft;
    private int _heartCount;
    public static GameUIManager Instance { get; private set; }
    
    protected override void Awake()
    {
        base.Awake();
        Instance = this;
        _timer = GetElement<Label>("Timer");
        _heartCounter = GetElement<Label>("HeartCounter");
        _timeLeft = _gameLength;
    }

    private void Start()
    {
        HideUI();
    }

    private void Update()
    {
        Countdown();
        UpdateTimer();
    }

    private void Countdown()
    {
        _timeLeft -= Time.deltaTime;
        if (_timeLeft <= 0)
        {
            GameManager.Instance.EndGame(_heartCount);
        }
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
    
}
