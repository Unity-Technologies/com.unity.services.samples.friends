using System;
using Unity.Services.Friends.Models;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnityGamingServicesUsesCases.Relationships.UI
{
    public class PlayerEntryControl
    {
        const string k_PlayerEntryRootName = "base-player-entry";

        public Action<PresenceAvailabilityOptions> onStatusChanged;
        public Action<string> onActivityChanged;

        Label m_PlayerName;
        Label m_PlayerActivity;
        Label m_PlayerStatusLabel;
        VisualElement m_PlayerStatusCircle;
        DropdownField m_PlayerStatusDropDown;

        public PlayerEntryControl(VisualElement viewParent)
        {
            var playerEntryView = viewParent.Q(k_PlayerEntryRootName);
            m_PlayerName = playerEntryView.Q<Label>("player-name-label");
            m_PlayerStatusDropDown = playerEntryView.Q<DropdownField>("player-status-dropdown");
            m_PlayerStatusLabel = playerEntryView.Q<Label>("player-status-label");
            m_PlayerStatusCircle = playerEntryView.Q<VisualElement>("player-status-circle");
            m_PlayerActivity = playerEntryView.Q<Label>("player-activity-label");
            SetStatus(PresenceAvailabilityOptions.OFFLINE);

            m_PlayerStatusDropDown.RegisterValueChangedCallback(status =>
            {
                var capsValue = status.newValue.ToUpper();
                if (Enum.TryParse(capsValue, out PresenceAvailabilityOptions result))
                {
                    onStatusChanged?.Invoke(result);
                    SetStatusColor(result);
                }
            });
        }

        public void SetName(string name)
        {
            m_PlayerName.text = name;
        }

        public void SetActivity(string activity)
        {
            m_PlayerActivity.text = activity;
        }

        public void SetStatus(PresenceAvailabilityOptions status)
        {
            m_PlayerStatusDropDown.SetValueWithoutNotify(status.ToString());
            SetStatusColor(status);
        }

        void SetStatusColor(PresenceAvailabilityOptions status)
        {
            //Shifted to adapt the PresenceAvailabilityOptions enum-integers
            var validColor = m_PresenceUIColors[(int)status - 1];
            m_PlayerStatusLabel.style.color = validColor;
            m_PlayerStatusCircle.style.backgroundColor = validColor;
            m_PlayerStatusDropDown.style.color = validColor;
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