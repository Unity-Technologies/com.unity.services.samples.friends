using System;
using UnityEngine.UIElements;

namespace UnityGamingServicesUsesCases.Relationships.UI
{
    public class FriendEntryControl
    {
        const string k_FriendEntryViewName = "friend-entry-view";

        public Action onRemoveFriend;
        public Action onBlockFriend;

        public PlayerEntryControl playerEntryControl;
        Button m_RemoveFriendButton;
        Button m_BlockFriendButton;

        public FriendEntryControl(VisualElement documentParent)
        {
            var friendEntryView = documentParent.Q(k_FriendEntryViewName);
            playerEntryControl = new PlayerEntryControl(friendEntryView);
            m_RemoveFriendButton = friendEntryView.Q<Button>("remove-button");
            m_RemoveFriendButton.RegisterCallback<ClickEvent>(_ =>
            {
                onRemoveFriend?.Invoke();
            });
            m_BlockFriendButton = friendEntryView.Q<Button>("block-button");
            m_BlockFriendButton.RegisterCallback<ClickEvent>(_ =>
            {
                onBlockFriend?.Invoke();
            });
        }
    }
}
