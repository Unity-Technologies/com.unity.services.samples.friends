using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UnityGamingServicesUsesCases.Relationships.UGUI
{
    public class AddFriendViewUGUI : MonoBehaviour, IAddFriendView
    {
        [SerializeField] private Button m_AddFriendButton = null;
        [SerializeField] private Button m_CloseButton = null;
        [SerializeField] private Button m_BackgroundButton = null;
        [SerializeField] private TMP_InputField m_IdInputField = null;

        public void Init()
        {
            var playerId = string.Empty;
            m_IdInputField.onValueChanged.AddListener((value) => { playerId = value; });
            m_AddFriendButton.onClick.AddListener(() => onFriendRequestSent?.Invoke(playerId));
            m_BackgroundButton.onClick.AddListener(Hide);
            m_CloseButton.onClick.AddListener(Hide);
            Hide();
        }
        
        public void FriendRequestSuccess()
        {
            
        }

        public void FriendRequestFailed()
        {
          //
        }

        public Action<string> onFriendRequestSent { get; set; }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}