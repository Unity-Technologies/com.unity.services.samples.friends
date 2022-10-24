using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Friends;
using Unity.Services.Friends.Exceptions;
using Unity.Services.Friends.Models;
using Unity.Services.Friends.Notifications;
using Unity.Services.Friends.Options;
using UnityEngine;

namespace UnityGamingServicesUsesCases.Relationships
{
    public class RelationshipsManager : MonoBehaviour
    {
        [Header("UI GameObject"), Tooltip("Put in a GameObject with a MonoBehaviour extending IRelationshipsUIController.")]
        [SerializeField]
        GameObject m_UIControllerObject;
        IRelationshipsUIController m_UIController;

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
        IAddFriendView m_AddFriendView;
        IFriendsListView m_FriendsListView;
        IRequestListView m_RequestListView;
        IBlockedListView m_BlockListView;
        ISocialProfileService m_SocialProfileService;


        string LoggedPlayerId => AuthenticationService.Instance.PlayerId;

        public async Task Init(string currentPlayerName, ISocialProfileService profileService)
        {
            m_SocialProfileService = profileService;
            UISetup();

            m_LoggedPlayerName = currentPlayerName;
            m_LocalPlayerView.Refresh(m_LoggedPlayerName, LoggedPlayerId, "In Friends Menu",
                PresenceAvailabilityOptions.ONLINE);
            await SetPresence(PresenceAvailabilityOptions.ONLINE);
            await SubscribeToFriendsEventCallbacks();
        }

        void UISetup()
        {
            //Do you want to do null checks?
            if (m_UIControllerObject == null)
            {
                Debug.LogError("No GameObject in m_UIController");
                return;
            }

            m_UIController = m_UIControllerObject.GetComponent<IRelationshipsUIController>();
            if (m_UIController == null)
            {
                Debug.LogError($"No Component extending IRelationshipsUIController on {m_UIControllerObject.name}");
                return;
            }

            m_UIController.Init();
            m_LocalPlayerView = m_UIController.LocalPlayerView;
            m_AddFriendView = m_UIController.AddFriendView;

            //Bind Entry Lists
            m_FriendsListView = m_UIController.FriendsListView;
            m_FriendsListView.BindList(m_FriendsEntryDatas);
            m_RequestListView = m_UIController.RequestListView;
            m_RequestListView.BindList(m_RequestsEntryDatas);
            m_BlockListView = m_UIController.BlockListView;
            m_BlockListView.BindList(m_BlockEntryDatas);

            //Bind Friend Calls
            m_AddFriendView.onFriendRequestSent += AddFriendAsync;
            m_FriendsListView.onRemove += RemoveFriendAsync;
            m_FriendsListView.onBlock += BlockFriendAsync;
            m_RequestListView.onAccept += AcceptRequestAsync;
            m_RequestListView.onDecline += DeclineRequestAsync;
            m_RequestListView.onBlock += BlockFriendAsync;
            m_BlockListView.onUnblock += UnblockFriendAsync;
            m_LocalPlayerView.onPresenceChanged += SetPresenceAsync;
        }


        public async void LogIn(string playerName)
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

        public async void RefreshAll()
        {
            await RefreshFriends();
            await RefreshRequests();
            await RefreshBlocks();
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

        async void SetPresenceAsync((PresenceAvailabilityOptions presence, string activity) status)
        {
            await SetPresence(status.presence, status.activity);
            m_LocalPlayerView.Refresh(m_LoggedPlayerName, LoggedPlayerId, status.activity,
                status.presence);
        }

        async void AddFriendAsync(string id)
        {
            var success = await RequestFriend(id, "button");
            if (success)
                m_AddFriendView.FriendRequestSuccess(); // Make Into Task.
            else
                m_AddFriendView.FriendRequestFailed();
        }

        async Task RefreshFriends()
        {
            m_FriendsEntryDatas.Clear();
            bool gotAllFriends = false;
            int friendsCount = 0;
            //Will continue until we've gotten all your friends, 25 at a time.
            while (!gotAllFriends)
            {
                var friends = await GetFriendsWithPresence(25, friendsCount);
                if (friends == null)
                    return;
                friendsCount += friends.Count;

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
                        Name = m_SocialProfileService.GetName(friend.Player.Id),
                        Id = friend.Player.Id,
                        Availability = friend.Presence.GetAvailability(),
                        Activity = activityText
                    };
                    m_FriendsEntryDatas.Add(info);
                }
                m_FriendsListView.Refresh();

                if (friends.Count < 25)
                    gotAllFriends = true;
            }
        }


        async Task RefreshRequests()
        {
            m_RequestsEntryDatas.Clear();
            bool gotAllRequests = false;
            int requestsCount = 0;
            //Will continue until we've gotten all your requests, 25 at a time.

            while (!gotAllRequests)
            {
                var requests = await GetRequests(25, requestsCount);
                if (requests == null)
                    return;
                requestsCount += requests.Count;
                foreach (var request in requests)
                {
                    m_RequestsEntryDatas.Add(new PlayerProfile(m_SocialProfileService.GetName(request.Id), request.Id));
                }

                m_RequestListView.Refresh();

                if (requests.Count < 25)
                    gotAllRequests = true;
            }
        }


        async Task RefreshBlocks()
        {
            m_BlockEntryDatas.Clear();
            bool gotAllBlocks = false;
            int blocksCount = 0;
            //Will continue until we've gotten all your blocks, 25 at a time.
            while (!gotAllBlocks)
            {
                var blocks = await GetBlocks(25, blocksCount);
                if (blocks == null)
                    return;
                blocksCount += blocks.Count;
                foreach (var block in blocks)
                {
                    m_BlockEntryDatas.Add(new PlayerProfile(m_SocialProfileService.GetName(block.Id), block.Id));
                }

                m_BlockListView.Refresh();

                if (blocks.Count < 25)
                    gotAllBlocks = true;
            }
        }


        public async Task<bool> RequestFriend(string playerId, string eventSource)
        {
            try
            {
                await Friends.Instance.AddFriendAsync(playerId, eventSource);
                Debug.Log($"{playerId} friend request sent.");
                return true;
            }
            catch (FriendsServiceException e)
            {
                Debug.Log($"Failed to add {playerId} - {e}.");
                return false;
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

        async Task<List<PlayerPresence<Activity>>> GetFriendsWithPresence(int limit, int offset)
        {
            var paginationOptions = new PaginationOptions()
            {
                Limit = limit, // 25 is the max the API allows to fetch at a time.
                Offset = offset
            };
            try
            {
                var friends = await Friends.Instance.GetFriendsAsync<Activity>(paginationOptions);
                return friends;
            }
            catch (FriendsServiceException e)
            {
                Debug.Log("Failed to retrieve the friend list.");
                Debug.LogError(e);
            }

            return null;
        }

        async Task<List<Player>> GetRequests(int limit, int fromOffset)
        {
            var paginationOptions = new PaginationOptions()
            {
                Limit = limit,
                Offset = fromOffset
            };
            try
            {
                var requests = await Friends.Instance.GetInboxAsync(paginationOptions);
                return requests;
            }
            catch (FriendsServiceException e)
            {
                Debug.Log("Failed to retrieve the requests list.");
                Debug.LogError(e);
            }

            return null;
        }

        async Task<List<Player>> GetBlocks(int limit, int fromOffset)
        {
            var paginationOptions = new PaginationOptions()
            {
                Limit = limit,
                Offset = fromOffset
            };
            try
            {
                var requests = await Friends.Instance.GetBlocksAsync(paginationOptions);
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
