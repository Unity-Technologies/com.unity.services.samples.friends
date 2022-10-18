using System;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Friends.Models;
using UnityEngine;
using UnityEngine.UI;

namespace UnityGamingServicesUsesCases.Relationships.UGUI
{
    public class LocalPlayerViewUGUI : MonoBehaviour, ILocalPlayerView
    {
        public Action<(PresenceAvailabilityOptions, string)> onPresenceChanged { get; set; }

        [SerializeField] private TextMeshProUGUI m_NameText = null;
        [SerializeField] private TMP_InputField m_Id = null;
        [SerializeField] private TMP_InputField m_Activity = null;
        [SerializeField] private TMP_Dropdown m_PresenceSelector = null;
        [SerializeField] private Image m_PresenceColor = null;
        [SerializeField] private TextMeshProUGUI m_PresenceText = null;
        
        private void Awake()
        {
            var names = new List<string>
            {
                PresenceAvailabilityOptions.ONLINE.ToString(),
                PresenceAvailabilityOptions.BUSY.ToString(),
                PresenceAvailabilityOptions.AWAY.ToString(),
                PresenceAvailabilityOptions.INVISIBLE.ToString()
            };

            m_PresenceSelector.AddOptions(names);
            m_PresenceSelector.onValueChanged.AddListener((value) =>
            {
                OnStatusChanged(value,m_Activity.text);
            });
            m_Activity.onEndEdit.AddListener((value) =>
            {
                OnStatusChanged(m_PresenceSelector.value,value);
            });
        }

        private void OnStatusChanged(int value, string activity)
        {
            var presence = (PresenceAvailabilityOptions) Enum.Parse(typeof(PresenceAvailabilityOptions),
                m_PresenceSelector.options[value].text, true);
                
            onPresenceChanged?.Invoke((presence,activity));
        }

        public void Refresh(string name, string id, string activity, PresenceAvailabilityOptions presenceAvailabilityOptions)
        {
            m_NameText.text = name;
            m_Id.text = id;
            
            //Presence
            var index = (int)presenceAvailabilityOptions - 1;
            m_PresenceSelector.SetValueWithoutNotify(index);
            var presenceColor =  UIUtils.GetPresenceColor(presenceAvailabilityOptions);
            m_PresenceColor.color = presenceColor;
            m_PresenceText.color = presenceColor;
            
            m_Activity.text = activity;
        }
    }
}