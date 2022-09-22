using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityGamingServicesUsesCases.Relationships
{
    [CreateAssetMenu(fileName = "playerIds_data", menuName = "Data/PlayerIds")]
    public class PlayerProfilesData : ScriptableObject, IEnumerable<PlayerProfile>
    {
        [SerializeField] private List<PlayerProfile> _playerProfiles = new List<PlayerProfile>();
       
        public PlayerProfile this[int i]
        {
            get => _playerProfiles[i];
            set => _playerProfiles[i] = value;
        }

        public void Add(string playerName, string id)
        {
            var playerData = new PlayerProfile { Name = playerName, Id = id };
            _playerProfiles.Add(playerData);
            PlayerPrefs.SetString(playerName,id);
        }

        public void Clear()
        {
            _playerProfiles.Clear();
            PlayerPrefs.DeleteAll();
        }

        public string GetId(string playerName)
        {
            return _playerProfiles.First(x => x.Name == playerName).Id;
        }
        
        public string GetName(string id)
        {
            return _playerProfiles.First(x => x.Id == id).Name;
        }

        public IEnumerator<PlayerProfile> GetEnumerator()
        {
            return _playerProfiles.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    [System.Serializable]
    public class PlayerProfile
    {
        [field: SerializeField] public string Name;
        [field: SerializeField] public string Id;

        public override string ToString()
        {
            return $"{Name} : {Id}";
        }
    }
}