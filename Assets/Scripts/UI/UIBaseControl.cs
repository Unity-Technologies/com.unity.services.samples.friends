using System;
using UnityEngine;
using UnityEngine.UIElements;
/// <summary>
/// Inherit from this class to create a control pairing to your UIToolkit views,
/// Set the ViewName to the top-level ViewElement in the template or UI document you wish to query from.
/// </summary>
[System.Serializable]

public abstract class UIBaseControl
{
    // visual elements
    protected VisualElement m_ViewRoot;

    public event Action OnScreenStarted;
    public event Action OnScreenEnded;

    /// <summary>
    /// Should be set to the name of the top level viewRoot we are looking for
    /// </summary>
    public abstract string ViewRootName { get; }

    /// <summary>
    /// Base Concstructor for the UI Control Classes
    /// </summary>
    /// <param name="documentParent">The top level root you want to query for elements within.</param>
    protected UIBaseControl(VisualElement documentParent)
    {
        m_ViewRoot = documentParent.Q(ViewRootName);
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
        OnScreenStarted?.Invoke();
    }

    public virtual void HideScreen()
    {
        if (IsVisible())
        {
            ShowVisualElement(m_ViewRoot, false);
            OnScreenEnded?.Invoke();
        }
    }
}
