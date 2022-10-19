using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityGamingServicesUsesCases.Relationships.UGUI
{
    public class RequestsViewUGUI : MonoBehaviour, IRequestListView
    {
        [SerializeField] RectTransform m_ParentTransform = null;
        [SerializeField] RequestEntryViewUGUI m_RequestEntryViewPrefab = null;

        List<RequestEntryViewUGUI> m_RequestEntries = new List<RequestEntryViewUGUI>();
        List<PlayerProfile> m_PlayerProfiles = new List<PlayerProfile>();
        public Action<string> onAccept { get; set; }
        public Action<string> onDecline { get; set; }
        public Action<string> onBlock { get; set; }

        public void BindList(List<PlayerProfile> playerProfiles)
        {
            m_PlayerProfiles = playerProfiles;
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
            m_RequestEntries.ForEach(entry => Destroy(entry.gameObject));
            m_RequestEntries.Clear();

            foreach (var playerProfile in m_PlayerProfiles)
            {
                var entry = Instantiate(m_RequestEntryViewPrefab, m_ParentTransform);
                entry.Init(playerProfile.Name);
                entry.acceptButton.onClick.AddListener(() => { onAccept?.Invoke(playerProfile.Id); });
                entry.declineButton.onClick.AddListener(() => { onDecline?.Invoke(playerProfile.Id); });
                entry.blockButton.onClick.AddListener(() => { onBlock?.Invoke(playerProfile.Id); });
                m_RequestEntries.Add(entry);
            }
        }
    }
}