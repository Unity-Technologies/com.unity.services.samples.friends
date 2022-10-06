using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnityGamingServicesUsesCases.Relationships.UI
{
    public class BlockedEntryControl
    {
        const string k_BlockEntryViewName = "block-entry-view";

        public Action onUnblock;

        public PlayerEntryControl playerEntryControl;

        public BlockedEntryControl(VisualElement viewParent)
        {
            var blockedEntryRoot = viewParent.Q(k_BlockEntryViewName);
            playerEntryControl = new PlayerEntryControl(blockedEntryRoot);

            var blockButton = blockedEntryRoot.Q<Button>("unblock-button");
            blockButton.RegisterCallback<ClickEvent>(_ =>
            {
                onUnblock?.Invoke();
            });
        }
    }
}
