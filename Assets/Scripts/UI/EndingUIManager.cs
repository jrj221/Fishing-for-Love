using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class EndingUIManager : UIManger
{
    public static EndingUIManager Instance { get; private set; }
    private Image _ending;
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
    }

    public void ShowEnding(Ending ending)
    {
        ShowUI();
        _ending = GetElement<Image>(ending + "Ending");
        ShowElement(_ending);
    }
}
