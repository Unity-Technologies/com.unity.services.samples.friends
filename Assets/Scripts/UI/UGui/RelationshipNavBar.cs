using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGamingServicesUsesCases.Relationships;

public class RelationshipNavBar : MonoBehaviour, IRelationshipBarView
{
    [SerializeField] private Button m_FriendsButton;
    [SerializeField] private Button m_RequestsButton;
    [SerializeField] private Button m_BlocksButton;
    
    private void Awake()
    {
        m_FriendsButton.onClick.AddListener(() => onShowFriends?.Invoke());
        m_RequestsButton.onClick.AddListener(() => onShowRequests?.Invoke());
        m_BlocksButton.onClick.AddListener(() => onShowBlocks?.Invoke());
    }

    public Action onShowFriends { get; set; }
    public Action onShowRequests { get; set; }
    public Action onShowBlocks { get; set; }
    public Action onShowRequestFriend { get; set; }
}