using System;
using UnityEngine.UIElements;

namespace UnityGamingServicesUsesCases.Relationships.UI
{
    public class RequestEntryControl
    {
        const string k_RequestEntryViewName = "request-friend-view";

        public Action onAccept;
        public Action onDeny;
        public Action onBlockFriend;

        public PlayerEntryControl playerEntryControl;

        public RequestEntryControl(VisualElement viewParent)
        {
            var requestEntryView = viewParent.Q(k_RequestEntryViewName);
            playerEntryControl = new PlayerEntryControl(requestEntryView);
            var acceptButton = requestEntryView.Q<Button>("accept-button");

            acceptButton.RegisterCallback<ClickEvent>(_ =>
            {
                onAccept?.Invoke();
            });

            var denyButton = requestEntryView.Q<Button>("remove-button");
            denyButton.RegisterCallback<ClickEvent>(_ =>
            {
                onDeny?.Invoke();
            });

            var blockButton = requestEntryView.Q<Button>("block-button");
            blockButton.RegisterCallback<ClickEvent>(_ =>
            {
                onBlockFriend?.Invoke();
            });
        }
    }
}
