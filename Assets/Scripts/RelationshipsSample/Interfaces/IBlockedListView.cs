using System;
using System.Collections.Generic;


namespace Unity.Services.Toolkits.Friends
{
    public interface IBlockedListView : IListView
    {
        Action<string> onUnblock { get; set; }
        void BindList(List<PlayerProfile> playerProfiles);
    }

}

