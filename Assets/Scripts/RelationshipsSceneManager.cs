using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Friends;
using Unity.Services.Friends.Exceptions;
using Unity.Services.Friends.Models;
using Unity.Services.Friends.Notifications;
using Unity.Services.Friends.Options;
using UnityEngine;
using UnityEngine.UI;

namespace UnityGamingServicesUsesCases.Relationships
{
    public class RelationshipsSceneManager : MonoBehaviour
    {
        [Header("View's References")] 
        [SerializeField] private AddFriendView m_AddFriendView;
        [SerializeField] private AddFriendByIdView m_AddFriendByIdView;
        [SerializeField] private LogInView m_LogInView;
        [SerializeField] private PlayerInfoView m_PlayerInfoView;
        [SerializeField] private RefreshView m_RefreshView;
        [SerializeField] private FriendsView m_FriendsView;
        [SerializeField] private RequestsView m_RequestsView;
        [SerializeField] private BlocksView m_BlocksView;
        [SerializeField] private Button m_QuitButton;

        [Header("Data")]
        [SerializeField] private PlayerProfilesData m_PlayerProfilesData = null;

        private string m_LoggedPlayerName;
        private string LoggedPlayerId => AuthenticationService.Instance.PlayerId;
        
        public async Task Init(string currentPlayerName)
        {
            //Bind Actionable 
            m_AddFriendView.Init();
            m_AddFriendView.OnAddFriend += AddFriendAsync;
            m_AddFriendByIdView.Init();
            m_AddFriendByIdView.OnAddFriend += AddFriendAsync;
            m_LogInView.Init();
            m_LogInView.OnLogIn += LogIn;
            m_RefreshView.Init();
            m_RefreshView.OnRefresh += RefreshAsync;
            m_QuitButton.onClick.AddListener(QuitAsync);

            //Bind Views
            m_FriendsView.OnFriendRemove += RemoveFriendAsync;
            m_FriendsView.OnFriendBlock += BlockFriendAsync;
            m_RequestsView.OnRequestAccepted += AcceptRequestAsync;
            m_RequestsView.OnRequestDeclined += DeclineRequestAsync;
            m_BlocksView.OnFriendUnblock += UnblockFriendAsync;
            m_PlayerInfoView.OnPresenceChanged += SetPresenceAsync;

            m_LoggedPlayerName = currentPlayerName;
            m_PlayerInfoView.Init(m_LoggedPlayerName, LoggedPlayerId, PresenceAvailabilityOptions.ONLINE);
            await SetPresence(PresenceAvailabilityOptions.ONLINE);
            await RefreshViews();
            await SubscribeToFriendsEventCallbacks();
        }
        
        private async void BlockFriendAsync(string id)
        {
            await BlockFriend(id);
            await RefreshViews();
        }

        private async void UnblockFriendAsync(string id)
        {
            await UnblockFriend(id);
            await RefreshViews();
        }

        private async void RemoveFriendAsync(string id)
        {
            await RemoveFriend(id);
            await RefreshViews();
        }

        private async void AddFriendAsync(string id)
        {
            await AddFriend(id, "button");
        }

        private async void LogIn(string playerName)
        {
            await UASUtils.SwitchUser(playerName);
            m_LoggedPlayerName = playerName;
            await SetPresence(PresenceAvailabilityOptions.ONLINE);
            m_PlayerInfoView.Init(m_LoggedPlayerName, LoggedPlayerId, PresenceAvailabilityOptions.ONLINE);
            await RefreshViews();
            Debug.Log($"Logged in as {playerName} id: {LoggedPlayerId}");
            Debug.Log($"Token ID{AuthenticationService.Instance.AccessToken}");
        }

        private async void AcceptRequestAsync(string id)
        {
            await AcceptRequest(id);
            await RefreshViews();
        }

        private async void DeclineRequestAsync(string id)
        {
            await DeclineRequest(id);
            await RefreshViews();
        }

        private async void RefreshAsync()
        {
            await RefreshViews();
        }
        
        private async void QuitAsync()
        {
            Friends.Instance.Dispose();
            await Task.Delay(1000);
// #if !UNITY_EDITOR
//             PlayerPrefs.DeleteAll();
// #endif
            Application.Quit();
        }

        private async void SetPresenceAsync((PresenceAvailabilityOptions presence, string activity) status)
        {
            await SetPresence(status.presence, status.activity);
        }

