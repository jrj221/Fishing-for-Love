using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class EndingUIManager : UIManger
{
    public static EndingUIManager Instance { get; private set; }
    private Image _ending;
    private Label _endingDesc;
    private Label _heartCountNotice;
    
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
    }

    private void Start()
    {
        HideUI();
    }
    
    public void ShowEnding(Ending ending, int heartCount)
    {
        ShowUI();
        ApplyEndingText(ending, heartCount);
        ShowElement(_heartCountNotice);
        ShowElement(_endingDesc);
        _ending = GetElement<Image>(ending + "Ending");
        Helpers.Instance.Delay(5f, () =>
        {
            HideElement(_heartCountNotice);
            HideElement(_endingDesc);
            ShowElement(_ending);
        });
    }
    
    private void ApplyEndingText(Ending ending, int heartCount)
    {
        _heartCountNotice.text = "You got " + heartCount + " hearts.";
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
