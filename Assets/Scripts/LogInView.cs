using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace UnityGamingServicesUsesCases.Relationships
{
    public class LogInView : MonoBehaviour
    {
        public Action<int> OnLogIn;

        [SerializeField] private Button _button = null;
        [SerializeField] private InputField _inputField = null;
        [SerializeField] private PlayerIdsData _playerIdsData = null;

        private int _playerId;
        
        private void Awake()
        {
            _button.onClick.AddListener(() => OnLogIn?.Invoke(_playerId));
            _inputField.onValueChanged.AddListener(OnValueChanged);
        }

        private void OnValueChanged(string arg0)
        {
            _playerId = int.Parse(arg0);
        }
    }
}