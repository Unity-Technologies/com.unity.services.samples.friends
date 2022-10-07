using System;
using UnityEngine.UIElements;

namespace UnityGamingServicesUsesCases.Relationships.UI
{
    public class RequestEntryView
    {
        const string k_RequestEntryViewName = "request-friend-view";

        public Action onAccept;
        public Action onDecline;
        public Action onBlockFriend;

        Label m_PlayerName;

        public RequestEntryView(VisualElement viewParent)
        {
            var requestEntryView = viewParent.Q(k_RequestEntryViewName);
            m_PlayerName = requestEntryView.Q<Label>("player-name-label");
            var acceptButton = requestEntryView.Q<Button>("accept-button");

            acceptButton.RegisterCallback<ClickEvent>(_ =>
            {
                onAccept?.Invoke();
            });

            var denyButton = requestEntryView.Q<Button>("remove-button");
            denyButton.RegisterCallback<ClickEvent>(_ =>
            {
                onDecline?.Invoke();
            });

            var blockButton = requestEntryView.Q<Button>("block-button");
            blockButton.RegisterCallback<ClickEvent>(_ =>
            {
                onBlockFriend?.Invoke();
            });
        }

        public void Refresh(string name)
        {
            m_PlayerName.text = name;
        }
    }
}