using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UnityGamingServicesUsesCases.Relationships.UGUI
{
    public class SendRequestViewUGUI : MonoBehaviour, IRequestFriendView
    {
        [SerializeField] private Button m_Button = null;
        [SerializeField] private Button m_closeButton = null;
        [SerializeField] private Button m_backgroundButton = null;
        [SerializeField] private TMP_InputField m_InputField = null;

        public void Init()
        {
            var playerId = string.Empty;
            m_InputField.onValueChanged.AddListener((value) => { playerId = value; });
            m_Button.onClick.AddListener(() => tryAddFriend?.Invoke(playerId));
            m_backgroundButton.onClick.AddListener(Hide);
            m_closeButton.onClick.AddListener(Hide);
            Hide();
        }

        public void AddFriendSuccess()
        {
           
        }

        public void AddFriendFailed()
        {
           
        }

        public Action<string> tryAddFriend { get; set; }
        public bool IsShowing { get; }
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