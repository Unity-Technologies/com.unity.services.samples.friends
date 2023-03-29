using System;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.Services.Samples.Friends.UGUI
{
    public class FriendsViewUGUI : ListViewUGUI, IFriendsListView
    {
        [SerializeField] RectTransform m_ParentTransform = null;
        [SerializeField] FriendEntryViewUGUI m_FriendEntryViewPrefab = null;

        List<FriendEntryViewUGUI> m_FriendEntries = new List<FriendEntryViewUGUI>();
        List<FriendsEntryData> m_FriendsEntryDatas = new List<FriendsEntryData>();

        public Action<string> onRemove { get; set; }
        public Action<string> onBlock { get; set; }

#if LOBBY_SDK_AVAILABLE
        string m_LobbyCode = "";
        public Action<string> onInviteToParty { get; set; }
        public Action<string> onJoinFriendParty { get; set; }

        void Start()
        {
            LobbyEvents.OnLobbyJoined += OnLobbyJoined;
            LobbyEvents.OnLobbyLeft += OnLobbyLeft;
        }

        void OnLobbyJoined(string lobbyCode)
        {
            m_LobbyCode = lobbyCode;
        }

        void OnLobbyLeft()
        {
            m_LobbyCode = "";
        }

        void OnDestroy()
        {
            LobbyEvents.OnLobbyJoined -= OnLobbyJoined;
            LobbyEvents.OnLobbyLeft -= OnLobbyLeft;
        }
#endif

        public void BindList(List<FriendsEntryData> friendEntryDatas)
        {
            m_FriendsEntryDatas = friendEntryDatas;
        }

        public override void Refresh()
        {
            m_FriendEntries.ForEach(entry => Destroy(entry.gameObject));
            m_FriendEntries.Clear();

            foreach (var friendsEntryData in m_FriendsEntryDatas)
            {
                var entry = Instantiate(m_FriendEntryViewPrefab, m_ParentTransform);
                entry.Init(friendsEntryData.Name, friendsEntryData.Availability, friendsEntryData.Activity);
                entry.removeFriendButton.onClick.AddListener(() =>
                {
                    onRemove?.Invoke(friendsEntryData.Id);
                    entry.gameObject.SetActive(false);
                });
                entry.blockFriendButton.onClick.AddListener(() =>
                {
                    onBlock?.Invoke(friendsEntryData.Id);
                    entry.gameObject.SetActive(false);
                });

#if LOBBY_SDK_AVAILABLE

                entry.UpdateFriendPartyState(m_LobbyCode, friendsEntryData.Activity);
                entry.inviteFriendButton.onClick.AddListener(() =>
                {
                    onInviteToParty?.Invoke(friendsEntryData.Id);
                });
                entry.joinFriendButton.onClick.AddListener(() =>
                {
                    onJoinFriendParty?.Invoke(friendsEntryData.Activity.m_ActivityData);
                });
#endif
                m_FriendEntries.Add(entry);
            }
        }
    }
}