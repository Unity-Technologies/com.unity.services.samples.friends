using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Unity.Services.Friends.Models;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnityGamingServicesUsesCases.Relationships.UI
{
    public class PlayerEntryControl
    {
        const string k_PlayerEntryRootName = "base-player-entry";
        Label m_PlayerName;
        Label m_PlayerActivity;
        Label m_PlayerStatusLabel;
        VisualElement m_PlayerStatusCircle;
        DropdownField m_PlayerStatusDropDown;

        public PlayerEntryControl(VisualElement viewParent, bool localPlayerEntry = false)
        {
            var playerEntryView = viewParent.Q(k_PlayerEntryRootName);
            m_PlayerName = playerEntryView.Q<Label>("player-name-label");
            m_PlayerStatusDropDown = playerEntryView.Q<DropdownField>("player-status-dropdown");
            m_PlayerStatusLabel = playerEntryView.Q<Label>("player-status-label");
            m_PlayerStatusCircle = playerEntryView.Q<VisualElement>("player-status-circle");
            m_PlayerActivity = playerEntryView.Q<Label>("player-activity-label");

            //Set dropdown choices to the available presence enums
            m_PlayerStatusDropDown.choices = Enum.GetNames(typeof(PresenceAvailabilityOptions)).ToList();

            //Remove the "Unknown" choice for the local player, callback for changing the status color
            if (localPlayerEntry)
            {
                m_PlayerStatusLabel.style.display = DisplayStyle.None;
                m_PlayerStatusDropDown.style.display = DisplayStyle.Flex;
                m_PlayerStatusDropDown.choices.Remove(m_PlayerStatusDropDown.choices.Last());
                m_PlayerStatusDropDown.RegisterValueChangedCallback(choice =>
                {
                    var choiceInt = m_PlayerStatusDropDown.choices.IndexOf(choice.newValue);
                    SetStatusColor(choiceInt);
                });
            }

            SetStatus(PresenceAvailabilityOptions.OFFLINE);
        }


        public void SetName(string name)
        {
            if (m_PlayerName.text == name)
                return;
            m_PlayerName.text = name;
        }

        public void SetActivity(string activity)
        {
            if (m_PlayerActivity.text == activity)
                return;
            m_PlayerActivity.text = activity;
        }

        /// <summary>
        /// Since the SDK status integers start at 1, we need to shift them over to use our arrays/
        /// </summary>
        public void SetStatus(PresenceAvailabilityOptions presenceStatus)
        {
            //Clamp my choice to the choices available to the dropdown element
            var statusIntToIndex = GetChoiceIndex(presenceStatus);
            var dropDownChoice = m_PlayerStatusDropDown.choices[statusIntToIndex];
            m_PlayerStatusDropDown.SetValueWithoutNotify(dropDownChoice);
            SetStatusColor(statusIntToIndex);
        }

        void SetStatusColor(int statusIndex)
        {
            var validColor = m_PresenceUIColors[statusIndex];
            m_PlayerStatusLabel.style.color = validColor;
            m_PlayerStatusCircle.style.backgroundColor = validColor;
            m_PlayerStatusDropDown.style.color = validColor;
        }

        int GetChoiceIndex(PresenceAvailabilityOptions presenceStatus)
        {
            return Mathf.Clamp((int)presenceStatus - 1, 0, m_PlayerStatusDropDown.choices.Count);
        }


        //Mapping of colors to PresenceAvailabilityOptions
        Color[] m_PresenceUIColors =
        {
            new Color(.1f, .8f, .1f), //ONLINE
            new Color(.8f, .7f, .2f), //BUSY
            new Color(.7f, .2f, .1f), //AWAY
            new Color(.4f, .1f, .6f), //INVISIBLE
            new Color(.4f, .4f, .4f), //OFFLINE
            new Color(1f, .4f, 1f) //UNKNOWN
        };
    }
}
