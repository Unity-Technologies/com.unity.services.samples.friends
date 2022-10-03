using System;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Friends.Models;
using UnityEngine;

namespace UnityGamingServicesUsesCases.Relationships
{
    public class PlayerInfoView : MonoBehaviour
    {
        public Action<PresenceAvailabilityOptions> OnPresenceChanged;

        [SerializeField] private TextMeshProUGUI m_Text = null;
        [SerializeField] private TMP_InputField m_InputField = null;
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
                var presence = (PresenceAvailabilityOptions) Enum.Parse(typeof(PresenceAvailabilityOptions),
                    m_PresenceDropdown.options[value].text, true);
                
                OnPresenceChanged?.Invoke(presence);
            });
        }

        public void Refresh(string name, string id,PresenceAvailabilityOptions presence)
        {
            m_Text.text = name;
            m_InputField.text = id;
            var index = (int)presence - 1;
            m_PresenceDropdown.SetValueWithoutNotify(index);
        }
    }
}