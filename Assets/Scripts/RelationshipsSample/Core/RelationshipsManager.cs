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
        [Tooltip("Reference a GameObject that has a component extending from IRelationshipsUIController.")]
        [SerializeField]
        GameObject m_RelationshipsUIControllerGameObject;
        IRelationshipsUIController m_RelationshipsUIController;

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

        const int k_MAXAmountOfUsersPerRequest = 25;

        string LoggedPlayerId => AuthenticationService.Instance.PlayerId;

        public async Task Init(string currentPlayerName, ISocialProfileService profileService)
        {
            m_SocialProfileService = profileService;
            UIInit();

            m_LoggedPlayerName = currentPlayerName;
            m_LocalPlayerView.Refresh(m_LoggedPlayerName, LoggedPlayerId, "In Friends Menu",
                PresenceAvailabilityOptions.ONLINE);
            await SetPresence(PresenceAvailabilityOptions.ONLINE);
            await SubscribeToFriendsEventCallbacks();
            RefreshAll();
        }

        void UIInit()
        {
            if (m_RelationshipsUIControllerGameObject == null)
            {
                Debug.LogError($"Missing GameObject in {name}",gameObject);
                return;
            }

            m_RelationshipsUIController = m_RelationshipsUIControllerGameObject.GetComponent<IRelationshipsUIController>();
            if (m_RelationshipsUIController == null)
            {
                Debug.LogError($"No Component extending IRelationshipsUIController {m_RelationshipsUIControllerGameObject.name}",
                    m_RelationshipsUIControllerGameObject );
                return;
            }

            m_RelationshipsUIController.Init();
            m_LocalPlayerView = m_RelationshipsUIController.LocalPlayerView;
            m_AddFriendView = m_RelationshipsUIController.AddFriendView;

            //Bind Lists
            m_FriendsListView = m_RelationshipsUIController.FriendsListView;
            m_FriendsListView.BindList(m_FriendsEntryDatas);
            m_RequestListView = m_RelationshipsUIController.RequestListView;
            m_RequestListView.BindList(m_RequestsEntryDatas);
            m_BlockListView = m_RelationshipsUIController.BlockListView;
            m_BlockListView.BindList(m_BlockEntryDatas);

            //Bind Friends SDK Callbacks
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
            //Debug.Log($"Token ID{AuthenticationService.Instance.AccessToken}");
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
            RefreshAll();
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
            m_LocalPlayerView.Refresh(m_LoggedPlayerName, LoggedPlayerId, status.activity, status.presence);
        }

        async void AddFriendAsync(string id)
        {
            var success = await SendFriendRequest(id, "AddFriendView");
            if(success)
                m_AddFriendView.FriendRequestSuccess();
            else
                m_AddFriendView.FriendRequestFailed();
        }

        async Task RefreshFriends()
        {
            m_FriendsEntryDatas.Clear();
            var gotAllFriends = false;
            var friendsCount = 0;

            //Will continue until we've gotten all your friends, 25 at a time.
            while (!gotAllFriends)
            {
                var friends = await GetFriendsWithPresence(k_MAXAmountOfUsersPerRequest, friendsCount);
                if (friends == null)
                    return;
                AddFriends(friends);
            }

            void AddFriends(List<PlayerPresence<Activity>> friends)
            {

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
                m_RelationshipsUIController.RelationshipBarView.Refresh();
                friendsCount += friends.Count;
                if (friends.Count < 25)
                    gotAllFriends = true;
            }
        }


        async Task RefreshRequests()
        {
            m_RequestsEntryDatas.Clear();
            var gotAllRequests = false;
            var requestsCount = 0;
            //Will continue until we've gotten all your requests, 25 at a time.

            while (!gotAllRequests)
            {
                var requests = await GetRequests(k_MAXAmountOfUsersPerRequest, requestsCount);
                if (requests == null)
                    return;
                AddRequests(requests);
            }

            void AddRequests(List<Player> requests)
            {
                foreach (var request in requests)
                    m_RequestsEntryDatas.Add(new PlayerProfile(m_SocialProfileService.GetName(request.Id), request.Id));

                m_RelationshipsUIController.RelationshipBarView.Refresh();
                requestsCount += requests.Count;
                if (requests.Count < 25)
                    gotAllRequests = true;
            }
        }


        async Task RefreshBlocks()
        {
            m_BlockEntryDatas.Clear();
            var gotAllBlocks = false;
            var blocksCount = 0;
            //Will continue until we've gotten all your blocks, 25 at a time.
            while (!gotAllBlocks)
            {
                var blocks = await GetBlocks(k_MAXAmountOfUsersPerRequest, blocksCount);
                if (blocks == null)
                    return;
                AddBlocks(blocks);
            }

            void AddBlocks(List<Player> blocks)
            {
                foreach (var block in blocks)
                    m_BlockEntryDatas.Add(new PlayerProfile(m_SocialProfileService.GetName(block.Id), block.Id));

                m_RelationshipsUIController.RelationshipBarView.Refresh();
                blocksCount += blocks.Count;
                if (blocks.Count < 25)
                    gotAllBlocks = true;
            }
        }


        public async Task<bool> SendFriendRequest(string playerId, string eventSource)
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

        /// <summary>
        /// Currently the friends SDK can only ask for up to 25 users at a time, so to get a full list of users you need to call
        /// it multiple times with an offset.
        /// </summary>
        /// <param name="limit">How many users are we asking for.</param>
        /// <param name="offset">From where in my user list am i starting from.</param>
        /// <returns>List of friends presences.</returns>
        async Task<List<PlayerPresence<Activity>>> GetFriendsWithPresence(int limit, int offset)
        {
            var paginationOptions = new PaginationOptions()
            {
                Limit = Mathf.Clamp(limit, 1, k_MAXAmountOfUsersPerRequest), // 25 is the max the API allows to fetch at a time.
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

        /// <summary>
        /// Currently the friends SDK can only ask for up to 25 users at a time, so to get a full list of users you need to call
        /// it multiple times with an offset.
        /// </summary>
        /// <param name="limit">How many users are we asking for.</param>
        /// <param name="offset">From where in my user list am i starting from.</param>
        /// <returns>List of players.</returns>
        async Task<List<Player>> GetRequests(int limit, int offset)
        {
            var paginationOptions = new PaginationOptions()
            {
                Limit = Mathf.Clamp(limit, 1, k_MAXAmountOfUsersPerRequest), // 25 is the max the API allows to fetch at a time.
                Offset = offset
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

        /// <summary>
        /// Currently the friends SDK can only ask for up to 25 users at a time, so to get a full list of users you need to call
        /// it multiple times with an offset.
        /// </summary>
        /// <param name="limit">How many users are we asking for.</param>
        /// <param name="offset">From where in my user list am i starting from.</param>
        /// <returns>List of players.</returns>
        async Task<List<Player>> GetBlocks(int limit, int offset)
        {
            var paginationOptions = new PaginationOptions()
            {
                Limit = Mathf.Clamp(limit, 1, k_MAXAmountOfUsersPerRequest), // 25 is the max the API allows to fetch at a time.
                Offset = offset
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
