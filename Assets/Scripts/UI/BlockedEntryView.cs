using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnityGamingServicesUsesCases.Relationships.UI
{
    public class BlockedEntryView
    {
        const string k_BlockEntryViewName = "block-entry-view";

        public Action onUnblock;

        public LocalPlayerView localPlayerView;

        public BlockedEntryView(VisualElement viewParent)
        {
            var blockedEntryRoot = viewParent.Q(k_BlockEntryViewName);
            localPlayerView = new LocalPlayerView(blockedEntryRoot);

            var blockButton = blockedEntryRoot.Q<Button>("unblock-button");
            blockButton.RegisterCallback<ClickEvent>(_ =>
            {
                onUnblock?.Invoke();
            });
        }
    }
}
