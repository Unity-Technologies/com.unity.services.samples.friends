using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityGamingServicesUsesCases.Relationships
{
    [CreateAssetMenu(fileName = "playerIds_data", menuName = "Data/PlayerIds")]
    public class PlayerProfilesData : ScriptableObject, IEnumerable<PlayerProfile>
    {
        [SerializeField]
        List<PlayerProfile> m_PlayerProfiles = new List<PlayerProfile>();

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
        public string Name => m_Name;
        public string Id => m_Id;
        [SerializeField]
        string m_Name;
        [SerializeField]
        string m_Id;

        public PlayerProfile(string name, string id)
        {
            m_Name = name;
            m_Id = id;
        }

        public override string ToString()
        {
            return $"{m_Name} , Id :{m_Id}";
        }
    }
}
