using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;

namespace UnityGamingServicesUsesCases.Relationships.UI
{
    public class RequestFriendPopupView
    {
        const string k_RequestFriendViewName = "request-friend-view";
        public Action<string> tryRequestFriend;

        TextField m_RequestFriendField;
        VisualElement m_RequestFriendView;
        Label m_WarningLabel;

        public RequestFriendPopupView(VisualElement viewParent)
        {
            m_RequestFriendView = viewParent.Q(k_RequestFriendViewName);

            m_WarningLabel = m_RequestFriendView.Q<Label>("warning-label");
            m_RequestFriendField = m_RequestFriendView.Q<TextField>("search-field");

            //Support for Enter and Numpad Enter
            m_RequestFriendField.Q(TextField.textInputUssName).RegisterCallback<KeyDownEvent>(
                evt =>
                {
                    if (evt.keyCode == KeyCode.Return || evt.keyCode == KeyCode.KeypadEnter)
                    {
                        tryRequestFriend?.Invoke(m_RequestFriendField.text);
                    }
                });
            var addFriendButton = m_RequestFriendView.Q<Button>("add-button");
            addFriendButton.RegisterCallback<ClickEvent>(_ =>
            {
                tryRequestFriend?.Invoke(m_RequestFriendField.text);
            });
        }

        public async void ShowAddFriendFailedWarning()
        {
            m_WarningLabel.style.opacity = 1;
            await Task.Delay(2000);
            m_WarningLabel.style.opacity = 0;
        }

        public void Show(bool show)
        {
            m_RequestFriendView.style.display = show ? DisplayStyle.Flex : DisplayStyle.None;
        }
    }
}
