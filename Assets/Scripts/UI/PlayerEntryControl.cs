using System;
using Unity.Services.Friends.Models;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnityGamingServicesUsesCases.Relationships.UI
{
    public class PlayerEntryControl : UIBaseControl
    {
        const string k_playerEntryRootName = "base-player-entry";
        public DropdownField PlayerStatusDropDown { get; private set; }

        public string Id { get; private set; }
        Label m_PlayerName;
        Label PlayerActivity { get; set; }
        Label m_PlayerStatusLabel;
        VisualElement m_PlayerStatusCircle;

        public void SetPlayer(PlayerProfile playerProfile)
        {
            Id = playerProfile.Id;
            SetName(playerProfile.Name);

            //TODO Set Status
            //TODO Set Activity
        }

        public PlayerEntryControl(VisualElement documentParent)
            : base(documentParent)
        {

        }

        public override string ViewRootName => k_playerEntryRootName;

        protected override void SetVisualElements()
        {
            m_PlayerName = GetElementByName<Label>("player-name-label");
            PlayerStatusDropDown = GetElementByName<DropdownField>("player-status-dropdown");
            m_PlayerStatusLabel = GetElementByName<Label>("player-status-label");
            m_PlayerStatusCircle = GetElementByName<VisualElement>("player-status-circle");
            PlayerActivity = GetElementByName<Label>("player-activity-label");
            SetStatus(PresenceAvailabilityOptions.OFFLINE);
        }

        protected override void RegisterButtonCallbacks()
        {
            //Take care to make sure the Dropdown Element in the UI has exact string name matches with the PresenceAvailabilityOptions
            PlayerStatusDropDown.RegisterValueChangedCallback(status =>
            {
                var capsValue = status.newValue.ToUpper();
                if (Enum.TryParse(capsValue, out PresenceAvailabilityOptions result))
                    SetStatusColor(result);
            });
        }

        public void SetName(string name)
        {
            m_PlayerName.text = name;
        }

        public void SetActivity(string activity)
        {
            PlayerActivity.text = activity;
        }

        public void SetStatus(PresenceAvailabilityOptions status)
        {
            PlayerStatusDropDown.SetValueWithoutNotify(status.ToString());
            SetStatusColor(status);
        }

        void SetStatusColor(PresenceAvailabilityOptions status)
        {
            //Shifted to adapt the PresenceAvailabilityOptions enum-integers
            var validColor = m_PresenceUIColors[(int)status - 1];
            m_PlayerStatusLabel.style.color = validColor;
            m_PlayerStatusCircle.style.backgroundColor = validColor;
            PlayerStatusDropDown.style.color = validColor;
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