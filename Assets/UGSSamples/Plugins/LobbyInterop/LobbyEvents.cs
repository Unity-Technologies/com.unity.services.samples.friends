using System;

namespace Unity.Services.Samples
{
    public class LobbyEvents
    {
        /// <summary>
        /// A Player requests to join a Lobby from the friends System.
        /// (Friends Should Invoke, Lobby should listen)
        /// </summary>
        public static Action<string> RequestJoinLobby;

        /// <summary>
        /// A player joined a Lobby from the Lobby System.
        /// (Lobby Should Invoke, Friends should listen)
        /// </summary>
        public static Action<string> OnLobbyJoined;

        public static Action OnLobbyLeft;
    }

}
