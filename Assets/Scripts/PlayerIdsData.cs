using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityGamingServicesUsesCases.Relationships
{
    [CreateAssetMenu(fileName = "playerIds_data", menuName = "Data/PlayerIds")]
    public class PlayerIdsData : ScriptableObject
    {
        [SerializeField] private List<string> m_PlayerIds = new List<string>();
        
        public string this[int i]
        {
            get => m_PlayerIds[i];
            set => m_PlayerIds[i] = value;
        }

        public void Add(string id)
        {
            if (m_PlayerIds.Contains(id))
                return;
            m_PlayerIds.Add(id);
            PlayerPrefs.SetString(name,id);
        }

        public void Clear()
        {
            m_PlayerIds.Clear();
            PlayerPrefs.DeleteAll();
        }
    }
}