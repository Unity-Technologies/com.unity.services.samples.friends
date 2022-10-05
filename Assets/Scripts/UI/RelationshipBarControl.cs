using System;
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

    public class RelationshipBarControl
    {
        const string k_RelationshipsBarViewName = "relationship-bar-view";

        public Action<ShowListState> onListButtonClicked;
        public Action onAddFriendPressed;

        public RelationshipBarControl(VisualElement viewParent)
        {
            var relationshipsBarView = viewParent.Q(k_RelationshipsBarViewName);
            var friendsListButton = relationshipsBarView.Q<Button>("friends-button");
            var requestListButton = relationshipsBarView.Q<Button>("requests-button");
            var blockedListButton = relationshipsBarView.Q<Button>("blocked-button");
            var addFriendButton = relationshipsBarView.Q<Button>("add-friend-button");

            friendsListButton.RegisterCallback<ClickEvent>((_) =>
            {
                onListButtonClicked?.Invoke(ShowListState.Friends);
            });
            requestListButton.RegisterCallback<ClickEvent>((_) =>
            {
                onListButtonClicked?.Invoke(ShowListState.Requests);
            });
            blockedListButton.RegisterCallback<ClickEvent>((_) =>
            {
                onListButtonClicked?.Invoke(ShowListState.Blocked);
            });
            addFriendButton.RegisterCallback<ClickEvent>((_) =>
            {
                onAddFriendPressed?.Invoke();
            });
        }
    }
}