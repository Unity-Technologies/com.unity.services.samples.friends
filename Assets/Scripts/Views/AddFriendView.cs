using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UnityGamingServicesUsesCases.Relationships
{
    public class AddFriendView : MonoBehaviour
    {
        public Action<string> OnAddFriend;

        [SerializeField] private Button _button = null;
        [SerializeField] private TMP_Dropdown _dropdown = null;
        [SerializeField] private PlayerProfilesData playerProfilesData = null;

        private string _selectedPlayerName = string.Empty;
        public void Init()
        {
            var names = new List<string>();
            foreach (var playerData in playerProfilesData)
            {
                names.Add(playerData.Name);
            }
            
            _dropdown.AddOptions(names);
            _dropdown.onValueChanged.AddListener((value) => { _selectedPlayerName = names[value]; });
            _selectedPlayerName = names[0];

            _button.onClick.AddListener(() => OnAddFriend?.Invoke(playerProfilesData.GetId(_selectedPlayerName)));
        }
    }
}