using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class UIManger : MonoBehaviour
{
    private UIDocument _document;

    protected virtual void Awake()
    {
        _document = GetComponent<UIDocument>();
    }

    public void ShowUI()
    {
        _document.rootVisualElement.style.display = DisplayStyle.Flex;
    }

    public void HideUI()
    {
        _document.rootVisualElement.style.display = DisplayStyle.None;
    }

    protected Button GetButton(string elementName)
    {
        return _document.rootVisualElement.Q<Button>(elementName);
    }
    
    protected Label GetLabel(string elementName)
    {
        return _document.rootVisualElement.Q<Label>(elementName);
    }
}
