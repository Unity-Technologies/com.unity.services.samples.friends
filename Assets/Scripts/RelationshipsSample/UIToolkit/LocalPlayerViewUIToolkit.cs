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
        static readonly string[] k_LocalPlayerChoices = { "Online", "Busy", "Away", "Invisible" };

        public Action<(PresenceAvailabilityOptions, string)> onPresenceChanged { get; set; }

        DropdownField m_PlayerStatusDropDown;
        Label m_PlayerName;
        TextField m_PlayerActivity;
        TextField m_PlayerId;
        VisualElement m_PlayerStatusCircle;
        Button m_AcceptChangeButton;
        Button m_CancelChangeButton;
        string m_LastActivityString;

        public LocalPlayerViewUIToolkit(VisualElement viewParent)
        {
            var playerEntryView = viewParent.Q(k_PlayerEntryRootName);
            m_PlayerStatusDropDown = playerEntryView.Q<DropdownField>("player-status-dropdown");
            m_PlayerName = playerEntryView.Q<Label>("player-name-label");
            m_PlayerId = playerEntryView.Q<TextField>("id-field");

            m_PlayerStatusCircle = playerEntryView.Q<VisualElement>("player-status-circle");
            m_PlayerActivity = playerEntryView.Q<TextField>("player-activity-field");
            m_AcceptChangeButton = m_PlayerActivity.Q<Button>("player-accept");
            m_CancelChangeButton = m_PlayerActivity.Q<Button>("player-cancel");

            m_AcceptChangeButton.RegisterCallback<ClickEvent>((_) =>
            {
                var currentOption = ParseStatus(m_PlayerStatusDropDown.value);
                onPresenceChanged?.Invoke((currentOption, m_PlayerActivity.text));
                m_PlayerActivity.Blur();
            });

            m_CancelChangeButton.RegisterCallback<ClickEvent>((_) =>
            {
                m_PlayerActivity.Blur();
            });

            m_PlayerStatusDropDown.choices = k_LocalPlayerChoices.ToList();

            m_PlayerStatusDropDown.RegisterValueChangedCallback(choice =>
            {
                var choiceInt = m_PlayerStatusDropDown.choices.IndexOf(choice.newValue);
                SetPresenceColor((PresenceAvailabilityOptions)(choiceInt + 1));
                var option = ParseStatus(choice.newValue);
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
            m_PlayerActivity.SetValueWithoutNotify(activity);
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
        }

        PresenceAvailabilityOptions ParseStatus(string status)
        {
            var capsStatus = status.ToUpper();
            if (Enum.TryParse(capsStatus, out PresenceAvailabilityOptions parsedOption))
            {
                return parsedOption;
            }

            return PresenceAvailabilityOptions.UNKNOWN;
        }
    }
}