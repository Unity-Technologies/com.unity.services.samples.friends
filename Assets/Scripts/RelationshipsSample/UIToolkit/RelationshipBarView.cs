using System;
using UnityEngine.UIElements;

namespace UnityGamingServicesUsesCases.Relationships.UIToolkit
{
    public class RelationshipBarView : IRelationshipBarView
    {
        const string k_RelationshipsBarViewName = "relationship-bar-view";

        public Action onShowFriends { get; set; }
        public Action onShowRequests { get; set; }
        public Action onShowBlocks { get; set; }
        public Action onShowRequestFriend { get; set; }

        public RelationshipBarView(VisualElement viewParent)
        {
            var relationshipsBarView = viewParent.Q(k_RelationshipsBarViewName);
            var friendsListButton = relationshipsBarView.Q<Button>("friends-button");
            var requestListButton = relationshipsBarView.Q<Button>("requests-button");
            var blockedListButton = relationshipsBarView.Q<Button>("blocked-button");
            var addFriendButton = relationshipsBarView.Q<Button>("add-friend-button");

            friendsListButton.RegisterCallback<ClickEvent>((_) =>
            {
                onShowFriends?.Invoke();
            });
            requestListButton.RegisterCallback<ClickEvent>((_) =>
            {
                onShowRequests?.Invoke();
            });
            blockedListButton.RegisterCallback<ClickEvent>((_) =>
            {
                onShowBlocks?.Invoke();
            });
            addFriendButton.RegisterCallback<ClickEvent>((_) =>
            {
                onShowRequestFriend?.Invoke();
            });
        }
    }
}
