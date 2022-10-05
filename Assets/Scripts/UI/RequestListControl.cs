using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnityGamingServicesUsesCases.Relationships.UI
{
    public class RequestListControl
    {
        public Action<string> onAcceptUser;
        public Action<string> onDenyUser;
        public Action<string> onBlockUser;

        const string k_RequestListViewName = "friends-list";

        ListView m_RequestListView;

        /// <summary>
        /// Finds and binds the UI Elements with the controller
        /// </summary>
        /// <param name="viewParent">One of the parents of the friends-list (In RelationShipBarView.uxml)</param>
        /// <param name="requestEntryTemplate">The Friends Template (FriendListEntry.uxml) </param>
        /// <param name="boundRequestProfiles">The List of users we bind the listview to.</param>
        public RequestListControl(VisualElement viewParent, VisualTreeAsset requestEntryTemplate)
        {
            m_RequestListView = viewParent.Q<ListView>(k_RequestListViewName);

            m_RequestListView.makeItem = () =>
            {
                var newListEntry = requestEntryTemplate.Instantiate();
                var newListEntryLogic = new RequestEntryControl(newListEntry.contentContainer);
                newListEntry.userData = newListEntryLogic;
                return newListEntry;
            };
        }

        public void BindList(List<PlayerProfile> listToBind)
        {
            m_RequestListView.bindItem = (item, index) =>
            {
                var requestControl = item.userData as RequestEntryControl;
                var userProfile = listToBind[index];
                requestControl.playerEntryControl.SetName(userProfile.Name);

                requestControl.onAcceptPressed += () =>
                {
                    onAcceptUser?.Invoke(userProfile.Id);
                };
                requestControl.onDenyPressed += () =>
                {
                    onDenyUser?.Invoke(userProfile.Id);
                };
                requestControl.onBlockFriendPressed += () =>
                {
                    onBlockUser?.Invoke(userProfile.Id);
                };
            };

            m_RequestListView.itemsSource = listToBind;
            Refresh();
        }

        public void Show(bool show)
        {
            m_RequestListView.style.display = show ? DisplayStyle.Flex : DisplayStyle.None;
        }

        /// <summary>
        /// Force a refresh now!
        /// </summary>
        public void Refresh()
        {
            m_RequestListView.RefreshItems();
        }
    }
}