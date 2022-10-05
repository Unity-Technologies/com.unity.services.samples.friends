using System;
using UnityEngine.UIElements;

namespace UnityGamingServicesUsesCases.Relationships.UI
{
    public class FriendEntryControl
    {
        const string k_FriendEntryViewName = "friend-entry-view";
        VisualElement m_FriendEntryView;

        public Action onRemoveFriendPressed;
        public Action onBlockFriendPressed;

        public PlayerEntryControl playerEntryControl;
        Button m_RemoveFriendButton;
        Button m_BlockFriendButton;

        public FriendEntryControl(VisualElement documentParent)
        {
            m_FriendEntryView = documentParent.Q(k_FriendEntryViewName);
            playerEntryControl = new PlayerEntryControl(m_FriendEntryView);
            m_RemoveFriendButton = m_FriendEntryView.Q<Button>("remove-button");
            m_RemoveFriendButton.RegisterCallback<ClickEvent>(_ =>
            {
                onRemoveFriendPressed?.Invoke();
            });
            m_BlockFriendButton = m_FriendEntryView.Q<Button>("block-button");
            m_BlockFriendButton.RegisterCallback<ClickEvent>(_ =>
            {
                onBlockFriendPressed?.Invoke();
            });
        }
    }
}