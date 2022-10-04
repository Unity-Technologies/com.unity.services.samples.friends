using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnityGamingServicesUsesCases.Relationships.UI
{
    public class BlockedEntryControl
    {
        const string k_BlockEntryRootName = "block-entry-view";

        public Action onUnblockPressed;

        public PlayerEntryControl playerEntryControl;

        Button m_BlockButton;


        VisualElement m_BlockEntryRoot;

        public BlockedEntryControl(VisualElement documentParent)
        {
            m_BlockEntryRoot = documentParent.Q(k_BlockEntryRootName);
            playerEntryControl = new PlayerEntryControl(m_BlockEntryRoot);

            m_BlockButton = m_BlockEntryRoot.Q<Button>("unblock-button");
            m_BlockButton.RegisterCallback<ClickEvent>(_ =>
            {
                onUnblockPressed?.Invoke();
            });
        }
    }
}