using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GOGDotNet.Models
{
    public class GogGameResponse
    {
        public int pages { get; set; }
        public GogEmbedded _embedded { get; set; }
    }

    public class GogEmbedded
    {
        public List<GogItem> items { get; set; }
    }

    public class GogItem
    {
        public GogGame game { get; set; }

        [JsonConverter(typeof(GogStatsConverter))]
        public Dictionary<string, GogStats> stats { get; set; }
    }

    public class GogGame
    {
        public string image { get; set; }
        public string id { get; set; }
        public string title { get; set; }
        public string url { get; set; }
        public bool? achievementSupport { get; set; }
    }

    public class GogStats
    {
        public DateTimeOffset? lastSession { get; set; }
        public uint? achievementsPercentage { get; set; }
        public uint? playtime { get; set; }
    }
}
