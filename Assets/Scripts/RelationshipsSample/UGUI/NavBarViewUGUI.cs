using System;
using UnityEngine;
using UnityEngine.UI;

namespace Unity.Services.Toolkits.Friends.UGUI
{
    public class NavBarViewUGUI : MonoBehaviour, IRelationshipBarView
    {
        [SerializeField] NavBarButtonUGUI[] m_NavBarButtons;
        [SerializeField] Button m_AddFriendButton;

        NavBarTab m_CurrentSelectedTab = null;
        NavBarTab[] m_NavBarTabs;
        public Action onShowAddFriend { get; set; }

        public void Init(IListView[] listViews)
        {
            var tabCount = listViews.Length;
            m_NavBarTabs = new NavBarTab[tabCount];
            for (var i = 0; i < tabCount; i++)
            {
                m_NavBarTabs[i] = new NavBarTab
                {
                    ListView = listViews[i],
                    NavBarButton = m_NavBarButtons[i]
                };
            }

            foreach (var navBarTab in m_NavBarTabs)
            {
                navBarTab.NavBarButton.Init();
                navBarTab.NavBarButton.onSelected += () => { ShowTab(navBarTab); };
                navBarTab.ListView.Hide();
            }

            m_AddFriendButton.onClick.AddListener(() => { onShowAddFriend?.Invoke(); });
        }

        public void Refresh()
        {
            m_CurrentSelectedTab?.ListView.Refresh();
        }

        void ShowTab(NavBarTab navBarTab)
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