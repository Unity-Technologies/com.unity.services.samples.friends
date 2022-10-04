using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Services.Friends.Models;
using Unity.VisualScripting;
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

        Button m_FriendsListButton;
        Button m_RequestListButton;
        Button m_BlockedListButton;
        Button m_AddFriendButton;
        VisualElement m_ListBackground;
        ListView m_RelationshipListView;

        VisualTreeAsset m_FriendEntryTemplate;
        VisualTreeAsset m_RequestEntryTemplate;
        VisualTreeAsset m_BlockEntryTemplate;

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
                    m_RelationshipListView.makeItem = CreateFriendListEntry;
                    
                    m_RelationshipListView.bindItem = (item, index) =>
                    {
                        (item.userData as FriendEntryControl)?.playerEntryControl.SetPlayer(m_FriendsList[index]);
                    };
                    m_RelationshipListView.fixedItemHeight = 70;
                    m_RelationshipListView.itemsSource = m_FriendsList;
                    Debug.Log($"Switched to show Data: {m_FriendsList.Count} UI:{m_RelationshipListView.Children().Count()} List");
                    break;
                case ShowListState.Requests:

                    break;
                case ShowListState.Blocked:

                    break;
                case ShowListState.None:

                    m_RelationshipListView.itemsSource = new List<PlayerProfile>();
                    break;

            }
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


        VisualElement CreateFriendListEntry()
        {
            var newListEntry = m_FriendEntryTemplate.Instantiate();
            var newListEntryLogic = new FriendEntryControl(newListEntry.contentContainer);
            newListEntry.userData = newListEntryLogic;
            return newListEntry;

        }

        VisualElement CreateRequestListEntry()
        {
            var newListEntry = m_RequestEntryTemplate.Instantiate();
            var newListEntryLogic = new FriendEntryControl(newListEntry);
            newListEntry.userData = newListEntryLogic;
            return newListEntry;

        }

        VisualElement CreateBlockListEntry()
        {
            var newListEntry = m_BlockEntryTemplate.Instantiate();
            var newListEntryLogic = new FriendEntryControl(newListEntry);
            newListEntry.userData = newListEntryLogic;
            return newListEntry;

        }

    }
}
