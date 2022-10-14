using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityGamingServicesUsesCases.Relationships
{
    public class RequestsView : MonoBehaviour
    {
        public Action<string> OnRequestAccepted = null;
        public Action<string> OnRequestDeclined = null;

        [SerializeField] private RectTransform m_ParentTransform = null;
        [SerializeField] private GenericEntryView m_RequestEntryViewPrefab = null;

        List<GenericEntryView> m_Requests = new List<GenericEntryView>();

        public void Refresh(List<PlayerProfile> playerProfiles)
        {
            foreach (var entry in m_Requests)
            {
                Destroy(entry.gameObject);
            }
            m_Requests.Clear();

            foreach (var playerProfile in playerProfiles)
            {
                var entry = Instantiate(m_RequestEntryViewPrefab, m_ParentTransform);
                entry.Init(playerProfile.Name);
                entry.button1.onClick.AddListener(() => { OnRequestAccepted?.Invoke(playerProfile.Id); });
                entry.button2.onClick.AddListener(() => { OnRequestDeclined?.Invoke(playerProfile.Id); });
                m_Requests.Add(entry);
            }
        }
    }
}
