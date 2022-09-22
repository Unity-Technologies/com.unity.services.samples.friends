using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityGamingServicesUsesCases.Relationships
{
    [CreateAssetMenu(fileName = "playerIds_data", menuName = "Data/PlayerIds")]
    public class PlayerIdsData : ScriptableObject
    {
        [SerializeField] private List<string> _playerIds = new List<string>();
        
        public string this[int i]
        {
            get => _playerIds[i];
            set => _playerIds[i] = value;
        }

        public void Add(string id)
        {
            if (_playerIds.Contains(id))
                return;
            _playerIds.Add(id);
            PlayerPrefs.SetString(name,id);
        }

        public void Clear()
        {
            _playerIds.Clear();
            PlayerPrefs.DeleteAll();
        }
    }
}