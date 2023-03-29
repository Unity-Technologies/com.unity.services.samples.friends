using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.Services.Friends;
using Unity.Services.Friends.Exceptions;
using Unity.Services.Friends.Models;
using UnityEngine;

namespace Unity.Services.Samples.Friends
{
    public class RelationshipsManager : MonoBehaviour
    {
        [Tooltip("Reference a GameObject that has a component extending from IRelationshipsUIController.")]
        [SerializeField]
        GameObject
            m_RelationshipsViewGameObject; //This gameObject reference is only needed to get the IRelationshipUIController component from it.
        IRelationshipsView m_RelationshipsView;

        List<FriendsEntryData> m_FriendsEntryDatas = new List<FriendsEntryData>();
        List<PlayerProfile> m_RequestsEntryDatas = new List<PlayerProfile>();
        List<PlayerProfile> m_BlockEntryDatas = new List<PlayerProfile>();

        ILocalPlayerView m_LocalPlayerView;
        IAddFriendView m_AddFriendView;
        IFriendsListView m_FriendsListView;
        IRequestListView m_RequestListView;
        IBlockedListView m_BlockListView;

        IPlayerProfileService m_SamplePlayerProfileService;
        PlayerProfile m_LocalPlayer;
        [SerializeField] PlayerAuthentication m_PlayerAuthentication;

        PresenceAvailabilityOptions m_LocalPresence = PresenceAvailabilityOptions.ONLINE;
        Activity m_LocalActivity = new Activity();

        async void Start()
        {
            //If you are using multiple unity services, make sure to initialize it only once before using your services.
            await Init();
        }

        async Task Init()
        {
            await m_PlayerAuthentication.SignIn();
            await FriendsService.Instance.InitializeAsync();
            UIInit();
            await LogInAsync();

            SubscribeToFriendsEventCallbacks();
            RefreshAll();
            InitParty();
        }

