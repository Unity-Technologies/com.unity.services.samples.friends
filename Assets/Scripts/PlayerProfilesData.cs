using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityGamingServicesUsesCases.Relationships
{
    [CreateAssetMenu(fileName = "playerIds_data", menuName = "Data/PlayerIds")]
    public class PlayerProfilesData : ScriptableObject, IEnumerable<PlayerProfile>
    {
        [SerializeField] private List<PlayerProfile> m_PlayerProfiles = new();

        public void Add(string playerName, string id)
        {
            var playerProfile = new PlayerProfile(playerName, id);
            m_PlayerProfiles.Add(playerProfile);
            Debug.Log($"Added: {playerProfile}");
        }

        public void Clear()
        {
            m_PlayerProfiles.Clear();
            PlayerPrefs.DeleteAll();
        }

        public string GetId(string playerName)
        {
            return m_PlayerProfiles.First(x => x.Name == playerName).Id;
        }

        public string GetName(string id)
        {
            foreach (var playerProfile in m_PlayerProfiles)
            {
                if (id == playerProfile.Id)
                    return playerProfile.Name;
            }
            
            return $"Friend_{id}";
        }

        public IEnumerator<PlayerProfile> GetEnumerator()
        {
            return m_PlayerProfiles.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }


    [System.Serializable]
    public class PlayerProfile
    {

        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public string Id { get; private set; }

        public PlayerProfile(string name, string id)
        {
            Name = name;
            Id = id;
        }

        public override string ToString()
        {
            return $"{Name} , Id :{Id}";
        }
    }
}
