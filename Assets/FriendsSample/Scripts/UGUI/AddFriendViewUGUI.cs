using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Unity.Services.Samples.Friends.UGUI
{
    public class AddFriendViewUGUI : MonoBehaviour, IAddFriendView
    {
        [SerializeField] Button m_AddFriendButton = null;
        [SerializeField] Button m_CloseButton = null;
        [SerializeField] Button m_BackgroundButton = null;
        [SerializeField] TMP_InputField m_IdInputField = null;
        [SerializeField] TextMeshProUGUI m_RequestResultText = null;
        public Action<string> onFriendRequestSent { get; set; }

        public void Init()
        {
            var playerId = string.Empty;
            m_IdInputField.onValueChanged.AddListener((value) => { playerId = value; });
            m_AddFriendButton.onClick.AddListener(() =>
            {
                m_RequestResultText.text = string.Empty;
                onFriendRequestSent?.Invoke(playerId);
            });
            m_BackgroundButton.onClick.AddListener(Hide);
            m_CloseButton.onClick.AddListener(Hide);
            Hide();
        }

        public void FriendRequestSuccess()
        {
            m_RequestResultText.text = "Friend request sent!";
        }

        public void FriendRequestFailed()
        {

        }

        public void Show()
        {
            m_RequestResultText.text = string.Empty;
            m_IdInputField.SetTextWithoutNotify("");
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}