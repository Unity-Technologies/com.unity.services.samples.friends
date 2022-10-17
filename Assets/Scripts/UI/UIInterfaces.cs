using System;
using System.Collections.Generic;
using Unity.Services.Friends.Models;
using UnityEngine;

namespace UnityGamingServicesUsesCases.Relationships
{
    //what about having interfaces in different files ?
    public interface IRelationshipsUIController
    {
        ILocalPlayerView LocalPlayerView { get; }
        IRelationshipBarView RelationshipBarView { get; }
        IRequestFriendView RequestFriendView { get; } //naming is confusing
        IFriendsListView FriendsListView { get; }
        IRequestListView RequestListView { get; }
        IBlockedListView BlockListView { get; }

        void Init();
    }

    public interface ILocalPlayerView
    {
        Action<(PresenceAvailabilityOptions, string)> onPresenceChanged { get; set; }

        void Refresh(string name, string id, string acitvity, //typo
            PresenceAvailabilityOptions presenceAvailabilityOptions);
    }

    public interface IRelationshipBarView
    {
        Action onShowFriends { get; set; } //are public camel Case?
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
        void BindList(List<FriendsEntryData> listToBind); //naming
        void Show();
        void Hide();
        void Refresh();
    }

    //forgot s at request list view
    public interface IRequestListView
    {
        Action<string> onAcceptUser { get; set; } //remove user?
        Action<string> onDeclineUser { get; set; }
        Action<string> onBlockUser { get; set; }
        void BindList(List<PlayerProfile> listToBind); //naming
        void Show();
        void Hide();
        void Refresh();
    }

    public interface IBlockedListView
    {
        Action<string> onUnBlock { get; set; } //typo
        void BindList(List<PlayerProfile> listToBind); //naming
        void Show();
        void Hide();
        void Refresh();
    }
}