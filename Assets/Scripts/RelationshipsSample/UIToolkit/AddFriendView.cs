using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;

namespace UnityGamingServicesUsesCases.Relationships.UIToolkit
{
    public class AddFriendView : IAddFriendView
    {
        const string k_AddFriendViewName = "request-friend-bg";
        public Action<string> onFriendRequestSent { get; set; }

        TextField m_AddFriendField;
        VisualElement m_AddFriendView;
        Label m_WarningLabel;

        public AddFriendView(VisualElement viewParent)
        {
            m_AddFriendView = viewParent.Q(k_AddFriendViewName);

            var exitButton = m_AddFriendView.Q<Button>("exit-button");
            exitButton.RegisterCallback<ClickEvent>((e) =>
            {
                Hide();
            });

            var clickOffButton = m_AddFriendView.Q<Button>("request-friend-clickoff-button");
            clickOffButton.RegisterCallback<ClickEvent>((e) =>
            {
                Hide();
            });
            m_WarningLabel = m_AddFriendView.Q<Label>("warning-label");
            m_AddFriendField = m_AddFriendView.Q<TextField>("search-field");

            //Support for Enter and Numpad Enter
            m_AddFriendField.Q(TextField.textInputUssName).RegisterCallback<KeyDownEvent>(
                evt =>
                {
                    if (evt.keyCode == KeyCode.Return || evt.keyCode == KeyCode.KeypadEnter)
                    {
                        onFriendRequestSent?.Invoke(m_AddFriendField.text);
                    }
                });
            var requestFriendButton = m_AddFriendView.Q<Button>("add-button");
            requestFriendButton.RegisterCallback<ClickEvent>(_ =>
            {
                onFriendRequestSent?.Invoke(m_AddFriendField.text);
            });
        }

        public void FriendRequestSuccess() { }

        public async void FriendRequestFailed()
        {
            m_WarningLabel.style.opacity = 1;
            await Task.Delay(2000);
            m_WarningLabel.style.opacity = 0;
        }

        public void Show()
        {
            m_AddFriendView.style.display = DisplayStyle.Flex;
        }

        public void Hide()
        {
            m_AddFriendView.style.display = DisplayStyle.None;
        }
    }
}
