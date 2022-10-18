using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityGamingServicesUsesCases.Relationships.UGUI
{
    public class BlocksViewUGUI : MonoBehaviour, IBlockedListView
    {
        public Action<string> OnFriendUnblock = null;

        [SerializeField]private RectTransform m_ParentTransform = null;
        [SerializeField]private GenericEntryViewUGUI m_BlockedEntryViewPrefab = null;

        List<GenericEntryViewUGUI> m_Blocked = new List<GenericEntryViewUGUI>();
        private List<PlayerProfile> m_PlayerProfiles = new List<PlayerProfile>();
        public void Refresh(List<PlayerProfile> playerProfiles)
        {
            foreach (var entry in m_Blocked)
            {
                Destroy(entry.gameObject);
            }
            m_Blocked.Clear();

            foreach (var playerProfile in playerProfiles)
            {
                var entry = Instantiate(m_BlockedEntryViewPrefab, m_ParentTransform);
                entry.Init(playerProfile.Name);
                entry.button1.onClick.AddListener(() =>
                {
                    OnFriendUnblock?.Invoke(playerProfile.Id);
                });
                m_Blocked.Add(entry);
                entry.button2.gameObject.SetActive(false);
            }
        }

        public Action<string> onUnBlock { get; set; }
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
