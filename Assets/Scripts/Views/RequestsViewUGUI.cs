using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityGamingServicesUsesCases.Relationships.UGUI
{
    public class RequestsViewUGUI : MonoBehaviour, IRequestListView
    {
        [SerializeField] private RectTransform m_ParentTransform = null;
        [SerializeField] private GenericEntryViewUGUI m_RequestEntryViewPrefab = null;

        List<GenericEntryViewUGUI> m_Requests = new List<GenericEntryViewUGUI>();

        private List<PlayerProfile> m_PlayerProfiles = new List<PlayerProfile>();

        public void Refresh(List<PlayerProfile> playerProfiles)
        {
            foreach (var entry in m_Requests)
            {
                Destroy(entry.gameObject);
            }

            m_Requests.Clear();

            foreach (var playerProfile in playerProfiles)
            {
                var entry = Instantiate(m_RequestEntryViewPrefab, m_ParentTransform);
                entry.Init(playerProfile.Name);
                entry.button1.onClick.AddListener(() => { onAccept?.Invoke(playerProfile.Id); });
                entry.button2.onClick.AddListener(() => { onDecline?.Invoke(playerProfile.Id); });
                m_Requests.Add(entry);
            }
        }

        public Action<string> onAccept { get; set; }
        public Action<string> onDecline { get; set; }
        public Action<string> onBlock { get; set; }

        public void BindList(List<PlayerProfile> listToBind)
        {
            m_PlayerProfiles = listToBind;
        }

        public void Show()
        {
            Refresh();
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Refresh()
        {
            Refresh(m_PlayerProfiles);
        }
    }
}