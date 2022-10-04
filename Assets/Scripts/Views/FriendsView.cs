using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Friends.Models;
using UnityEngine;
using UnityEngine.Serialization;

namespace UnityGamingServicesUsesCases.Relationships
{
    public class FriendsView : MonoBehaviour
    {
        public Action<string> OnFriendRemove = null;
        public Action<string> OnFriendBlock = null;
        
        [SerializeField]private RectTransform m_ParentTransform = null;
        [SerializeField]private FriendsEntryView m_FriendEntryViewPrefab = null;

        private List<FriendsEntryView> m_Friends = new ();

        public void Refresh(List<(string,string,string)> playerPresences)
        {
            foreach (var entry in m_Friends)
            {
                Destroy(entry.gameObject);
            }
            m_Friends.Clear();

            foreach (var playerPresence in playerPresences)
            {
                var entry = Instantiate(m_FriendEntryViewPrefab, m_ParentTransform);
                entry.Init(playerPresence.Item2,playerPresence.Item3);
                entry.button1.onClick.AddListener(() =>
                {
                    OnFriendRemove?.Invoke(playerPresence.Item1);
                });
                entry.button2.onClick.AddListener(() =>
                {
                    OnFriendBlock?.Invoke(playerPresence.Item1);
                });
                m_Friends.Add(entry);
            }
        }
    }
}