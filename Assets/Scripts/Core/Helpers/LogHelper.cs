﻿namespace Core.Helpers
{
    public static class LogHelper
    {
        public static string Colored(this object message, string color) => $"<color={color}>{message}</color>";
        public static string Bold(this object message) => $"<b>{message}</b>";
        public static string Italic(this object message) => $"<i>{message}</i>";
    }
}
