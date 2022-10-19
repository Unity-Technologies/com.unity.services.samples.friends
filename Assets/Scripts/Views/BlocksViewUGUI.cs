using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityGamingServicesUsesCases.Relationships.UGUI
{
    public class BlocksViewUGUI : MonoBehaviour, IBlockedListView
    {
        [SerializeField] RectTransform m_ParentTransform = null;
        [SerializeField] BlockEntryViewUGUI m_BlockEntryViewPrefab = null;

        List<BlockEntryViewUGUI> m_BlockEntries = new List<BlockEntryViewUGUI>();
        List<PlayerProfile> m_PlayerProfiles = new List<PlayerProfile>();
        public Action<string> onUnblock { get; set; }

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
            m_BlockEntries.ForEach(entry => Destroy(entry.gameObject));
            m_BlockEntries.Clear();
            
            foreach (var playerProfile in m_PlayerProfiles)
            {
                var entry = Instantiate(m_BlockEntryViewPrefab, m_ParentTransform);
                entry.Init(playerProfile.Name);
                entry.unblockButton.onClick.AddListener(() => { onUnblock?.Invoke(playerProfile.Id); });
                m_BlockEntries.Add(entry);
            }
        }
    }
}