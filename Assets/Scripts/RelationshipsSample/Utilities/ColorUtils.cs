using Unity.Services.Friends.Models;
using UnityEngine;

namespace UnityGamingServicesUsesCases.Relationships
{
    public class ColorUtils
    {
        public static Color GetPresenceColor(PresenceAvailabilityOptions presenceStatus)
        {
            var colorIndex = Mathf.Clamp((int)presenceStatus - 1, 0, s_PresenceUIColors.Length - 1);
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

        public static Color DefaultNavBarTabColor => ColorUtility.TryParseHtmlString("#554DB1", out var color) ? color : Color.black;
        public static Color DefaultNavBarIconColor => Color.white;
        public static Color SelectedNavBarTabColor => ColorUtility.TryParseHtmlString("#443D8F", out var color) ? color : Color.black;
        public static Color SelectedNavBarIconColor => ColorUtility.TryParseHtmlString("#F6D175", out var color) ? color : Color.white;
    }
}