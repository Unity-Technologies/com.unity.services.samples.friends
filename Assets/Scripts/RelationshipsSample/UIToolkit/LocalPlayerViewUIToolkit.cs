using System;
using System.Linq;
using Unity.Services.Friends.Models;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnityGamingServicesUsesCases.Relationships.UIToolkit
{
    public class LocalPlayerViewUIToolkit : ILocalPlayerView
    {
        const string k_PlayerEntryRootName = "local-player-entry";

        //We dont support the player selecting OFFLINE or UNKNOWN with the UI
        static readonly string[] k_LocalPlayerChoices = { "ONLINE", "BUSY", "AWAY", "INVISIBLE" };

        public Action<(PresenceAvailabilityOptions, string)> onPresenceChanged { get; set; }

        //TODO Add Editable Activity Field?

        DropdownField m_PlayerStatusDropDown;
        TextElement m_DropDownText;
        Label m_PlayerName;
        Label m_PlayerActivity;
        TextField m_PlayerId;
        VisualElement m_PlayerStatusCircle;

        public LocalPlayerViewUIToolkit(VisualElement viewParent)
        {
            var playerEntryView = viewParent.Q(k_PlayerEntryRootName);
            m_PlayerStatusDropDown = playerEntryView.Q<DropdownField>("player-status-dropdown");
            m_DropDownText = m_PlayerStatusDropDown.Q<TextElement>();
            m_PlayerName = playerEntryView.Q<Label>("player-name-label");
            m_PlayerId = playerEntryView.Q<TextField>("id-field");

            m_PlayerStatusCircle = playerEntryView.Q<VisualElement>("player-status-circle");
            m_PlayerActivity = playerEntryView.Q<Label>("player-activity-label");

            m_PlayerStatusDropDown.choices = k_LocalPlayerChoices.ToList();

            m_PlayerStatusDropDown.RegisterValueChangedCallback(choice =>
            {
                var choiceInt = m_PlayerStatusDropDown.choices.IndexOf(choice.newValue);
                SetPresenceColor((PresenceAvailabilityOptions)(choiceInt + 1));
                PresenceAvailabilityOptions option = PresenceAvailabilityOptions.UNKNOWN;
                if (Enum.TryParse(choice.newValue, out PresenceAvailabilityOptions parsedOption))
                    option = parsedOption;
                onPresenceChanged?.Invoke((option, m_PlayerActivity.text));
            });

            SetPresence(PresenceAvailabilityOptions.INVISIBLE);
        }

        //Keeping these setters seperate in case we wan to support name and activity changesxwx`

        public void Refresh(string name, string id, string activity,
            PresenceAvailabilityOptions presenceAvailabilityOptions)
        {
            m_PlayerName.text = name;
            m_PlayerId.SetValueWithoutNotify(id);
            m_PlayerActivity.text = activity;
            SetPresence(presenceAvailabilityOptions);
        }

        void SetPresence(PresenceAvailabilityOptions presenceStatus)
        {
            var clampedStatusIndex = Mathf.Clamp((int)presenceStatus - 1, 0, k_LocalPlayerChoices.Length - 1);
            var dropDownChoice = m_PlayerStatusDropDown.choices[clampedStatusIndex];
            m_PlayerStatusDropDown.SetValueWithoutNotify(dropDownChoice);
            SetPresenceColor(presenceStatus);
        }

        void SetPresenceColor(PresenceAvailabilityOptions presenceStatus)
        {
            var presenceColor = ColorUtils.GetPresenceColor(presenceStatus);
            m_PlayerStatusCircle.style.backgroundColor = presenceColor;
            m_DropDownText.style.color = presenceColor;
        }
    }
}