using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnityGamingServicesUsesCases.Relationships.UI
{
    public class RelationshipBarView
    {
        const string k_RelationshipsBarViewName = "relationship-bar-view";

        public Action onFriends;
        public Action onRequests;
        public Action onBlocks;
        public Action onAddFriend;

        public RelationshipBarView(VisualElement viewParent)
        {
            var relationshipsBarView = viewParent.Q(k_RelationshipsBarViewName);
            var friendsListButton = relationshipsBarView.Q<Button>("friends-button");
            var requestListButton = relationshipsBarView.Q<Button>("requests-button");
            var blockedListButton = relationshipsBarView.Q<Button>("blocked-button");
            var addFriendButton = relationshipsBarView.Q<Button>("add-friend-button");

            friendsListButton.RegisterCallback<ClickEvent>((_) =>
            {
                onFriends?.Invoke();
            });
            requestListButton.RegisterCallback<ClickEvent>((_) =>
            {
                onRequests?.Invoke();
            });
            blockedListButton.RegisterCallback<ClickEvent>((_) =>
            {
                onBlocks?.Invoke();
            });
            addFriendButton.RegisterCallback<ClickEvent>((_) =>
            {
                onAddFriend?.Invoke();
            });
        }
    }
}