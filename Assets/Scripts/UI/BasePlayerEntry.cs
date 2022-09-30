using System;
using System.Collections.Generic;
using Unity.Services.Friends.Models;
using UnityEngine;
using UnityEngine.UIElements;

public class BasePlayerEntry : UIBaseView
{
    public override string ViewName => "BasePlayerEntry";
    public Action<string> OnStatusChanged;

    Label m_PlayerName;
    DropdownField m_PlayerStatusDropDown;
    Label m_PlayerStatusLabel;
    Label m_PlayerActivity;

    public BasePlayerEntry(VisualElement viewRoot)
        : base(viewRoot) { }

    protected override void SetVisualElements()
    {
        m_PlayerName = GetElementByName<Label>("player-name-label");
        m_PlayerStatusDropDown = GetElementByName<DropdownField>("player-name-label");
        m_PlayerStatusLabel = GetElementByName<Label>("player-status-label");
        m_PlayerActivity = GetElementByName<Label>("player-activity-label");
    }

    public void SetStatus(PresenceAvailabilityOptions status)
    {

    }

    protected override void RegisterButtonCallbacks()
    {
        m_PlayerStatusDropDown.RegisterValueChangedCallback(status =>
        {
            OnStatusChanged?.Invoke(status.newValue);
        });
    }
}