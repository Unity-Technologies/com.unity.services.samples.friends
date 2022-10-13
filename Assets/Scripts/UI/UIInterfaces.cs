using System;
using System.Collections.Generic;
using Unity.Services.Friends.Models;
using UnityEngine;

namespace UnityGamingServicesUsesCases.Relationships
{
    public interface IRelationshipsUIController
    {
        ILocalPlayerView LocalPlayerView { get; }
        IRelationshipBarView RelationshipBarView { get; }
        IRequestFriendView RequestFriendView { get; }
        IFriendsListView FriendsListView { get; }
        IRequestListView RequestListView { get; }
        IBlockedListView BlockListView { get; }

        void Init();
    }

    public interface ILocalPlayerView
    {
        Action<(PresenceAvailabilityOptions, string)> onPresenceChanged { get; set; }

        void Refresh(string name, string id, string acitvity,
            PresenceAvailabilityOptions presenceAvailabilityOptions);
    }

    public interface IRelationshipBarView
    {
        Action onShowFriends { get; set; }
        Action onShowRequests { get; set; }
        Action onShowBlocks { get; set; }
        Action onShowRequestFriend { get; set; }
    }

    public interface IRequestFriendView
    {
        void AddFriendSuccess();
        void AddFriendFailed();
        Action<string> tryAddFriend { get; set; }
        bool IsShowing { get; }
        void Show();
        void Hide();
    }

    public interface IFriendsListView
    {
        Action<string> onRemoveFriend { get; set; }
        Action<string> onBlockFriend { get; set; }
        void BindList(List<FriendsEntryData> listToBind);
        void Show();
        void Hide();
        void Refresh();
    }

    public interface IRequestListView
    {
        Action<string> onAcceptUser { get; set; }
        Action<string> onDeclineUser { get; set; }
        Action<string> onBlockUser { get; set; }
        void BindList(List<PlayerProfile> listToBind);
        void Show();
        void Hide();
        void Refresh();
    }

    public interface IBlockedListView
    {
        Action<string> onUnBlock { get; set; }
        void BindList(List<PlayerProfile> listToBind);
        void Show();
        void Hide();
        void Refresh();
    }
}