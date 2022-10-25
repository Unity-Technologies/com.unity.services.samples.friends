using System;
using UnityEngine;
using UnityEngine.UI;

namespace UnityGamingServicesUsesCases.Relationships.UGUI
{
    public class NavBarViewUGUI : MonoBehaviour, IRelationshipBarView
    {
        public Action onShowRequestFriend { get; set; }
        
        [SerializeField] NavBarButtonUGUI[] m_NavBarButtons;
        [SerializeField] Button m_AddFriendButton;
        NavBarTab m_CurrentSelectedTab = null;
        NavBarTab[] m_NavBarTabs;

        public void Init(IListView[] listViews)
        {
            var count = listViews.Length;
            m_NavBarTabs = new NavBarTab[count];
            for (int i = 0; i < count; i++)
            {
                m_NavBarTabs[i] = new NavBarTab()
                {
                    ListView = listViews[i],
                    NavBarButton = m_NavBarButtons[i]
                };
            }

            foreach (var navBarTab in m_NavBarTabs)
            {
                navBarTab.NavBarButton.Init();
                navBarTab.NavBarButton.onSelected += () => { ShowListView(navBarTab); };
                navBarTab.ListView.Hide();
            }

            m_AddFriendButton.onClick.AddListener(() => { onShowRequestFriend?.Invoke(); });
        }

        public void Refresh()
        {
            m_CurrentSelectedTab?.ListView.Refresh();
        }

        void ShowListView(NavBarTab navBarTab)
        {
            if (m_CurrentSelectedTab != null)
            {
                m_CurrentSelectedTab.NavBarButton.Deselect();
                m_CurrentSelectedTab.ListView.Hide();
            }
            
            if (navBarTab == m_CurrentSelectedTab)
            {
                m_CurrentSelectedTab = null;
                return;
            }

            m_CurrentSelectedTab = navBarTab;
            m_CurrentSelectedTab.ListView.Show();
        }
        
        class NavBarTab
        {
            public IListView ListView;
            public NavBarButtonUGUI NavBarButton;
        }
    }
}