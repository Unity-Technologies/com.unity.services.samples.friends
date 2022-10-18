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
            m_Button.onClick.AddListener(() => tryRequestFriend?.Invoke(playerId));
            m_backgroundButton.onClick.AddListener(Hide);
            m_closeButton.onClick.AddListener(Hide);
            Hide();
        }
        
        public void RequestFriendSuccess()
        {
            
        }

        public void RequestFriendFailed()
        {
          //
        }

        public Action<string> tryRequestFriend { get; set; }

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