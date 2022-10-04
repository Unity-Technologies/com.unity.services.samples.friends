using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnityGamingServicesUsesCases.Relationships.UI
{
    public class RequestEntryControl
    {
        const string k_RequestEntryRootName = "request-friend-view";

        public Action onAcceptPressed;
        public Action onDenyPressed;
        public Action onBlockFriendPressed;

        public PlayerEntryControl playerEntryControl;

        Button m_AcceptButton { get; }
        Button m_DenyButton { get; }
        Button m_BlockButton { get; }


        VisualElement m_RequestEntryRoot;

        public RequestEntryControl(VisualElement documentParent)
        {
            m_RequestEntryRoot = documentParent.Q(k_RequestEntryRootName);
            playerEntryControl = new PlayerEntryControl(m_RequestEntryRoot);
            m_AcceptButton = m_RequestEntryRoot.Q<Button>("accept-button");

            m_AcceptButton.RegisterCallback<ClickEvent>(_ =>
            {
                onAcceptPressed?.Invoke();
            });

            m_DenyButton = m_RequestEntryRoot.Q<Button>("remove-button");
            m_DenyButton.RegisterCallback<ClickEvent>(_ =>
            {
                onDenyPressed?.Invoke();
            });

            m_BlockButton = m_RequestEntryRoot.Q<Button>("block-button");
            m_BlockButton.RegisterCallback<ClickEvent>(_ =>
            {
                onBlockFriendPressed?.Invoke();
            });
        }
    }
}