using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Friends;
using Unity.Services.Friends.Exceptions;
using Unity.Services.Friends.Models;
using Unity.Services.Friends.Options;
using UnityEngine;


namespace UnityGamingServicesUsesCases.Relationships
{
    public class RelationshipsSceneManager : MonoBehaviour
    {
        [Header("View's References")] [SerializeField]
        private AddFriendView _addFriendView = null;

        [SerializeField] private LogInView _logInView = null;
        [SerializeField] private PlayerInfoView _playerInfoView = null;
        [SerializeField] private FriendsView m_FriendsView = null;
        [SerializeField] private RequestsView m_RequestsView = null;
        [SerializeField] private BlocksView m_BlocksView = null;

        [Header("Debug")] [SerializeField] private PlayerProfilesData _playerProfilesData = null;

        private string _currentPlayerName;

        public async Task Init(string currentPlayerName)
        {
            //Bind Debug 
            _addFriendView.Init();
            _addFriendView.OnAddFriend += AddFriendAsync;
            _logInView.Init();
            _logInView.OnLogIn += LogIn;

            //Bind Views
            m_FriendsView.OnFriendRemove += RemoveFriendAsync;
            m_FriendsView.OnFriendBlock += BlockFriendAsync;
            m_RequestsView.OnRequestAccepted += AcceptRequestAsync;
            m_RequestsView.OnRequestDeclined += DeclineRequestAsync;
            m_BlocksView.OnFriendUnblock += UnblockFriendAsync;

            _currentPlayerName = currentPlayerName;
            await RefreshPlayerView();
        }

        private async void BlockFriendAsync(string id)
        {
            await BlockFriend(id);
            await RefreshPlayerView();
        }

        private async void UnblockFriendAsync(string id)
        {
            await UnblockFriend(id);
            await RefreshPlayerView();
        }

        private async void RemoveFriendAsync(string id)
        {
            await RemoveFriend(id);
            await RefreshPlayerView();
        }

        private async void AddFriendAsync(string id)
        {
            await AddFriend(id, "button");
        }

        private async void LogIn(string playerName)
        {
            await UASUtils.SwitchUser(playerName);
            _currentPlayerName = playerName;
            await RefreshPlayerView();
            Debug.Log($"Logged in as {playerName} id: {AuthenticationService.Instance.PlayerId}");
        }

        private async void AcceptRequestAsync(string id)
        {
            await AcceptRequest(id);
            await RefreshPlayerView();
        }

        private async void DeclineRequestAsync(string id)
        {
            await DeclineRequest(id);
            await RefreshPlayerView();
        }
        

        private async Task RefreshPlayerView()
        {
            _playerInfoView.Refresh(_currentPlayerName);
            
            //Friends
            var friends = await GetFriendsWithoutPresence();
            var friendProfiles = new List<PlayerProfile>();
            foreach (var friend in friends)
            {
                friendProfiles.Add(new PlayerProfile { Id = friend.Id, Name = _playerProfilesData.GetName(friend.Id) });
            }
            m_FriendsView.Refresh(friendProfiles);

            //Requests
            var requests = await GetRequests();
            var requestProfile = new List<PlayerProfile>();
            foreach (var request in requests)
            {
                requestProfile.Add(
                    new PlayerProfile { Id = request.Id, Name = _playerProfilesData.GetName(request.Id) });
            }
            m_RequestsView.Refresh(requestProfile);

            //Blocks
            var blocks = await GetBlocks();
            var blocksProfiles = new List<PlayerProfile>();
            foreach (var block in blocks)
            {
                blocksProfiles.Add(new PlayerProfile { Id = block.Id, Name = _playerProfilesData.GetName(block.Id) });
            }

            m_BlocksView.Refresh(blocksProfiles);
        }

        private async Task AddFriend(string playerId, string eventSource)
        {
            try
            {
                await Friends.Instance.AddFriendAsync(playerId, eventSource);
                Debug.Log($"{playerId} was added to the friends list.");
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
                Debug.Log($"{playerId} was removed to the friends list.");
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
            string activityStatus)
        {
            var activity = new Activity { Status = activityStatus };
            var presence = new Presence<Activity>(presenceAvailabilityOptions, activity);

            try
            {
                await Friends.Instance.SetPresenceAsync(presence);
            }
            catch (FriendsServiceException e)
            {
                Debug.Log($"Failed to set the presence to {presence.GetAvailability()}");
                Debug.LogError(e);
            }
        }
    }
}