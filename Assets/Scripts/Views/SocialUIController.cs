using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class SocialUIController : MonoBehaviour
{
    [SerializeField]
    UIDocument m_SocialUIDoc;

    VisualElement m_Root;

    LocalUserView m_LocalUserView;
    SocialBarView m_SocialBarView;
    FriendsListView m_FriendsListView;
    RequestListView m_RequestListView;
    BlockedListView m_BlockedListView;

    public enum ShowListState
    {
        Friends = 0,
        Requests = 1,
        Blocked = 2,
        None = 3
    }

    List<UIBaseView> m_ModalList;

    void Awake()
    {
        m_Root = m_SocialUIDoc.rootVisualElement;

        //Initialize with the various View Roots
        m_LocalUserView = new LocalUserView(m_Root.Q("LocalUserView"));
        m_SocialBarView = new SocialBarView(m_Root.Q("SocialBarView"));
        m_FriendsListView = new FriendsListView(m_Root.Q("FriendsListView"));
        m_RequestListView = new RequestListView(m_Root.Q("RequestListView"));
        m_BlockedListView = new BlockedListView(m_Root.Q("BlockedListView"));
        m_ModalList = new List<UIBaseView> { m_FriendsListView, m_RequestListView, m_BlockedListView };
        ShowList(ShowListState.None);



    }



    public void ShowList(ShowListState state)
    {
        for (var i = 0; i < m_ModalList.Count; i++)
        {
            if (i == (int)state)
                m_ModalList[i].ShowScreen();
            else
            {
                m_ModalList[i].HideScreen();
            }
        }
    }
}