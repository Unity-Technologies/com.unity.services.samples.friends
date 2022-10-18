using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;

namespace UnityGamingServicesUsesCases.Relationships.UIToolkit
{
    public class RequestFriendView : IRequestFriendView
    {
        const string k_RequestFriendViewName = "request-friend-view";
        public Action<string> tryRequestFriend { get; set; }

        TextField m_RequestFriendField;
        VisualElement m_RequestFriendView;
        Label m_WarningLabel;

        public RequestFriendView(VisualElement viewParent)
        {
            m_RequestFriendView = viewParent.Q(k_RequestFriendViewName);

            var exitButton = m_RequestFriendView.Q<Button>("exit-button");
            exitButton.RegisterCallback<ClickEvent>((e) =>
            {
                Hide();
            });

            var clickOffButton = m_RequestFriendView.Q<Button>("request-friend-clickoff-button");
            clickOffButton.RegisterCallback<ClickEvent>((e) =>
            {
                Hide();
            });
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
            var requestFriendButton = m_RequestFriendView.Q<Button>("request-button");
            requestFriendButton.RegisterCallback<ClickEvent>(_ =>
            {
                tryRequestFriend?.Invoke(m_RequestFriendField.text);
            });
        }

        public void RequestFriendSuccess() { }

        public async void RequestFriendFailed()
        {
            m_WarningLabel.style.opacity = 1;
            await Task.Delay(2000);
            m_WarningLabel.style.opacity = 0;
        }

        public void Show()
        {
            m_RequestFriendView.style.display = DisplayStyle.Flex;
        }

        public void Hide()
        {
            m_RequestFriendView.style.display = DisplayStyle.None;
        }
    }
}
