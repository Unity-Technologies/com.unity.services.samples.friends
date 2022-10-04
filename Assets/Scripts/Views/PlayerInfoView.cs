using System;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Friends.Models;
using UnityEngine;

namespace UnityGamingServicesUsesCases.Relationships
{
    public class PlayerInfoView : MonoBehaviour
    {
        public Action<(PresenceAvailabilityOptions,string)> OnPresenceChanged;

        [SerializeField] private TextMeshProUGUI m_Text = null;
        [SerializeField] private TMP_InputField m_IdInputField = null;
        [SerializeField] private TMP_InputField m_ActivityInputField = null;
        [SerializeField] private TMP_Dropdown m_PresenceDropdown = null;

        private void Awake()
        {
            var names = new List<string>
            {
                PresenceAvailabilityOptions.ONLINE.ToString(),
                PresenceAvailabilityOptions.BUSY.ToString(),
                PresenceAvailabilityOptions.AWAY.ToString(),
                PresenceAvailabilityOptions.INVISIBLE.ToString()
            };

            m_PresenceDropdown.AddOptions(names);
            m_PresenceDropdown.onValueChanged.AddListener((value) =>
            {
                OnStatusChanged(value,m_ActivityInputField.text);
            });
            m_ActivityInputField.onEndEdit.AddListener((value) =>
            {
                OnStatusChanged(m_PresenceDropdown.value,value);
            });
        }

        private void OnStatusChanged(int value, string activity)
        {
            var presence = (PresenceAvailabilityOptions) Enum.Parse(typeof(PresenceAvailabilityOptions),
                m_PresenceDropdown.options[value].text, true);
                
            OnPresenceChanged?.Invoke((presence,activity));
        }

        public void Refresh(string name, string id,PresenceAvailabilityOptions presence)
        {
            m_Text.text = name;
            m_IdInputField.text = id;
            var index = (int)presence - 1;
            m_PresenceDropdown.SetValueWithoutNotify(index);
            m_ActivityInputField.text = string.Empty;
        }
    }
}