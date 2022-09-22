using System;
using UnityEngine;
using UnityEngine.UI;

namespace UnityGamingServicesUsesCases.Relationships
{
    public class AcceptRequestView : MonoBehaviour
    {
        public Action<string> OnRequestAccepted;

        [SerializeField] private Button _button = null;
        [SerializeField] private PlayerProfilesData _playerData = null;

        private void Awake()
        {
            _button.onClick.AddListener(() => OnRequestAccepted?.Invoke(_playerData.GetId("Player_4")));
        }
    }
}