        void UIInit()
        {
            if (m_RelationshipsViewGameObject == null)
            {
                Debug.LogError($"Missing GameObject in {name}", gameObject);
                return;
            }

            m_RelationshipsView = m_RelationshipsViewGameObject.GetComponent<IRelationshipsView>();
            if (m_RelationshipsView == null)
            {
                Debug.LogError($"No Component extending IRelationshipsView {m_RelationshipsViewGameObject.name}",
                    m_RelationshipsViewGameObject);
                return;
            }

            m_RelationshipsView.Init();
            m_LocalPlayerView = m_RelationshipsView.LocalPlayerView;
            m_AddFriendView = m_RelationshipsView.AddFriendView;

            //Bind Lists
            m_FriendsListView = m_RelationshipsView.FriendsListView;
            m_FriendsListView.BindList(m_FriendsEntryDatas);
            m_RequestListView = m_RelationshipsView.RequestListView;
            m_RequestListView.BindList(m_RequestsEntryDatas);
            m_BlockListView = m_RelationshipsView.BlockListView;
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

        async Task LogInAsync()
        {
            m_SamplePlayerProfileService = new SamplePlayerProfileService();
            m_LocalPlayer = m_PlayerAuthentication.LocalPlayer;

            m_LocalActivity.m_ActivityType = Activity.ActivityType.Menu;
            m_LocalActivity.SetStatus("Chilling.");
            m_LocalPresence = PresenceAvailabilityOptions.ONLINE;

            await PushLocalPresence();
            m_LocalPlayerView.Refresh(m_LocalPlayer.Name, m_LocalPlayer.Id, "In Friends Menu",
                PresenceAvailabilityOptions.ONLINE);
            Debug.Log($"Logged in as {m_LocalPlayer}");
        }

        /// <summary>
        /// Interoperable Setup for the Parties Sample
        /// Enables Inviting and Joining Friends.
        /// </summary>
        void InitParty()
        {
#if LOBBY_SDK_AVAILABLE
           FriendsPartyManager partyManager = new FriendsPartyManager(m_LocalPlayer);

            m_FriendsListView.onInviteToParty += (friendID) =>
            {
                partyManager.SendPartyInvite(friendID, m_LocalActivity.m_ActivityData);
            };

            m_FriendsListView.onJoinFriendParty += (friendPartyCode) =>
            {
                partyManager.TryJoinParty(friendPartyCode);
            };

            LobbyEvents.OnLobbyJoined += async (partyCode) =>
            {
                m_LocalActivity.m_ActivityType = Activity.ActivityType.Party;
                m_LocalActivity.m_ActivityData = partyCode;
                await PushLocalPresence();
                RefreshFriends();
            };

            LobbyEvents.OnLobbyLeft += async () =>
            {
                m_LocalActivity.m_ActivityType = Activity.ActivityType.Menu;
                m_LocalActivity.m_ActivityData = "";
                await PushLocalPresence();
                RefreshFriends();
            };
#endif
        }

        void RefreshAll()
        {
            RefreshFriends();
            RefreshRequests();
            RefreshBlocks();
        }

        async void BlockFriendAsync(string id)
        {
            await BlockFriend(id);
            RefreshAll();
        }

        async void UnblockFriendAsync(string id)
        {
            await UnblockFriend(id);
            RefreshBlocks();
            RefreshFriends();
        }

        async void RemoveFriendAsync(string id)
        {
            await RemoveFriend(id);
            RefreshFriends();
        }

        async void AcceptRequestAsync(string id)
        {
            await AcceptRequest(id);
            RefreshRequests();
            RefreshFriends();
        }

        async void DeclineRequestAsync(string id)
        {
            await DeclineRequest(id);
            RefreshRequests();
        }

        async void SetPresenceAsync((PresenceAvailabilityOptions availability, string status) presence)
        {
            m_LocalPresence = presence.availability;
            m_LocalActivity.SetStatus(presence.status);

            await PushLocalPresence();
            m_LocalPlayerView.Refresh(m_LocalPlayer.Name, m_LocalPlayer.Id, presence.status,
                presence.availability);
        }

        async void AddFriendAsync(string id)
        {
            var success = await SendFriendRequest(id);
            if (success)
                m_AddFriendView.FriendRequestSuccess();
            else
                m_AddFriendView.FriendRequestFailed();
        }

        void RefreshFriends()
        {
            m_FriendsEntryDatas.Clear();

            var friends = GetFriends();

            foreach (var friend in friends)
            {
                Activity friendsActivity = new Activity();
                if (friend.Presence.Availability == PresenceAvailabilityOptions.OFFLINE ||
                    friend.Presence.Availability == PresenceAvailabilityOptions.INVISIBLE)
                {
                    friendsActivity.Status = friend.Presence.LastSeen.ToShortDateString() + " " +
                        friend.Presence.LastSeen.ToLongTimeString();
                    friendsActivity.m_ActivityType = Activity.ActivityType.Offline;
                }
                else
                {
                    try
                    {
                        friendsActivity = friend.Presence.GetActivity<Activity>();
                    }
                    catch (Exception ex)
                    {
                        Debug.LogWarning($"{friend.Id} had no activity Set!\n {ex}");
                    }
                }

                var info = new FriendsEntryData
                {
                    Name = friend.Profile.Name,
                    Id = friend.Id,
                    Availability = friend.Presence.Availability,
                    Activity = friendsActivity
                };
                m_FriendsEntryDatas.Add(info);
            }

            m_RelationshipsView.RelationshipBarView.Refresh();
        }

        void RefreshRequests()
        {
            m_RequestsEntryDatas.Clear();
            var requests = GetRequests();

            foreach (var request in requests)
            {
                m_RequestsEntryDatas.Add(
                    new PlayerProfile(m_SamplePlayerProfileService.GetName(request.Id), request.Id));
            }

            m_RelationshipsView.RelationshipBarView.Refresh();
        }

        void RefreshBlocks()
        {
            m_BlockEntryDatas.Clear();

            foreach (var block in FriendsService.Instance.Blocks)
            {
                m_BlockEntryDatas.Add(new PlayerProfile(m_SamplePlayerProfileService.GetName(block.Member.Id),
                    block.Member.Id));
            }

            m_RelationshipsView.RelationshipBarView.Refresh();
        }

        async Task<bool> SendFriendRequest(string playerId)
        {
            try
            {
                var relationship = await FriendsService.Instance.AddFriendAsync(playerId);
                Debug.Log($"Friend request sent to {playerId}.");
                return relationship.Type == RelationshipType.FRIEND_REQUEST;
            }
            catch (RelationshipsServiceException e)
            {
                Debug.Log($"Failed to Request {playerId} - {e}.");
                return false;
            }
        }

        async Task RemoveFriend(string playerId)
        {
            try
            {
                await FriendsService.Instance.DeleteFriendAsync(playerId);
                Debug.Log($"{playerId} was removed from the friends list.");
            }
            catch (RelationshipsServiceException e)
            {
                Debug.Log($"Failed to remove {playerId}.");
                Debug.LogError(e);
            }
        }

        async Task BlockFriend(string playerId)
        {
            try
            {
                await FriendsService.Instance.AddBlockAsync(playerId);
                Debug.Log($"{playerId} was blocked.");
            }
            catch (RelationshipsServiceException e)
            {
                Debug.Log($"Failed to block {playerId}.");
                Debug.LogError(e);
            }
        }

        async Task UnblockFriend(string playerId)
        {
            try
            {
                await FriendsService.Instance.DeleteBlockAsync(playerId);
                Debug.Log($"{playerId} was unblocked.");
            }
            catch (RelationshipsServiceException e)
            {
                Debug.Log($"Failed to unblock {playerId}.");
                Debug.LogError(e);
            }
        }

        async Task AcceptRequest(string playerId)
        {
            try
            {
                await SendFriendRequest(playerId);
                Debug.Log($"Friend request from {playerId} was accepted.");
            }
            catch (RelationshipsServiceException e)
            {
                Debug.Log($"Failed to accept request from {playerId}.");
                Debug.LogError(e);
            }
        }

        async Task DeclineRequest(string playerId)
        {
            try
            {
                await FriendsService.Instance.DeleteIncomingFriendRequestAsync(playerId);
                Debug.Log($"Friend request from {playerId} was declined.");
            }
            catch (RelationshipsServiceException e)
            {
                Debug.Log($"Failed to decline request from {playerId}.");
                Debug.LogError(e);
            }
        }

        /// <summary>
        /// Get an amount of friends (including presence data).
        /// </summary>
        /// <returns>List of friends.</returns>
        List<Member> GetFriends()
        {
            return GetNonBlockedMembers(FriendsService.Instance.Friends);
        }

        /// <summary>
        /// Get an amount of Requests. The friends SDK maintains relationships unless explicitly deleted, even those
        /// towards blocked players. We don't want to show blocked players' requests, so we filter them out.
        /// </summary>
        /// <returns>List of players.</returns>
        List<Member> GetRequests()
        {
            return GetNonBlockedMembers(FriendsService.Instance.IncomingFriendRequests);
        }

        async Task PushLocalPresence()
        {
            try
            {
                await FriendsService.Instance.SetPresenceAsync(m_LocalPresence, m_LocalActivity);
                Debug.Log($"Presence Pushed {m_LocalPresence} - {m_LocalActivity}");
            }
            catch (RelationshipsServiceException e)
            {
                Debug.LogError(e);
            }
        }

        void SubscribeToFriendsEventCallbacks()
        {
            try
            {
                FriendsService.Instance.RelationshipAdded += e =>
                {
                    RefreshRequests();
                    RefreshFriends();
                    Debug.Log($"create {e.Relationship} EventReceived");
                };
                FriendsService.Instance.MessageReceived += e =>
                {
                    RefreshRequests();
                    Debug.Log("MessageReceived EventReceived");
                };

                FriendsService.Instance.PresenceUpdated += e =>
                {
                    RefreshFriends();
                    Debug.Log("PresenceUpdated EventReceived");
                };
                FriendsService.Instance.RelationshipDeleted += e =>
                {
                    RefreshFriends();
                    Debug.Log($"Delete {e.Relationship} EventReceived");
                };
            }
            catch (RelationshipsServiceException e)
            {
                Debug.Log(
                    "An error occurred while performing the action. Code: " + e.Reason + ", Message: " + e.Message);
            }
        }

        /// <summary>
        /// Returns a list of members that are not blocked by the active user.
        /// </summary>
        /// <param name="relationships">The list of relationships to filter.</param>
        /// <returns>Filtered list of members.</returns>
        List<Member> GetNonBlockedMembers(IReadOnlyList<Relationship> relationships)
        {
            var blocks = FriendsService.Instance.Blocks;
            return relationships
                .Where(relationship =>
                    !blocks.Any(blockedRelationship => blockedRelationship.Member.Id == relationship.Member.Id))
                .Select(relationship => relationship.Member)
                .ToList();
        }
    }
}