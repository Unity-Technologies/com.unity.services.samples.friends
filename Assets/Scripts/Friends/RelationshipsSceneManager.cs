using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Friends;
using Unity.Services.Friends.Exceptions;
using Unity.Services.Friends.Models;
using Unity.Services.Friends.Notifications;
using Unity.Services.Friends.Options;
using UnityEngine;
using UnityEngine.UIElements;
using UnityGamingServicesUsesCases.Relationships.UIToolkit;
using Button = UnityEngine.UI.Button;

namespace UnityGamingServicesUsesCases.Relationships
{
    public class RelationshipsSceneManager : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField]
        PlayerProfilesData m_PlayerProfilesData;

        [Header("UI")]
        [SerializeField]
        IRelationshipsUIController m_UIController;

        [Header("Debug UI")]
        [SerializeField]
        AddFriendView m_AddFriendView;

        [SerializeField]
        LogInView m_LogInView;

        [SerializeField]
        RefreshView m_RefreshView;

        [SerializeField]
        Button m_QuitButton;

        /// <summary>
        /// Serialized for debug inspection.
        /// Important to initialize these lists only once.
        /// </summary>
        [SerializeField]
        List<FriendsEntryData> m_FriendsEntryDatas = new List<FriendsEntryData>();
        [SerializeField]
        List<PlayerProfile> m_RequestsEntryDatas = new List<PlayerProfile>();
        [SerializeField]
        List<PlayerProfile> m_BlockEntryDatas = new List<PlayerProfile>();

        string m_LoggedPlayerName;
        ILocalPlayerView m_LocalPlayerView;
        IRequestFriendView m_RequestFriendView;
        IFriendsListView m_FriendsListView;
        IRequestListView m_RequestListView;
        IBlockedListView m_BlockListView;

        string LoggedPlayerId => AuthenticationService.Instance.PlayerId;

        public async Task Init(string currentPlayerName)
        {
            UISetup();
            DebugUISetup();

            m_LoggedPlayerName = currentPlayerName;
            m_LocalPlayerView.Refresh(m_LoggedPlayerName, LoggedPlayerId, "In Friends Menu",
                PresenceAvailabilityOptions.ONLINE);
            await SetPresence(PresenceAvailabilityOptions.ONLINE);
            await SubscribeToFriendsEventCallbacks();
        }

        void UISetup()
        {
            m_UIController.Init();
            m_LocalPlayerView = m_UIController.LocalPlayerView;
            m_RequestFriendView = m_UIController.RequestFriendView;

            //Bind Entry Lists
            m_FriendsListView = m_UIController.FriendsListView;
            m_FriendsListView.BindList(m_FriendsEntryDatas);
            m_RequestListView = m_UIController.RequestListView;
            m_RequestListView.BindList(m_RequestsEntryDatas);
            m_BlockListView = m_UIController.BlockListView;
            m_BlockListView.BindList(m_BlockEntryDatas);

            //Bind Friend Calls
            m_RequestFriendView.tryAddFriend += RequestFriendAsync;
            m_FriendsListView.onRemoveFriend += RemoveFriendAsync;
            m_FriendsListView.onBlockFriend += BlockFriendAsync;
            m_RequestListView.onAcceptUser += AcceptRequestAsync;
            m_RequestListView.onDeclineUser += DeclineRequestAsync;
            m_RequestListView.onBlockUser += BlockFriendAsync;
            m_BlockListView.onUnBlock += UnblockFriendAsync;
            m_LocalPlayerView.onPresenceChanged += SetPresenceAsync;
        }

        void DebugUISetup()
        {
            //Bind Actionable
            m_AddFriendView.Init();
            m_AddFriendView.OnAddFriend += RequestFriendAsync;

            m_LogInView.Init();
            m_LogInView.OnLogIn += LogIn;

            m_RefreshView.Init();
            m_RefreshView.OnRefresh += RefreshAll;
            m_QuitButton.onClick.AddListener(QuitAsync);
        }

        async void LogIn(string playerName)
        {
            await UASUtils.SwitchUser(playerName);
            m_LoggedPlayerName = playerName;
            await SetPresence(PresenceAvailabilityOptions.ONLINE);
            m_LocalPlayerView.Refresh(m_LoggedPlayerName, LoggedPlayerId, "In Friends Menu",
                PresenceAvailabilityOptions.ONLINE);
            RefreshAll();
            Debug.Log($"Logged in as {playerName} id: {LoggedPlayerId}");
            Debug.Log($"Token ID{AuthenticationService.Instance.AccessToken}");
        }

        async void BlockFriendAsync(string id)
        {
            await BlockFriend(id);
            await RefreshFriends();
            await RefreshRequests();
            await RefreshBlocks();
        }

        async void UnblockFriendAsync(string id)
        {
            await UnblockFriend(id);
            await RefreshBlocks();
            await RefreshFriends();
        }

        async void RemoveFriendAsync(string id)
        {
            await RemoveFriend(id);
            await RefreshFriends();
        }

        async void AcceptRequestAsync(string id)
        {
            await AcceptRequest(id);
            await RefreshRequests();
            await RefreshFriends();
        }

        async void DeclineRequestAsync(string id)
        {
            await DeclineRequest(id);
            await RefreshRequests();
        }

        async void QuitAsync()
        {
            Friends.Instance.Dispose();
            await Task.Delay(1000);
            Application.Quit();
        }

        async void SetPresenceAsync((PresenceAvailabilityOptions presence, string activity) status)
        {
            await SetPresence(status.presence, status.activity);
        }

        async void RequestFriendAsync(string id)
        {
            await RequestFriend(id, "button");
        }

        async void RefreshAll()
        {
            await RefreshFriends();
            await RefreshRequests();
            await RefreshBlocks();
        }

        async Task RefreshFriends()
        {
            //Friends
            var friends = await GetFriendsWithPresence();
            m_FriendsEntryDatas.Clear();
            foreach (var friend in friends)
            {
                string activityText;
                if (friend.Presence.GetAvailability() == PresenceAvailabilityOptions.OFFLINE ||
                    friend.Presence.GetAvailability() == PresenceAvailabilityOptions.INVISIBLE)
                {
                    activityText = friend.LastSeen.ToShortDateString() + " " + friend.LastSeen.ToLongTimeString();
                }
                else
                {
                    activityText = friend.Presence.GetActivity() == null ? "" : friend.Presence.GetActivity().Status;
                }

                var info = new FriendsEntryData
                {
                    Name = m_PlayerProfilesData.GetName(friend.Player.Id),
                    Id = friend.Player.Id,
                    Availability = friend.Presence.GetAvailability(),
                    Activity = activityText
                };
                m_FriendsEntryDatas.Add(info);
            }

            m_FriendsListView.Refresh();
        }

        async Task RefreshRequests()
        {
            //Requests
            var requests = await GetRequests();
            m_RequestsEntryDatas.Clear();
            foreach (var request in requests)
            {
                m_RequestsEntryDatas.Add(new PlayerProfile(m_PlayerProfilesData.GetName(request.Id), request.Id));
            }

            m_RequestListView.Refresh();
        }

        async Task RefreshBlocks()
        {
            //Blocks
            var blocks = await GetBlocks();
            m_BlockEntryDatas.Clear();
            foreach (var block in blocks)
            {
                m_BlockEntryDatas.Add(new PlayerProfile(m_PlayerProfilesData.GetName(block.Id), block.Id));
            }

            m_BlockListView.Refresh();
        }

        async Task RequestFriend(string playerId, string eventSource)
        {
            try
            {
                await Friends.Instance.AddFriendAsync(playerId, eventSource);
                Debug.Log($"{playerId} friend request sent.");
            }
            catch (FriendsServiceException e)
            {
                Debug.Log($"Failed to add {playerId} - {e}.");
                m_RequestFriendView.AddFriendFailed();
            }
        }

        async Task RemoveFriend(string playerId)
        {
            try
            {
                await Friends.Instance.RemoveFriendAsync(playerId);
                Debug.Log($"{playerId} was removed from the friends list.");
            }
            catch (FriendsServiceException e)
            {
                Debug.Log($"Failed to remove {playerId}.");
                Debug.LogError(e);
            }
        }

        async Task BlockFriend(string playerId, string eventSource = null)
        {
            try
            {
                await Friends.Instance.BlockAsync(playerId, eventSource);
                Debug.Log($"{playerId} was blocked.");
            }
            catch (FriendsServiceException e)
            {
                Debug.Log($"Failed to block {playerId}.");
                Debug.LogError(e);
            }
        }

        async Task UnblockFriend(string playerId)
        {
            try
            {
                await Friends.Instance.UnblockAsync(playerId);
                Debug.Log($"{playerId} was unblocked.");
            }
            catch (FriendsServiceException e)
            {
                Debug.Log($"Failed to unblock {playerId}.");
                Debug.LogError(e);
            }
        }

        async Task AcceptRequest(string playerId)
        {
            try
            {
                await Friends.Instance.ConsentFriendRequestAsync(playerId);
                Debug.Log($"Friend request from {playerId} was accepted.");
            }
            catch (FriendsServiceException e)
            {
                Debug.Log($"Failed to accept request from {playerId}.");
                Debug.LogError(e);
            }
        }

        async Task DeclineRequest(string playerId)
        {
            try
            {
                await Friends.Instance.IgnoreFriendRequestAsync(playerId);
                Debug.Log($"Friend request from {playerId} was declined.");
            }
            catch (FriendsServiceException e)
            {
                Debug.Log($"Failed to decline request from {playerId}.");
                Debug.LogError(e);
            }
        }

        async Task<List<Player>> GetFriendsWithoutPresence()
        {
            try
            {
                var friends = await Friends.Instance.GetFriendsAsync(new PaginationOptions());
                return friends;
            }
            catch (FriendsServiceException e)
            {
                Debug.Log("Failed to retrieve the friend list.");
                Debug.LogError(e);
            }

            return null;
        }

        async Task<List<PlayerPresence<Activity>>> GetFriendsWithPresence()
        {
            try
            {
                var friends = await Friends.Instance.GetFriendsAsync<Activity>(new PaginationOptions());
                return friends;
            }
            catch (FriendsServiceException e)
            {
                Debug.Log("Failed to retrieve the friend list.");
                Debug.LogError(e);
            }

            return null;
        }

        async Task<List<Player>> GetRequests()
        {
            try
            {
                var requests = await Friends.Instance.GetInboxAsync(new PaginationOptions());
                return requests;
            }
            catch (FriendsServiceException e)
            {
                Debug.Log("Failed to retrieve the requests list.");
                Debug.LogError(e);
            }

            return null;
        }

        async Task<List<Player>> GetBlocks()
        {
            try
            {
                var requests = await Friends.Instance.GetBlocksAsync(new PaginationOptions());
                return requests;
            }
            catch (FriendsServiceException e)
            {
                Debug.Log("Failed to retrieve the blocks list.");
                Debug.LogError(e);
            }

            return null;
        }

        async Task SetPresence(PresenceAvailabilityOptions presenceAvailabilityOptions,
            string activityStatus = "")
        {
            var activity = new Activity { Status = activityStatus };
            var presence = new Presence<Activity>(presenceAvailabilityOptions, activity);

            try
            {
                await Friends.Instance.SetPresenceAsync(presence);
                Debug.Log($"Availability changed to {presence.GetAvailability()}.");
            }
            catch (FriendsServiceException e)
            {
                Debug.Log($"Failed to set the presence to {presence.GetAvailability()}");
                Debug.LogError(e);
            }
        }

        async Task SubscribeToFriendsEventCallbacks()
        {
            try
            {
                var callbacks = new FriendsEventCallbacks<Activity>();
                callbacks.FriendsEventConnectionStateChanged += async e =>
                {
                    await RefreshFriends();
                    Debug.Log("FriendsEventConnectionStateChanged EventReceived");
                };
                callbacks.FriendAdded += async e =>
                {
                    await RefreshRequests();
                    await RefreshFriends();
                    Debug.Log("FriendAdded EventReceived");
                };
                callbacks.FriendRequestReceived += async e =>
                {
                    await RefreshRequests();
                    Debug.Log("FriendRequestReceived EventReceived");
                };
                callbacks.Blocked += async e =>
                {
                    await RefreshBlocks();
                    Debug.Log("Blocked EventReceived");
                };
                callbacks.PresenceUpdated += async e =>
                {
                    await RefreshFriends();
                    Debug.Log("PresenceUpdated EventReceived");
                };
                callbacks.FriendRemoved += async e =>
                {
                    await RefreshFriends();
                    Debug.Log("FriendRemoved EventReceived");
                };
                await Friends.Instance.SubscribeToFriendsEventsAsync(callbacks);
            }
            catch (FriendsServiceException e)
            {
                Debug.Log(
                    "An error occurred while performing the action. Code: " + e.Reason + ", Message: " + e.Message);
            }
        }
    }
}