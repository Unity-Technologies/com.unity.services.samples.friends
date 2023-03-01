using System;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.Services.Samples.Friends.UGUI
{
    public class BlocksViewUGUI : ListViewUGUI, IBlockedListView
    {
        [SerializeField] RectTransform m_ParentTransform = null;
        [SerializeField] BlockEntryViewUGUI m_BlockEntryViewPrefab = null;

        List<BlockEntryViewUGUI> m_BlockEntries = new List<BlockEntryViewUGUI>();
        List<IUGSPlayer> m_PlayerProfiles = new List<IUGSPlayer>();
        public Action<string> onUnblock { get; set; }

        public void BindList(List<IUGSPlayer> playerProfiles)
        {
            m_PlayerProfiles = playerProfiles;
        }

        public override void Refresh()
        {
            m_BlockEntries.ForEach(entry => Destroy(entry.gameObject));
            m_BlockEntries.Clear();

            foreach (var playerProfile in m_PlayerProfiles)
            {
                var entry = Instantiate(m_BlockEntryViewPrefab, m_ParentTransform);
                entry.Init(playerProfile.Name);
                entry.unblockButton.onClick.AddListener(() =>
                {
                    entry.gameObject.SetActive(false);
                    onUnblock?.Invoke(playerProfile.Id);
                });
                m_BlockEntries.Add(entry);
            }
        }
    }
}
