using System;
using System.Collections.Generic;
using System.Text;

namespace BrochureBuddy.Entities
{
    public class AdSettings
    {
        public int Id { get; set; }
        public float AdDuration { get; set; }
        public bool IgnoreAdDurationForVideos { get; set; }
    }
}