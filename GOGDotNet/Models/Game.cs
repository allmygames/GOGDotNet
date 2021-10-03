using System;

namespace GOGDotNet.Models
{
    public class Game
    {
        public ulong Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public bool AchievementSupport { get; set; }
        public uint? AchievementsPercentage { get; set; }
        public uint? Playtime { get; set; }
        public DateTimeOffset? LastSession { get; set; }
        public string Image { get; set; }
    }
}
