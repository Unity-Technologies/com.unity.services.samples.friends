using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnityGamingServicesUsesCases.Relationships.UIToolkit
{
    public class FriendsListView
    {
        public Action<string> onRemoveFriend;
        public Action<string> onBlockFriend;

        const string k_FriendsListViewName = "friends-list";

        ListView m_FriendListView;

        /// <summary>
        /// Finds and binds the UI Elements with the controller
        /// </summary>
        /// <param name="viewParent">One of the parents of the friends-list (In RelationShipBarView.uxml)</param>
        /// <param name="friendEntryTemplate">The Friends Template (FriendListEntry.uxml) </param>
        /// <param name="boundFriendProfiles">The List of users we bind the listview to.</param>
        public FriendsListView(VisualElement viewParent, VisualTreeAsset friendEntryTemplate)
        {
            m_FriendListView = viewParent.Q<ListView>(k_FriendsListViewName);

            m_FriendListView.makeItem = () =>
            {
                var newListEntry = friendEntryTemplate.Instantiate();
                var newListEntryLogic = new FriendEntryView(newListEntry.contentContainer);
                newListEntry.userData = newListEntryLogic;
                return newListEntry;
            };
        }

        public void BindList(List<FriendsEntryData> listToBind)
        {
            m_FriendListView.bindItem = (item, index) =>
            {
                var friendControl = item.userData as FriendEntryView;
                friendControl.Show();
                var friendData = listToBind[index];
                friendControl.Refresh(friendData.Name, friendData.Activity, friendData.Availability);
                friendControl.onRemoveFriend = () =>
                {
                    onRemoveFriend?.Invoke(friendData.Id);
                    friendControl.Hide();
                };

                friendControl.onBlockFriend = () =>
                {
                    onBlockFriend?.Invoke(friendData.Id);
                    friendControl.Hide();
                };
            };
            m_FriendListView.itemsSource = listToBind;
            Refresh();
        }

        public void Show()
        {
            m_FriendListView.style.display = DisplayStyle.Flex;
            Refresh();
        }

        public void Hide()
        {
            m_FriendListView.style.display = DisplayStyle.None;
        }

        /// <summary>
        /// Force a refresh now!
        /// </summary>
        public void Refresh()
        {
            #if UNITY_2020
            m_FriendListView.Refresh();
            #elif UNITY_2021
            m_FriendListView.RefreshItems();
            #endif
        }
    }
}
