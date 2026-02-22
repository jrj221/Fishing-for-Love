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

    protected T GetElement<T>(string elementName) where T : VisualElement // equivalent of T extends VisualElement
    {
        return _document.rootVisualElement.Q<T>(elementName);
    }

    protected void ShowElement(VisualElement element)
    {
        element.style.display = DisplayStyle.Flex;
    }
    
    protected void HideElement(VisualElement element)
    {
        element.style.display = DisplayStyle.None;
    }
}
