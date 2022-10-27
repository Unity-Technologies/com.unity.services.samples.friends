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

        [SerializeField] TextMeshProUGUI m_NameText = null;
        [SerializeField] TMP_InputField m_Id = null;
        [SerializeField] TMP_InputField m_Activity = null;
        [SerializeField] TMP_Dropdown m_PresenceSelector = null;
        [SerializeField] Image m_PresenceColor = null;
        [SerializeField] TextMeshProUGUI m_PresenceText = null;
        [SerializeField] Button m_CopyButton = null;

        void Awake()
        {
            var names = new List<string>
            {
                PresenceAvailabilityOptions.ONLINE.ToString(),
                PresenceAvailabilityOptions.BUSY.ToString(),
                PresenceAvailabilityOptions.AWAY.ToString(),
                PresenceAvailabilityOptions.INVISIBLE.ToString()
            };

            m_PresenceSelector.AddOptions(names);
            m_PresenceSelector.onValueChanged.AddListener((value) => { OnStatusChanged(value, m_Activity.text); });
            m_Activity.onEndEdit.AddListener((value) => { OnStatusChanged(m_PresenceSelector.value, value); });
            m_CopyButton.onClick.AddListener(() => { GUIUtility.systemCopyBuffer = m_Id.text; });
        }

        void OnStatusChanged(int value, string activity)
        {
            var presence = (PresenceAvailabilityOptions)Enum.Parse(typeof(PresenceAvailabilityOptions),
                m_PresenceSelector.options[value].text, true);

            onPresenceChanged?.Invoke((presence, activity));
        }

        public void Refresh(string name, string id, string activity, PresenceAvailabilityOptions presenceAvailabilityOptions)
        {
            m_NameText.text = name;
            m_Id.text = id;

            //Presence
            var index = (int)presenceAvailabilityOptions - 1;
            m_PresenceSelector.SetValueWithoutNotify(index);
            var presenceColor = ColorUtils.GetPresenceColor(presenceAvailabilityOptions);
            m_PresenceColor.color = presenceColor;
            m_PresenceText.color = presenceColor;

            m_Activity.text = activity;
        }
    }
}