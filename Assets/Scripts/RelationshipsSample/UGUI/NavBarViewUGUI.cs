using System;
using UnityEngine;
using UnityEngine.UI;

namespace UnityGamingServicesUsesCases.Relationships.UGUI
{
    public class NavBarViewUGUI : MonoBehaviour, IRelationshipBarView
    {
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

            foreach (var info in m_NavBarTabs)
            {
                info.NavBarButton.Init();
                info.NavBarButton.onSelected += () => { ShowListView(info); };
                info.ListView.Hide();
            }

            m_AddFriendButton.onClick.AddListener(() => { onShowRequestFriend?.Invoke(); });
        }

        public void Refresh()
        {
            m_CurrentSelectedTab?.ListView.Refresh();
        }

        void ShowListView(NavBarTab info)
        {
            if (info == m_CurrentSelectedTab)
                return;

            if (m_CurrentSelectedTab != null)
            {
                m_CurrentSelectedTab.NavBarButton.Deselect();
                m_CurrentSelectedTab.ListView.Hide();
            }

            m_CurrentSelectedTab = info;
            m_CurrentSelectedTab.NavBarButton.Select();
            m_CurrentSelectedTab.ListView.Show();
        }

        public Action onShowRequestFriend { get; set; }

        class NavBarTab
        {
            public IListView ListView;
            public NavBarButtonUGUI NavBarButton;
        }
    }
}