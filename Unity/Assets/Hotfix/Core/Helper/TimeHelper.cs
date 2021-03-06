﻿using System;

namespace Hotfix
{
    internal static class TimeHelper
    {
        private static readonly long epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks;

        public static long Now => (DateTime.UtcNow.Ticks - epoch) / 10000;
    }
}