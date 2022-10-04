using System;
using System.Collections.Generic;
using Unity.Services.Friends.Models;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnityGamingServicesUsesCases.Relationships.UI
{
    public enum ShowListState
    {
        Friends = 0,
        Requests = 1,
        Blocked = 2,
        None = 3
    }

    public class RelationshipBarControl : UIBaseControl
    {
        public override string ViewRootName => "relationship-bar-view";

        public Action onFriendsListPressed;
        public Action onRequestListPressed;
        public Action onBlockedListPressed;
        public Action onAddFriendPressed;

        public Action<string> onAcceptFriend;
        public Action<string> onDenyFriend;
        public Action<string> onRemoveFriend;
        public Action<string> onBlockUser;
        public Action<string> onUnblockuser;

        Button m_FriendsListButton;
        Button m_RequestListButton;
        Button m_BlockedListButton;
        Button m_AddFriendButton;
        VisualElement m_ListBackground;
        ListView m_RelationshipListView;

        VisualTreeAsset m_FriendEntryTemplate;
        VisualTreeAsset m_RequestEntryTemplate;
        VisualTreeAsset m_BlockEntryTemplate;
        ShowListState m_PreviousState = ShowListState.None;

        List<PlayerProfile> m_FriendsList = new List<PlayerProfile>();
        List<PlayerProfile> m_RequestList = new List<PlayerProfile>();
        List<PlayerProfile> m_BlockedList = new List<PlayerProfile>();

        public RelationshipBarControl(VisualElement documentParent,
            VisualTreeAsset friendEntryTemplate,
            VisualTreeAsset requestEntryTemplate,
            VisualTreeAsset blockEntryTemplate)
            : base(documentParent)
        {
            m_FriendEntryTemplate = friendEntryTemplate;
            m_RequestEntryTemplate = requestEntryTemplate;
            m_BlockEntryTemplate = blockEntryTemplate;
        }

        public void SetFriendsListReference(List<PlayerProfile> friendProfiles)
        {
            m_FriendsList = friendProfiles;
        }

        public void SetRequestListReference(List<PlayerProfile> requestProfiles)
        {
            m_RequestList = requestProfiles;
        }

        public void SetBlockedListReference(List<PlayerProfile> blockedProfiles)
        {
            m_BlockedList = blockedProfiles;
        }

        protected override void SetVisualElements()
        {
            m_FriendsListButton = GetElementByName<Button>("friends-button");
            m_RequestListButton = GetElementByName<Button>("requests-button");
            m_BlockedListButton = GetElementByName<Button>("blocked-button");
            m_AddFriendButton = GetElementByName<Button>("add-friend-button");
            m_RelationshipListView = GetElementByName<ListView>("relationship-list");
        }

        protected override void RegisterButtonCallbacks()
        {
            m_FriendsListButton.RegisterCallback<ClickEvent>((_) =>
            {
                onFriendsListPressed?.Invoke();
                SwitchView(ShowListState.Friends);
            });
            m_RequestListButton.RegisterCallback<ClickEvent>((_) =>
            {
                onRequestListPressed?.Invoke();
                SwitchView(ShowListState.Requests);
            });
            m_BlockedListButton.RegisterCallback<ClickEvent>((_) =>
            {
                onBlockedListPressed?.Invoke();
                SwitchView(ShowListState.Blocked);
            });
            m_AddFriendButton.RegisterCallback<ClickEvent>((_) => { onAddFriendPressed?.Invoke(); });
        }

        /// <summary>
        /// We use a single list view that re-populates with elements from
        /// the friends, request and blocked lists respectively.
        /// Switching the state clears the list and changes the input data source and templates.
        /// </summary>
        /// <param name="listToView"></param>
        void SwitchView(ShowListState listToView)
        {
            m_RelationshipListView.Clear();
            switch (listToView)
            {
                case ShowListState.Friends:
                    SetupFriendsList();
                    break;
                case ShowListState.Requests:
                    SetupRequestList();
                    break;
                case ShowListState.Blocked:
                    SetupBlockList();
                    break;
                case ShowListState.None:
                    m_RelationshipListView.itemsSource = new List<PlayerProfile>();
                    break;
            }

            m_RelationshipListView.RefreshItems();
        }

        void SetupFriendsList()
        {
            m_RelationshipListView.makeItem = () =>
            {
                var newListEntry = m_FriendEntryTemplate.Instantiate();
                var newListEntryLogic = new FriendEntryControl(newListEntry.contentContainer);
                newListEntry.userData = newListEntryLogic;
                return newListEntry;
            };

            m_RelationshipListView.bindItem = (item, index) =>
            {
                var friendControl = item.userData as FriendEntryControl;
                var userProfile = m_FriendsList[index];
                friendControl.playerEntryControl.SetName(userProfile.Name);
                friendControl.playerEntryControl.SetActivity("TODO"); //TODO Get Actual Profile activity
                friendControl.playerEntryControl.SetStatus(
                    PresenceAvailabilityOptions.ONLINE); // TODO Get Actual Profile Status

                friendControl.onRemoveFriendPressed += () =>
                {
                    onRemoveFriend.Invoke(userProfile.Id);
                };
                friendControl.onBlockFriendPressed = () =>
                {
                    onBlockUser.Invoke(userProfile.Id);
                };
            };
            m_RelationshipListView.itemsSource = m_FriendsList;
        }

        void SetupRequestList()
        {
            m_RelationshipListView.makeItem = () =>
            {
                var newListEntry = m_RequestEntryTemplate.Instantiate();
                var newListEntryLogic = new RequestEntryControl(newListEntry.contentContainer);
                newListEntry.userData = newListEntryLogic;
                return newListEntry;
            };

            m_RelationshipListView.bindItem = (item, index) =>
            {
                var requestControl = item.userData as RequestEntryControl;
                var userProfile = m_RequestList[index];
                requestControl.playerEntryControl.SetName(userProfile.Name);

                requestControl.onAcceptPressed += () =>
                {
                    onAcceptFriend?.Invoke(userProfile.Id);
                };
                requestControl.onDenyPressed += () =>
                {
                    onDenyFriend?.Invoke(userProfile.Id);
                };
                requestControl.onBlockFriendPressed += () =>
                {
                    onBlockUser?.Invoke(userProfile.Id);
                };
            };

            m_RelationshipListView.itemsSource = m_RequestList;
        }

        void SetupBlockList()
        {
            m_RelationshipListView.makeItem = () =>
            {
                var newListEntry = m_BlockEntryTemplate.Instantiate();
                var newListEntryLogic = new BlockedEntryControl(newListEntry.contentContainer);
                newListEntry.userData = newListEntryLogic;
                return newListEntry;
            };

            m_RelationshipListView.bindItem = (item, index) =>
            {
                var blockedEntryControl = item.userData as BlockedEntryControl;
                var userProfile = m_BlockedList[index];
                blockedEntryControl.playerEntryControl.SetName(userProfile.Name);

                blockedEntryControl.onUnblockPressed += () =>
                {
                    onUnblockuser?.Invoke(userProfile.Id);
                };
            };
            m_RelationshipListView.itemsSource = m_BlockedList;
        }
    }
}