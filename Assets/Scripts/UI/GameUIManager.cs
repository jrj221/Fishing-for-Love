using System.Globalization;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class GameUIManager : UIManger
{
    private Label _timer;
    private Label _heartCounter;
    private float _timeLeft = 60;
    private float _heartCount;
    public static GameUIManager Instance { get; private set; }
    
    protected override void Awake()
    {
        base.Awake();
        Instance = this;
        _timer = GetLabel("Timer");
        _heartCounter = GetLabel("HeartCounter");
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
            GameManager.Instance.EndGame();
        }
    }

    private void UpdateTimer()
    {
        _timer.text = Mathf.Round(_timeLeft).ToString(CultureInfo.InvariantCulture) + "s"; // something about how it converts numbers to string
    }

    public void IncrementHeartCounter()
    {
        _heartCount++;
        _heartCounter.text = "Hearts: " + _heartCount.ToString(CultureInfo.InvariantCulture);
    }
    
}
