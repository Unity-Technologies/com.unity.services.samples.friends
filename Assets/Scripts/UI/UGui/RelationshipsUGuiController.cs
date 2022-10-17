using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGamingServicesUsesCases.Relationships;

public class RelationshipsUGuiController : MonoBehaviour, IRelationshipsUIController
{

    [SerializeField] private PlayerInfoView m_PlayerInfoView;
    [SerializeField] private RelationshipNavBar m_RelationshipNavBar;
    [SerializeField] private FriendsView m_FriendsView;
    [SerializeField] private RequestsView m_RequestsView;
    [SerializeField] private BlocksView m_BlocksView;
    public ILocalPlayerView LocalPlayerView => m_PlayerInfoView;

    public IRelationshipBarView RelationshipBarView => m_RelationshipNavBar;
    public IRequestFriendView RequestFriendView { get; }
    public IFriendsListView FriendsListView => m_FriendsView;
    public IRequestListView RequestListView => m_RequestsView;
    public IBlockedListView BlockListView => m_BlocksView;
    public void Init()
    {
        m_RelationshipNavBar.onShowFriends += ShowFriends;
        m_RelationshipNavBar.onShowRequests += ShowRequests;
        m_RelationshipNavBar.onShowBlocks += ShowBlocks;
        HideAll();
    }

    void HideAll()
    {
        FriendsListView.Hide();
        RequestListView.Hide();
        BlockListView.Hide();
    }

    void ShowFriends()
    {
        FriendsListView.Show();
        RequestListView.Hide();
        BlockListView.Hide();
    }

    void ShowRequests()
    {
        RequestListView.Show();
        FriendsListView.Hide();
        BlockListView.Hide();
    }

    void ShowBlocks()
    {
        BlockListView.Show();
        RequestListView.Hide();
        FriendsListView.Hide();
    }
}
