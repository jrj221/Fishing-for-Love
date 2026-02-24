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
    private Button _backToMenuButton;
    private EventCallback<ClickEvent> _restartGameCallback;
    private EventCallback<ClickEvent> _backToMenuCallback;
    
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
        _backToMenuButton = GetElement<Button>("BackToMenuButton");
        _goodEnding = GetElement<Image>("GoodEnding");
        _midEnding = GetElement<Image>("MidEnding");
        _badEnding = GetElement<Image>("BadEnding");
        _restartGameCallback = (evt) =>
        {
            ResetEnding();
            GameManager.Instance.RestartGame();
        };
        _backToMenuCallback = (evt) =>
        {
            ResetEnding();
            GameManager.Instance.BackToMenu();
        };
    }

    private void OnEnable()
    {
        _restartButton.RegisterCallback(_restartGameCallback);
        _backToMenuButton.RegisterCallback(_backToMenuCallback);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _restartButton.UnregisterCallback(_restartGameCallback);
        _backToMenuButton.UnregisterCallback(_backToMenuCallback);
    }

    protected override void Start()
    {
        base.Start();
        ResetEnding();
    }

    private void ResetEnding()
    {
        HideUI();
        HideAllElements();
    }

    public void HideAllElements()
    {
        HideElement(_heartCountNotice);
        HideElement(_endingDesc);
        HideElement(_restartButton);
        HideElement(_backToMenuButton);
        HideElement(_goodEnding);
        HideElement(_midEnding);
        HideElement(_badEnding);
    }
    
    public void ShowEnding(Ending ending, int heartCount)
    {
        HideAllElements();
        ShowUI();
        var descriptionTime = 7f;
        SetDescriptionState(descriptionTime, ending, heartCount);
        
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

        var imageFadeInTime = 2f;
        var buttonsFadeInDelay = 2f;
        var buttonsFadeInTime = 2f;
        Helpers.Instance.Delay(descriptionTime, () =>
        {
            ElementFadeIn(endingImage, imageFadeInTime);
            Helpers.Instance.Delay(imageFadeInTime + buttonsFadeInDelay, () =>
            {
                ElementFadeIn(_restartButton, buttonsFadeInTime);
                ElementFadeIn(_backToMenuButton, buttonsFadeInTime);
            });
        });
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

    private void SetDescriptionState(float time, Ending ending,  int heartCount)
    {
        time = Mathf.Min(7f, time); // At least 2 seconds fade in, 3 to read the text, and 2 to fade out
        float fadeTime = (time - 3) / 2;
        
        // Fade in...
        ApplyEndingText(ending, heartCount);
        ElementFadeIn(_heartCountNotice, fadeTime);
        ElementFadeIn(_endingDesc, fadeTime);
        
        // Hold for a few seconds... then fade out...
        Helpers.Instance.Delay(time - (fadeTime * 2), () =>
        {
            ElementFadeOut(_heartCountNotice, fadeTime);
            ElementFadeOut(_endingDesc, fadeTime);
        });
    }
}
