using System.Globalization;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class GameUIManager : MonoBehaviour
{
    private UIDocument _document;
    private Label _timer;
    private Label _heartCounter;
    private float _timeLeft = 60;
    private float _heartCount = 0;
    public static GameUIManager Instance { get; private set; }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        Instance = this;
        _document = GetComponent<UIDocument>();
        _timer = _document.rootVisualElement.Q<Label>("Timer");
        _heartCounter = _document.rootVisualElement.Q<Label>("HeartCounter");
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
    
    public void ShowUI()
    {
        _document.rootVisualElement.style.display = DisplayStyle.Flex;
    }

    public void HideUI()
    {
        _document.rootVisualElement.style.display = DisplayStyle.None;
    }
}
