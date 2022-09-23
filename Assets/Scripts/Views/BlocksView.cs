using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityGamingServicesUsesCases.Relationships
{
    public class BlocksView : MonoBehaviour
    {
        public Action<string> OnFriendUnblock = null;

        [SerializeField]private RectTransform m_ParentTransform = null;
        [SerializeField]private GenericEntryView m_BlockedEntryViewPrefab = null;

        private List<GenericEntryView> m_Blocked = new ();

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
    }
}