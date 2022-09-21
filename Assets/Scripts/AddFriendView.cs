using System;
using UnityEngine;
using UnityEngine.UI;

namespace UnityGamingServicesUsesCases.Relationships
{
    public class AddFriendView : MonoBehaviour
    {
        public Action<string> OnAddFriendRequested;

        [SerializeField] private Button _button = null;
        [SerializeField] private PlayerIdsData _playerData = null;

        private void Awake()
        {
            _button.onClick.AddListener(() => OnAddFriendRequested?.Invoke(_playerData[0]));
        }
    }
}