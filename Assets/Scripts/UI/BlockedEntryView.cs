using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnityGamingServicesUsesCases.Relationships.UI
{
    public class BlockedEntryView
    {
        const string k_BlockEntryViewName = "block-entry-view";
        public Action onUnBlock;
        Label m_PlayerName;

        public BlockedEntryView(VisualElement viewParent)
        {
            var blockedEntryRoot = viewParent.Q(k_BlockEntryViewName);
            m_PlayerName = blockedEntryRoot.Q<Label>("player-name-label");
            var blockButton = blockedEntryRoot.Q<Button>("unblock-button");
            blockButton.RegisterCallback<ClickEvent>(_ =>
                {
                    onUnBlock?.Invoke();
                }
            );
        }

        public void Refresh(string name)
        {
            m_PlayerName.text = name;
        }
    }
}