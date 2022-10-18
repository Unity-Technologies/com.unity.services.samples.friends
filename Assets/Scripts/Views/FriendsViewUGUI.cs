using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityGamingServicesUsesCases.Relationships.UGUI
{
    public class FriendsViewUGUI : MonoBehaviour, IFriendsListView
    {
        [SerializeField] private RectTransform m_ParentTransform = null;
        [SerializeField] private FriendsEntryViewUGUI m_FriendEntryViewPrefab = null;

        List<FriendsEntryViewUGUI> m_Friends = new List<FriendsEntryViewUGUI>();

        private List<FriendsEntryData> m_FriendsEntryDatas = new List<FriendsEntryData>();

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
                entry.Init(friendsEntryData.Name, friendsEntryData.Availability.ToString(), friendsEntryData.Activity);
                entry.button1.onClick.AddListener(() => { onRemove?.Invoke(friendsEntryData.Id); });
                entry.button2.onClick.AddListener(() => { onBlock?.Invoke(friendsEntryData.Id); });
                m_Friends.Add(entry);
            }
        }

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
            Refresh(m_FriendsEntryDatas);
        }
    }
}