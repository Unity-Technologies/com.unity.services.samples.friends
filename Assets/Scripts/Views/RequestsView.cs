using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityGamingServicesUsesCases.Relationships
{
    public class RequestsView : MonoBehaviour,IRequestListView
    {
        //public Action<string> OnRequestAccepted = null;
        //public Action<string> OnRequestDeclined = null;

        [SerializeField] private RectTransform m_ParentTransform = null;
        [SerializeField] private GenericEntryView m_RequestEntryViewPrefab = null;

        List<GenericEntryView> m_Requests = new List<GenericEntryView>();

        private List<PlayerProfile> m_PlayerProfiles = new List<PlayerProfile>();
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
                entry.button1.onClick.AddListener(() => { onAcceptUser?.Invoke(playerProfile.Id); });
                entry.button2.onClick.AddListener(() => { onDeclineUser?.Invoke(playerProfile.Id); });
                m_Requests.Add(entry);
            }
        }

        public Action<string> onAcceptUser { get; set; }
        public Action<string> onDeclineUser { get; set; }
        public Action<string> onBlockUser { get; set; }
        public void BindList(List<PlayerProfile> listToBind)
        {
            m_PlayerProfiles = listToBind;
        }

        public void Show()
        {
            Refresh();
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Refresh()
        {
            Refresh(m_PlayerProfiles);
        }
    }
}
