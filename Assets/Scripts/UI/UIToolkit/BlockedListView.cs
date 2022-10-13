using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace UnityGamingServicesUsesCases.Relationships.UIToolkit
{
    public class BlockedListView : IBlockedListView
    {
        const string k_BlockedListViewName = "blocked-list";

        public Action<string> onUnBlock { get; set; }

        ListView m_BlockedListView;

        /// <summary>
        /// Finds and binds the UI Elements with the controller
        /// </summary>
        /// <param name="viewParent">One of the parents of the friends-list (In RelationShipBarView.uxml)</param>
        /// <param name="blockFriendTemplate">The Friends Template (FriendListEntry.uxml) </param>
        /// <param name="boundBlockedProfiles">The List of users we bind the listview to.</param>
        public BlockedListView(VisualElement viewParent, VisualTreeAsset blockFriendTemplate)
        {
            m_BlockedListView = viewParent.Q<ListView>(k_BlockedListViewName);

            m_BlockedListView.makeItem = () =>
            {
                var newListEntry = blockFriendTemplate.Instantiate();
                var newListEntryLogic = new BlockedEntryView(newListEntry.contentContainer);
                newListEntry.userData = newListEntryLogic;
                return newListEntry;
            };
        }

        public void BindList(List<PlayerProfile> blockedListToBind)
        {
            m_BlockedListView.bindItem = (item, index) =>
            {
                var blockedEntryControl = item.userData as BlockedEntryView;
                blockedEntryControl.Show();
                var userProfile = blockedListToBind[index];
                blockedEntryControl.Refresh(userProfile.Name);
                blockedEntryControl.onUnBlock = () =>
                {
                    onUnBlock?.Invoke(userProfile.Id);
                };
            };

            m_BlockedListView.itemsSource = blockedListToBind;
            Refresh();
        }

        public void Show()
        {
            m_BlockedListView.style.display = DisplayStyle.Flex;
        }

        public void Hide()
        {
            m_BlockedListView.style.display = DisplayStyle.None;
        }

        /// <summary>
        /// Force a refresh now!
        /// </summary>
        public void Refresh()
        {
#if UNITY_2020
            m_BlockedListView.Refresh();
#elif UNITY_2021
            m_BlockedListView.RefreshItems();
#endif
        }
    }
}