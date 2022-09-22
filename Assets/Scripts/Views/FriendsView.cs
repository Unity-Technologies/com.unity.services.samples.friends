using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityGamingServicesUsesCases.Relationships
{
    public class FriendsView : MonoBehaviour
    {
        public Action<string> OnFriendRemove = null;
        public Action<string> OnFriendBlock = null;
        
        [SerializeField]private RectTransform m_ParentTransform = null;
        [SerializeField]private GenericEntryView m_GenericEntryViewPrefab = null;

        private List<GenericEntryView> m_Friends = new List<GenericEntryView>();

        public void Refresh(List<PlayerProfile> playerProfiles)
        {
            foreach (var entry in m_Friends)
            {
                Destroy(entry.gameObject);
            }
            m_Friends.Clear();

            foreach (var playerProfile in playerProfiles)
            {
                var entry = Instantiate(m_GenericEntryViewPrefab, m_ParentTransform);
                entry.Init(playerProfile.Name);
                entry.button1.onClick.AddListener(() =>
                {
                    OnFriendRemove?.Invoke(playerProfile.Id);
                });
                entry.button2.onClick.AddListener(() =>
                {
                    OnFriendBlock?.Invoke(playerProfile.Id);
                });
                m_Friends.Add(entry);
            }
        }
    }
}