using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityGamingServicesUsesCases.Relationships
{
    public class FriendsView : MonoBehaviour
    {
        public Action<string> OnFriendRemove = null;
        public Action<string> OnFriendBlock = null;

        [SerializeField]private RectTransform m_ParentTransform = null;
        [SerializeField]private FriendsEntryView m_FriendEntryViewPrefab = null;

        List<FriendsEntryView> m_Friends = new List<FriendsEntryView>();

        public void Refresh(List<FriendsEntryData> friendsEntryDatas)
        {
            foreach (var entry in m_Friends)
            {
                Destroy(entry.gameObject);
            }
            m_Friends.Clear();

            foreach (var friendsEntryData in friendsEntryDatas)
            {
                var entry = Instantiate(m_FriendEntryViewPrefab, m_ParentTransform);
                entry.Init(friendsEntryData.Name,friendsEntryData.Availability.ToString(), friendsEntryData.Activity);
                entry.button1.onClick.AddListener(() =>
                {
                    OnFriendRemove?.Invoke(friendsEntryData.Id);
                });
                entry.button2.onClick.AddListener(() =>
                {
                    OnFriendBlock?.Invoke(friendsEntryData.Id);
                });
                m_Friends.Add(entry);
            }
        }
    }
}
