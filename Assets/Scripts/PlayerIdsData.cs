using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityGamingServicesUsesCases.Relationships
{
    [CreateAssetMenu(fileName = "playerIds_data", menuName = "Data/PlayerIds")]
    public class PlayerIdsData : ScriptableObject, IEnumerable<PlayerData>
    {
        [SerializeField] private List<PlayerData> _playerDatas = new List<PlayerData>();
        //[SerializeField] private Dictionary<string, string> _playerMap = new Dictionary<string, string>();
        
        public PlayerData this[int i]
        {
            get => _playerDatas[i];
            set => _playerDatas[i] = value;
        }

        public void Add(string playerName, string id)
        {
            var playerData = new PlayerData { Name = playerName, Id = id };
            _playerDatas.Add(playerData);
            PlayerPrefs.SetString(playerName,id);
        }

        public void Clear()
        {
            _playerDatas.Clear();
            PlayerPrefs.DeleteAll();
        }

        public string GetId(string playerName)
        {
            Debug.Log(playerName);
            return _playerDatas.First(x => x.Name == playerName).Id;
        }

        public IEnumerator<PlayerData> GetEnumerator()
        {
            return _playerDatas.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    [System.Serializable]
    public class PlayerData
    {
        [field: SerializeField] public string Name;
        [field: SerializeField] public string Id;

        public override string ToString()
        {
            return $"{Name} : {Id}";
        }
    }
}