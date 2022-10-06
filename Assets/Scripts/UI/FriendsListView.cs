using System;
using System.Collections.Generic;
using Unity.Services.Friends.Models;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnityGamingServicesUsesCases.Relationships.UI
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

        public void BindList(List<PlayerProfile> listToBind)
        {
            m_FriendListView.bindItem = (item, index) =>
            {
                var friendControl = item.userData as FriendEntryView;
                var userProfile = listToBind[index];
                friendControl.localPlayerView.SetName(userProfile.Name);
                friendControl.localPlayerView.SetActivity("TODO"); //TODO Get Actual Profile activity
                friendControl.localPlayerView.SetStatus(
                    PresenceAvailabilityOptions.ONLINE); // TODO Get Actual Profile Status

                friendControl.onRemoveFriend = () =>
                {
                    onRemoveFriend?.Invoke(userProfile.Id);
                };
                friendControl.onBlockFriend = () =>
                {
                    onBlockFriend?.Invoke(userProfile.Id);
                };
            };
            m_FriendListView.itemsSource = listToBind;
            Refresh();
        }

        public void Show(bool show)
        {
            m_FriendListView.style.display = show ? DisplayStyle.Flex : DisplayStyle.None;
        }

        /// <summary>
        /// Force a refresh now!
        /// </summary>
        public void Refresh()
        {
            m_FriendListView.RefreshItems();
        }
    }
}
