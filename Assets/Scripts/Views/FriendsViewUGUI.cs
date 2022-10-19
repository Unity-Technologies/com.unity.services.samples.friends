using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityGamingServicesUsesCases.Relationships.UGUI
{
    public class FriendsViewUGUI : MonoBehaviour, IFriendsListView
    {
        [SerializeField] RectTransform m_ParentTransform = null;
        [SerializeField] FriendEntryViewUGUI m_FriendEntryViewPrefab = null;

        List<FriendEntryViewUGUI> m_FriendEntries = new List<FriendEntryViewUGUI>();
        List<FriendsEntryData> m_FriendsEntryDatas = new List<FriendsEntryData>();

        public Action<string> onRemove { get; set; }
        public Action<string> onBlock { get; set; }

        public void BindList(List<FriendsEntryData> listToBind)
        {
            m_FriendsEntryDatas = listToBind;
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
            m_FriendEntries.ForEach(entry => Destroy(entry.gameObject));
            m_FriendEntries.Clear();

            foreach (var friendsEntryData in m_FriendsEntryDatas)
            {
                var entry = Instantiate(m_FriendEntryViewPrefab, m_ParentTransform);
                entry.Init(friendsEntryData.Name, friendsEntryData.Availability.ToString(), friendsEntryData.Activity);
                entry.removeFriendButton.onClick.AddListener(() => { onRemove?.Invoke(friendsEntryData.Id); });
                entry.blockFriendButton.onClick.AddListener(() => { onBlock?.Invoke(friendsEntryData.Id); });
                m_FriendEntries.Add(entry);
            }
        }
    }
}