using System;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class UIBaseView
{
    // visual elements
    protected VisualElement m_ViewRoot;

    public event Action ScreenStarted;
    public event Action ScreenEnded;

    /// <summary>
    /// Should be set to the name of the top level viewRoot we are looking for
    /// </summary>
    public abstract string ViewName { get; }

    public UIBaseView(VisualElement viewRoot)
    {
        m_ViewRoot = viewRoot;
        SetVisualElements();
        RegisterButtonCallbacks();
    }

    protected abstract void SetVisualElements();

    // Once you have the VisualElements, you can add button events here, using the RegisterCallback functionality.
    // This allows you to use a number of different events (ClickEvent, ChangeEvent, etc.)
    protected abstract void RegisterButtonCallbacks();

    public bool IsVisible()
    {
        if (m_ViewRoot == null)
            return false;

        return (m_ViewRoot.style.display == DisplayStyle.Flex);
    }

    // Toggle a UI on and off using the DisplayStyle.
    public static void ShowVisualElement(VisualElement visualElement, bool state)
    {
        if (visualElement == null)
            return;

        visualElement.style.display = (state) ? DisplayStyle.Flex : DisplayStyle.None;
    }

    public T GetElementByName<T>(string elementName) where T : VisualElement
    {
        if (string.IsNullOrEmpty(elementName) || m_ViewRoot == null)
            return default;

        // query and return the element
        return m_ViewRoot.Q<T>(elementName);
    }

    public virtual void ShowScreen()
    {
        ShowVisualElement(m_ViewRoot, true);
        ScreenStarted?.Invoke();
    }

    public virtual void HideScreen()
    {
        if (IsVisible())
        {
            ShowVisualElement(m_ViewRoot, false);
            ScreenEnded?.Invoke();
        }
    }
}