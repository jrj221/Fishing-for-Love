using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class EndingUIManager : UIManger
{
    public static EndingUIManager Instance { get; private set; }
    private Image _goodEnding;
    private Image _midEnding;
    private Image _badEnding;
    private Label _endingDesc;
    private Label _heartCountNotice;
    private Button _restartButton;
    private EventCallback<ClickEvent> _restartGameCallback;
    
    public enum Ending
    {
        Good,
        Mid,
        Bad,
    }
    
    protected override void Awake()
    {
        base.Awake();
        Instance = this;
        _endingDesc =  GetElement<Label>("EndingDesc");
        _heartCountNotice = GetElement<Label>("HeartCountNotice");
        _restartButton = GetElement<Button>("RestartButton");
        _goodEnding = GetElement<Image>("GoodEnding");
        _midEnding = GetElement<Image>("MidEnding");
        _badEnding = GetElement<Image>("BadEnding");
        _restartGameCallback = (evt) => GameManager.Instance.RestartGame();
    }

    private void OnEnable()
    {
        _restartButton.RegisterCallback(_restartGameCallback);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _restartButton.UnregisterCallback(_restartGameCallback);
    }

    protected override void Start()
    {
        base.Start();
        HideUI();
        HideElements();
    }

    public void HideElements()
    {
        HideElement(_heartCountNotice);
        HideElement(_endingDesc);
        HideElement(_restartButton);
        HideElement(_goodEnding);
        HideElement(_midEnding);
        HideElement(_badEnding);
    }
    
    public void ShowEnding(Ending ending, int heartCount)
    {
        ShowUI();
        ApplyEndingText(ending, heartCount);
        ElementFadeIn(_heartCountNotice, 2f);
        ElementFadeIn(_endingDesc, 2f);
        Image endingImage;
        switch (ending)
        {
            case Ending.Good: 
                endingImage = _goodEnding;
                break;
            case Ending.Mid: 
                endingImage = _midEnding;
                break;
            default:
                endingImage = _badEnding;
                break;
        }
        Helpers.Instance.Delay(5f, () =>
        {
            ElementFadeOut(_heartCountNotice, 2f);
            ElementFadeOut(_endingDesc, 2f);
            Helpers.Instance.Delay(2f, () => ElementFadeIn(endingImage, 2f));
        });
        Helpers.Instance.Delay(10f, () => ElementFadeIn(_restartButton, 2f));
    }
    
    private void ApplyEndingText(Ending ending, int heartCount)
    {
        _heartCountNotice.text = "You got " + heartCount + " heart";
        if (heartCount != 1) _heartCountNotice.text += "s";
        switch (ending)
        {
            case Ending.Good:
                _endingDesc.text = "I think she wants to be your valentine!!";
                break;
            case Ending.Mid:
                _endingDesc.text = "It's alright, but I don't think there'll be a second date...";
                break;
            case Ending.Bad:
                _endingDesc.text = "Yeah... better luck next time...";
                break;
        }
    }
}
