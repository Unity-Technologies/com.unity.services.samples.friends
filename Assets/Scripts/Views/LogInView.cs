using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace UnityGamingServicesUsesCases.Relationships
{
    public class LogInView : MonoBehaviour
    {
        public Action<string> OnLogIn;

        [SerializeField] private Button _button = null;
        [SerializeField] private TMP_Dropdown _dropdown = null;
        [SerializeField] private PlayerProfilesData playerProfilesData = null;

        private int _playerId;

        public void Init()
        {
            var names = new List<string>();
            foreach (var playerData in playerProfilesData)
            {
                names.Add(playerData.Name);
            }
            _dropdown.AddOptions(names);
            var selectedPlayerName = string.Empty;
            _dropdown.onValueChanged.AddListener((value) => { selectedPlayerName = names[value]; });
            selectedPlayerName = names[0];
            
            _button.onClick.AddListener(() => OnLogIn?.Invoke(selectedPlayerName));
        }

      

       
    }
}