using System;

using UnityEngine.UIElements;

namespace UnityGamingServicesUsesCases.Relationships.UI
{
    public class FriendEntryControl : UIBaseControl
    {
        public override string ViewRootName => "friend-entry-view";

        public Action<string> onRemoveFriendPressed;
        public Action<string> onBlockFriendPressed;

        public PlayerEntryControl playerEntryControl;
        Button m_RemoveFriendButton;
        Button m_BlockFriendButton;

        public FriendEntryControl(VisualElement documentParent) : base(documentParent) {}
        protected override void SetVisualElements()
        {
            playerEntryControl = new PlayerEntryControl(m_ViewRoot);
            m_RemoveFriendButton = GetElementByName<Button>("remove-button");
            m_BlockFriendButton = GetElementByName<Button>("block-button");

        }
        protected override void RegisterButtonCallbacks()
        {
            m_RemoveFriendButton.RegisterCallback<ClickEvent>((clickEvent) =>
            {
                onRemoveFriendPressed?.Invoke(playerEntryControl.Id);
            });
            m_BlockFriendButton.RegisterCallback<ClickEvent>((clickEvent) =>
            {
                onBlockFriendPressed?.Invoke(playerEntryControl.Id);
            });
        }
    }

}
