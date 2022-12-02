using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Unity.Services.Toolkits.Relationships
{
    public class AddFriendDebugView : MonoBehaviour
    {
        public Action<string> OnAddFriend;

        [SerializeField] private Button m_Button = null;
        [SerializeField] private TMP_Dropdown m_Dropdown = null;
        [SerializeField] private PlayerProfilesData m_PlayerProfilesData = null;

        public void Init()
        {
            var names = new List<string>();
            foreach (var playerData in m_PlayerProfilesData)
            {
                names.Add(playerData.Name);
            }

            var playerName = names[0];
            m_Dropdown.AddOptions(names);
            m_Dropdown.onValueChanged.AddListener((value) => { playerName = names[value]; });

            m_Button.onClick.AddListener(() => OnAddFriend?.Invoke(m_PlayerProfilesData.GetId(playerName)));
        }
    }
}