using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Runtime.Serialization;

namespace TaskList
{
    [JsonObject(MemberSerialization.OptOut)]
    public class Objective : ICloneable
    {
        public enum ObjectiveType
        {
            [EnumMember(Value = "Homework")]
            HOMEWORK,
            [EnumMember(Value = "Project")]
            PROJECT_IDEA,
            [EnumMember(Value = "Club")]
            CLUB,
            [EnumMember(Value = "Personal")]
            PERSONAL,
            [EnumMember(Value = "Free Time")]
            FREE_TIME,
            [EnumMember(Value = "Job")]
            JOB,
            [EnumMember(Value = "Note")]
            NOTE,
            [EnumMember(Value = "Reminder")]
            REMINDER,
            [EnumMember(Value = "Other")]
            OTHER,
            ALL
        }

        public enum Priority
        {
            [EnumMember(Value = "None")]
            NONE,
            [EnumMember(Value = "Low")]
            LOW,
            [EnumMember(Value = "Medium")]
            MEDIUM,
            [EnumMember(Value = "High")]
            HIGH,
            [EnumMember(Value = "Critical")]
            CRITICAL,
            ALL
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public ObjectiveType Type { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public Priority Importance { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DueBy { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastEdited { get; set; }
        public bool Persist { get; set; }
        public bool Repeat { get; set; }

        public object Clone() => MemberwiseClone();
    }
}
