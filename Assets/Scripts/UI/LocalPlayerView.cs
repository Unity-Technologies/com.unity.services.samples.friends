using System;
using System.Linq;
using Unity.Services.Friends.Models;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnityGamingServicesUsesCases.Relationships.UI
{
    public class LocalPlayerView
    {
        const string k_PlayerEntryRootName = "base-player-entry";

        //We dont support the player selecting OFFLINE or UNKNOWN with the UI
        static readonly string[] k_LocalPlayerChoices = { "ONLINE", "BUSY", "AWAY", "INVISIBLE" };

        public Action<PresenceAvailabilityOptions> onPresenceChanged;
        //TODO With Presence PR Input fields for Names and Activities

        DropdownField m_PlayerStatusDropDown;
        Label m_PlayerName;
        Label m_PlayerActivity;
        VisualElement m_PlayerStatusCircle;

        public LocalPlayerView(VisualElement viewParent)
        {
            var playerEntryView = viewParent.Q(k_PlayerEntryRootName);
            m_PlayerStatusDropDown = playerEntryView.Q<DropdownField>("player-status-dropdown");
            m_PlayerName = playerEntryView.Q<Label>("player-name-label");
            m_PlayerStatusCircle = playerEntryView.Q<VisualElement>("player-status-circle");
            m_PlayerActivity = playerEntryView.Q<Label>("player-activity-label");

            m_PlayerStatusDropDown.choices = k_LocalPlayerChoices.ToList();

            m_PlayerStatusDropDown.RegisterValueChangedCallback(choice =>
            {
                var choiceInt = m_PlayerStatusDropDown.choices.IndexOf(choice.newValue);
                SetPresenceColor((PresenceAvailabilityOptions)choiceInt + 1);

                if (Enum.TryParse(choice.newValue, out PresenceAvailabilityOptions optionChoice))
                    onPresenceChanged?.Invoke(optionChoice);
            });

            SetPresence(PresenceAvailabilityOptions.INVISIBLE);
        }

        //Keeping these setters seperate in case we wan to support name and activity changes

        public void Refresh(string name, string activity, PresenceAvailabilityOptions presenceAvailabilityOptions)
        {
            m_PlayerName.text = name;
            m_PlayerActivity.text = activity;
            SetPresence(presenceAvailabilityOptions);
        }

        public void SetPresence(PresenceAvailabilityOptions presenceStatus)
        {
            var clampedStatusIndex = Mathf.Clamp((int)presenceStatus - 1, 0, k_LocalPlayerChoices.Length - 1);
            var dropDownChoice = m_PlayerStatusDropDown.choices[clampedStatusIndex];
            m_PlayerStatusDropDown.SetValueWithoutNotify(dropDownChoice);
        }

        void SetPresenceColor(PresenceAvailabilityOptions presenceStatus)
        {
            var presenceColor = UIUtils.GetPresenceColor(presenceStatus);
            m_PlayerStatusCircle.style.backgroundColor = presenceColor;
            m_PlayerStatusDropDown.style.color = presenceColor;
        }
    }
}