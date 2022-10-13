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
        public Action<string> tryAddFriend { get; set; }
        public bool IsShowing { get; private set; }

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
            m_WarningLabel = m_RequestFriendView.Q<Label>("warning-label");
            m_RequestFriendField = m_RequestFriendView.Q<TextField>("search-field");

            //Support for Enter and Numpad Enter
            m_RequestFriendField.Q(TextField.textInputUssName).RegisterCallback<KeyDownEvent>(
                evt =>
                {
                    if (evt.keyCode == KeyCode.Return || evt.keyCode == KeyCode.KeypadEnter)
                    {
                        tryAddFriend?.Invoke(m_RequestFriendField.text);
                    }
                });
            var addFriendButton = m_RequestFriendView.Q<Button>("add-button");
            addFriendButton.RegisterCallback<ClickEvent>(_ =>
            {
                tryAddFriend?.Invoke(m_RequestFriendField.text);
            });
        }

        public void AddFriendSuccess() { }

        public async void AddFriendFailed()
        {
            m_WarningLabel.style.opacity = 1;
            await Task.Delay(2000);
            m_WarningLabel.style.opacity = 0;
        }

        public void Show()
        {
            m_RequestFriendView.style.display = DisplayStyle.Flex;
            IsShowing = true;
        }

        public void Hide()
        {
            m_RequestFriendView.style.display = DisplayStyle.None;
            IsShowing = false;
        }
    }
}