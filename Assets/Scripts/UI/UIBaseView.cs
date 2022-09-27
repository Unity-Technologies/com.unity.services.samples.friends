using System;

using UnityEngine;
using UnityEngine.UIElements;

public abstract class UIBaseView : MonoBehaviour
{
    [Tooltip("String ID from the UXML for this menu panel/screen.")]
    [SerializeField]
    protected string m_ScreenName;

    [Header("UI Management")]
    [Tooltip("Set the Social UI Control here explicitly (or get automatically from current GameObject).")]
    [SerializeField]
    protected SocialUIController m_MainMenuUIManager;
    [Tooltip("Set the UI Document here explicitly (or get automatically from current GameObject).")]
    [SerializeField]
    protected UIDocument m_Document;

    // visual elements
    protected VisualElement m_Screen;
    protected VisualElement m_Root;

    public event Action ScreenStarted;
    public event Action ScreenEnded;

    //  UXML element name (defaults to the class name)
    protected virtual void OnValidate()
    {
        if (string.IsNullOrEmpty(m_ScreenName))
            m_ScreenName = this.GetType().Name;
    }

    protected virtual void Awake()
    {
        // set up MainMenuUIManager and UI Document
        if (m_MainMenuUIManager == null)
            m_MainMenuUIManager = GetComponent<SocialUIController>();

        // default to current UIDocument if not set in Inspector
        if (m_Document == null)
            m_Document = GetComponent<UIDocument>();

        if (m_Document == null)
        {
            Debug.LogWarning("SocialUI " + name + ": missing UIDocument. Check Script Execution Order.", gameObject);
            return;
        }

        SetVisualElements();
        RegisterButtonCallbacks();
    }

    // The general workflow uses string IDs to query the VisualTreeAsset and find matching Visual Elements in the UXML.
    // This is a shorthand for finding the Class name and
    protected virtual void SetVisualElements()
    {
        // get a reference to the root VisualElement
        if (m_Document != null)
            m_Root = m_Document.rootVisualElement;

        m_Screen = GetVisualElement(m_ScreenName);
    }

    // Once you have the VisualElements, you can add button events here, using the RegisterCallback functionality.
    // This allows you to use a number of different events (ClickEvent, ChangeEvent, etc.)
    protected virtual void RegisterButtonCallbacks() { }

    public bool IsVisible()
    {
        if (m_Screen == null)
            return false;

        return (m_Screen.style.display == DisplayStyle.Flex);
    }

    // Toggle a UI on and off using the DisplayStyle.
    public static void ShowVisualElement(VisualElement visualElement, bool state)
    {
        if (visualElement == null)
            return;

        visualElement.style.display = (state) ? DisplayStyle.Flex : DisplayStyle.None;
    }

    // returns an element by name
    public VisualElement GetVisualElement(string elementName)
    {
        if (string.IsNullOrEmpty(elementName) || m_Root == null)
            return null;

        // query and return the element
        return m_Root.Q(elementName);
    }

    public virtual void ShowScreen()
    {
        ShowVisualElement(m_Screen, true);
        ScreenStarted?.Invoke();
    }

    public virtual void HideScreen()
    {
        if (IsVisible())
        {
            ShowVisualElement(m_Screen, false);
            ScreenEnded?.Invoke();
        }
    }
}