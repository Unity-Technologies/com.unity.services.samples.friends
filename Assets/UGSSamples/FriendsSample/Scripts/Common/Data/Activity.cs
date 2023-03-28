using System.Runtime.Serialization;
using System.Text;
using UnityEngine.Scripting;

namespace Unity.Services.Samples.Friends
{
    /// <summary>
    /// Activity represent the activity containing the status of a player.
    /// </summary>
    [Preserve]
    [DataContract]
    [System.Serializable]
    public class Activity
    {

        public enum ActivityType
        {
            Menu,
            Party,
            Offline
        }
        /// <summary>
        /// Status of the player.
        /// </summary>
        [Preserve]
        [DataMember(Name = "status", IsRequired = true, EmitDefaultValue = true)]
        public string Status { get; set; }
        /// <summary>
        /// ActivityType of the player.
        /// </summary>
        [Preserve]
        [DataMember(Name = "activity", IsRequired = true, EmitDefaultValue = true)]
        public ActivityType m_ActivityType { get; set; }
        /// <summary>
        /// Activity Data of the player.
        /// </summary>
        [Preserve]
        [DataMember(Name = "data", IsRequired = false, EmitDefaultValue = false)]
        public string m_ActivityData { get; set; }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("ActivityData\n");
            sb.Append(Status);
            sb.Append(" - ");
            sb.AppendLine(m_ActivityType.ToString());
            sb.Append(" -  ");
            sb.AppendLine(m_ActivityData);
            return sb.ToString();
        }


        public void SetStatus(string status)
        {
            string activityTypeString = m_ActivityType.ToString();
            Status = $"({activityTypeString}) - {status}";
        }

    }
}
