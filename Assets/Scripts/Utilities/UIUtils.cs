using Unity.Services.Friends.Models;
using UnityEngine;

namespace UnityGamingServicesUsesCases.Relationships.UIToolkit
{
    public class UIUtils
    {
        public static Color GetPresenceColor(PresenceAvailabilityOptions presenceStatus)
        {
            var colorIndex = Mathf.Clamp((int)presenceStatus - 1, 0, s_PresenceUIColors.Length-1);
            return s_PresenceUIColors[colorIndex];
        }

        //Mapping of colors to PresenceAvailabilityOptions
        static Color[] s_PresenceUIColors =
        {
            new Color(.1f, .8f, .1f), //ONLINE
            new Color(.8f, .7f, .2f), //BUSY
            new Color(.7f, .2f, .1f), //AWAY
            new Color(.4f, .1f, .6f), //INVISIBLE
            new Color(.4f, .4f, .4f), //OFFLINE
            new Color(1f, .4f, 1f) //UNKNOWN
        };
    }
}