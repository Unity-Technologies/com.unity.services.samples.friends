using System;
using Unity.Services.Friends.Models;
using UnityEngine.UIElements;

namespace UnityGamingServicesUsesCases.Relationships.UI
{
    public class FriendEntryView
    {
        const string k_FriendEntryViewName = "friend-entry-view";

        public Action onRemoveFriend;
        public Action onBlockFriend;
        Label m_PlayerName;
        Label m_PlayerActivity;
        Label m_PlayerStatusLabel;
        VisualElement m_PlayerStatusCircle;

        public FriendEntryView(VisualElement documentParent)
        {
            var friendEntryView = documentParent.Q(k_FriendEntryViewName);

            m_PlayerName = friendEntryView.Q<Label>("player-name-label");
            m_PlayerStatusLabel = friendEntryView.Q<Label>("player-status-label");
            m_PlayerStatusCircle = friendEntryView.Q<VisualElement>("player-status-circle");
            m_PlayerActivity = friendEntryView.Q<Label>("player-activity-label");
            var removeFriendButton = friendEntryView.Q<Button>("remove-button");
            removeFriendButton.RegisterCallback<ClickEvent>(_ =>
            {
                onRemoveFriend?.Invoke();
            });
            var blockFriendButton = friendEntryView.Q<Button>("block-button");
            blockFriendButton.RegisterCallback<ClickEvent>(_ =>
            {
                onBlockFriend?.Invoke();
            });
        }

        public void Refresh(string name, string activity, PresenceAvailabilityOptions presenceStatus)
        {
            m_PlayerName.text = name;
            m_PlayerActivity.text = activity;
            m_PlayerStatusLabel.text = presenceStatus.ToString();

            var presenceColor = UIUtils.GetPresenceColor(presenceStatus);
            m_PlayerStatusLabel.style.color = presenceColor;
            m_PlayerStatusCircle.style.backgroundColor = presenceColor;
        }
    }
}