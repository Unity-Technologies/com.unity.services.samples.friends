using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityGamingServicesUsesCases.Relationships
{
    [CreateAssetMenu(fileName = "playerIds_data", menuName = "Data/PlayerIds")]
    public class PlayerProfilesData : ScriptableObject, IEnumerable<PlayerProfile>
    {
        [SerializeField] private List<PlayerProfile> m_PlayerProfiles = new ();

        public void Add(string playerName, string id)
        {
            var playerProfile = new PlayerProfile { Name = playerName, Id = id };
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

            var rand = Random.Range(0, 100);
            return $"ExternalPlayer_{rand}";
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
        [field: SerializeField] public string Name;
        [field: SerializeField] public string Id;

        public override string ToString()
        {
            return $"{Name} , Id :{Id}";
        }
    }
}