        private async Task RefreshViews()
        {
            //Friends
            var friends = await GetFriendsWithPresence();
            var infos = new List<FriendsEntryData>();
            foreach (var friend in friends)
            {
                string availabilityText;
                if (friend.Presence.GetAvailability() == PresenceAvailabilityOptions.OFFLINE ||
                    friend.Presence.GetAvailability() == PresenceAvailabilityOptions.INVISIBLE)
                {
                    availabilityText = friend.LastSeen.ToShortDateString() + " " + friend.LastSeen.ToLongTimeString();
                }
                else
                {
                    availabilityText = friend.Presence.GetAvailability().ToString();
                }

                var info = new FriendsEntryData
                {
                    Name = m_PlayerProfilesData.GetName(friend.Player.Id),
                    Id = friend.Player.Id,
                    Presence = availabilityText,
                    Activity = friend.Presence.GetActivity() == null? "": friend.Presence.GetActivity().Status
                };
                infos.Add(info);
            }

            m_FriendsView.Refresh(infos);

            //Requests
            var requests = await GetRequests();
            var requestsProfile = new List<PlayerProfile>();
            foreach (var request in requests)
            {
                requestsProfile.Add(new PlayerProfile(m_PlayerProfilesData.GetName(request.Id),request.Id));
            }

            m_RequestsView.Refresh(requestsProfile);

            //Blocks
            var blocks = await GetBlocks();
            var blocksProfiles = new List<PlayerProfile>();
            foreach (var block in blocks)
            {
                blocksProfiles.Add(new PlayerProfile( m_PlayerProfilesData.GetName(block.Id),block.Id));
            }

            m_BlocksView.Refresh(blocksProfiles);
        }

        private async Task AddFriend(string playerId, string eventSource)
        {
            try
            {
                await Friends.Instance.AddFriendAsync(playerId, eventSource);
                Debug.Log($"{playerId} friend request sent.");
            }
            catch (FriendsServiceException e)
            {
                Debug.Log($"Failed to add {playerId}.");
                Debug.LogError(e);
            }
        }

        private async Task RemoveFriend(string playerId)
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

        private async Task BlockFriend(string playerId, string eventSource = null)
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

        private async Task UnblockFriend(string playerId)
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

        private async Task AcceptRequest(string playerId)
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

        private async Task DeclineRequest(string playerId)
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

        private async Task<List<Player>> GetFriendsWithoutPresence()
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

        private async Task<List<PlayerPresence<Activity>>> GetFriendsWithPresence()
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

        private async Task<List<Player>> GetRequests()
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

        private async Task<List<Player>> GetBlocks()
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

        private async Task SetPresence(PresenceAvailabilityOptions presenceAvailabilityOptions,
            string activityStatus = "")
        {
            var activity = new Activity { Status = activityStatus };
            var presence = new Presence<Activity>(presenceAvailabilityOptions, activity);

            try
            {
                await Friends.Instance.SetPresenceAsync(presence);
                Debug.Log($"Presence changed to {presence.GetAvailability()}.");
            }
            catch (FriendsServiceException e)
            {
                Debug.Log($"Failed to set the presence to {presence.GetAvailability()}");
                Debug.LogError(e);
            }
        }
        
        private async Task SubscribeToFriendsEventCallbacks()
        {
            try
            {
                var callbacks = new FriendsEventCallbacks<Activity>();
                callbacks.FriendsEventConnectionStateChanged += (e) =>
                {
                    //RefreshAsync();
                    //Debug.Log($"error {e}");
                };
                callbacks.FriendAdded += (e) =>
                {
                    RefreshAsync();
                    Debug.Log("FriendAdded EventReceived");
                };
                callbacks.FriendRequestReceived += (e) =>
                {
                    RefreshAsync();
                    Debug.Log("FriendRequestReceived EventReceived");
                };
                callbacks.Blocked += (e) =>
                {
                    RefreshAsync();
                    Debug.Log("Blocked EventReceived");
                };
                callbacks.PresenceUpdated += (e) =>
                {
                    RefreshAsync();
                    Debug.Log("PresenceUpdated EventReceived");
                };
                callbacks.FriendRemoved += (e) =>
                {
                    RefreshAsync();
                    Debug.Log("FriendRemoved EventReceived");
                };
                await Friends.Instance.SubscribeToFriendsEventsAsync(callbacks);
            }
            catch (FriendsServiceException e)
            {
                Debug.Log("An error occurred while performing the action. Code: " + e.Reason + ", Message: " +     e.Message);
            }
        }
        
    }
}