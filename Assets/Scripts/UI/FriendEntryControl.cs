using System;
using UnityEngine.UIElements;

namespace UnityGamingServicesUsesCases.Relationships.UI
{
    public class FriendEntryControl
    {
        const string k_FriendEntryRootName = "friend-entry-view";
        VisualElement m_FriendEntryRoot;

        public Action onRemoveFriendPressed;
        public Action onBlockFriendPressed;

        public PlayerEntryControl playerEntryControl;
        Button m_RemoveFriendButton;
        Button m_BlockFriendButton;

        public FriendEntryControl(VisualElement documentParent)
        {
            m_FriendEntryRoot = documentParent.Q(k_FriendEntryRootName);
            playerEntryControl = new PlayerEntryControl(m_FriendEntryRoot);
            m_RemoveFriendButton = m_FriendEntryRoot.Q<Button>("remove-button");
            m_RemoveFriendButton.RegisterCallback<ClickEvent>(_ =>
            {
                onRemoveFriendPressed?.Invoke();
            });
            m_BlockFriendButton = m_FriendEntryRoot.Q<Button>("block-button");
            m_BlockFriendButton.RegisterCallback<ClickEvent>(_ =>
            {
                onBlockFriendPressed?.Invoke();
            });
        }
    }
}