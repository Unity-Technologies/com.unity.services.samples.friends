using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UnityGamingServicesUsesCases.Relationships
{
    public class AddFriendByIdView : MonoBehaviour
    {
        public Action<string> OnAddFriend;

        [SerializeField] private Button m_Button = null;
        [SerializeField] private TMP_InputField m_InputField = null;

        public void Init()
        {
            var playerName = string.Empty;
            m_InputField.onValueChanged.AddListener((value) => { playerName = value; });
            m_Button.onClick.AddListener(() => OnAddFriend?.Invoke(playerName));
        }
    }
}