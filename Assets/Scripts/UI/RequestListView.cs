using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnityGamingServicesUsesCases.Relationships.UI
{
    public class RequestListView
    {
        public Action<string> onAcceptUser;
        public Action<string> onDeclineUser;
        public Action<string> onBlockUser;

        const string k_RequestListViewName = "request-list";

        ListView m_RequestListView;

        /// <summary>
        /// Finds and binds the UI Elements with the controller
        /// </summary>
        /// <param name="viewParent">One of the parents of the friends-list (In RelationShipBarView.uxml)</param>
        /// <param name="requestEntryTemplate">The Friends Template (FriendListEntry.uxml) </param>
        /// <param name="boundRequestProfiles">The List of users we bind the listview to.</param>
        public RequestListView(VisualElement viewParent, VisualTreeAsset requestEntryTemplate)
        {
            m_RequestListView = viewParent.Q<ListView>(k_RequestListViewName);

            m_RequestListView.makeItem = () =>
            {
                var newListEntry = requestEntryTemplate.Instantiate();
                var newListEntryLogic = new RequestEntryView(newListEntry.contentContainer);
                newListEntry.userData = newListEntryLogic;
                return newListEntry;
            };
        }

        public void BindList(List<PlayerProfile> listToBind)
        {
            m_RequestListView.bindItem = (item, index) =>
            {
                var requestControl = item.userData as RequestEntryView;
                requestControl.Show();
                var userProfile = listToBind[index];
                requestControl.Refresh(userProfile.Name);
                requestControl.onAccept = () =>
                {
                    onAcceptUser?.Invoke(userProfile.Id);
                };

                requestControl.onDecline = () =>
                {
                    onDeclineUser?.Invoke(userProfile.Id);
                };

                requestControl.onBlockFriend = () =>
                {
                    onBlockUser?.Invoke(userProfile.Id);
                };
            };

            m_RequestListView.itemsSource = listToBind;
            Refresh();
        }

        public void Show()
        {
            m_RequestListView.style.display = DisplayStyle.Flex;
        }

        public void Hide()
        {
            m_RequestListView.style.display = DisplayStyle.None;
        }

        /// <summary>
        /// Force a refresh now!
        /// </summary>
        public void Refresh()
        {
             #if UNITY_2020
            m_RequestListView.Refresh();
            #elif UNITY_2021
            m_RequestListView.RefreshItems();
            #endif
        }
    }
}
