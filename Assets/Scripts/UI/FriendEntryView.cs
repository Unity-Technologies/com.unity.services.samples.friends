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
        VisualElement m_FriendEntryView;

        public FriendEntryView(VisualElement documentParent)
        {
            m_FriendEntryView = documentParent.Q(k_FriendEntryViewName);

            m_PlayerName = m_FriendEntryView.Q<Label>("player-name-label");
            m_PlayerStatusLabel = m_FriendEntryView.Q<Label>("player-status-label");
            m_PlayerStatusCircle = m_FriendEntryView.Q<VisualElement>("player-status-circle");
            m_PlayerActivity = m_FriendEntryView.Q<Label>("player-activity-label");
            var removeFriendButton = m_FriendEntryView.Q<Button>("remove-button");
            removeFriendButton.RegisterCallback<ClickEvent>(_ =>
            {
                onRemoveFriend?.Invoke();
            });
            var blockFriendButton = m_FriendEntryView.Q<Button>("block-button");
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
        public void Show()
        {
            m_FriendEntryView.style.display = DisplayStyle.Flex;
        }

        public void Hide()
        {
            m_FriendEntryView.style.display = DisplayStyle.None;
        }

    }
}
