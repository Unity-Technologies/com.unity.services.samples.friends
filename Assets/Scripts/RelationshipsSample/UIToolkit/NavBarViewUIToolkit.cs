using System;
using UnityEngine.UIElements;

namespace UnityGamingServicesUsesCases.Relationships.UIToolkit
{
    public class NavBarViewUIToolkit : IRelationshipBarView
    {
        const string k_RelationshipsBarViewName = "nav-bar-view";
        public Action onShowRequestFriend { get; set; }

        NavBarTab m_CurrentSelectedTab;
        NavBarTab[] m_NavBarTabs;

        public NavBarViewUIToolkit(VisualElement viewParent, IListView[] listViews)
        {
            InitNavBar(viewParent, listViews);

            var relationshipsBarView = viewParent.Q(k_RelationshipsBarViewName);

            var addFriendButton = relationshipsBarView.Q<Button>("add-friend-button");
            addFriendButton.RegisterCallback<ClickEvent>((_) => { onShowRequestFriend?.Invoke(); });
        }

        private void InitNavBar(VisualElement viewParent, IListView[] listViews)
        {
            var friendsButton = new NavBarButtonUIToolkit(viewParent, "friends-button");
            var requestsButton = new NavBarButtonUIToolkit(viewParent, "requests-button");
            var blocksButton = new NavBarButtonUIToolkit(viewParent, "blocks-button");
            var navBarButtons = new[] { friendsButton, requestsButton, blocksButton };

            var count = listViews.Length;
            m_NavBarTabs = new NavBarTab[count];
            for (int i = 0; i < count; i++)
            {
                m_NavBarTabs[i] = new NavBarTab()
                {
                    ListView = listViews[i],
                    NavBarButton = navBarButtons[i]
                };
            }

            foreach (var navBarTab in m_NavBarTabs)
            {
                navBarTab.NavBarButton.onSelected += () => { ShowListView(navBarTab); };
                navBarTab.ListView.Hide();
            }
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

        private class NavBarTab
        {
            public IListView ListView;
            public NavBarButtonUIToolkit NavBarButton;
        }
    }
}