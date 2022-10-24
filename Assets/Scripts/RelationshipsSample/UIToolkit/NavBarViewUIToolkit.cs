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

        public void Refresh()
        {
            m_CurrentSelectedTab?.ListView.Refresh();
        }

        public NavBarViewUIToolkit(VisualElement viewParent, IListView[] listViews)
        {
            var relationshipsBarView = viewParent.Q(k_RelationshipsBarViewName);

            //instantiate the navbar button toolkit
            
            var friendsButton = new NavBarButtonUIToolkit(viewParent, "friends-button");
            var requestsButton = new NavBarButtonUIToolkit(viewParent, "requests-button");
            var blocksButton = new NavBarButtonUIToolkit(viewParent,"blocks-button");
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

            foreach (var info in m_NavBarTabs)
            {
                info.NavBarButton.onSelected += () => { ShowListView(info); };
                info.ListView.Hide();
            }

            var addFriendButton = relationshipsBarView.Q<Button>("add-friend-button");
            addFriendButton.RegisterCallback<ClickEvent>((_) => { onShowRequestFriend?.Invoke(); });
        }

        private void ShowListView(NavBarTab info)
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

        private class NavBarTab
        {
            public IListView ListView;
            public NavBarButtonUIToolkit NavBarButton;
        }
    }
}