﻿using System.Collections.Generic;

namespace NewsSystem.Common
{
    public static class GlobalConstants
    {
        public const string SystemName = "| 078 . NEWS |";
        public const string SystemBaseUrl = "https://078.news/";
        public const string TempUrl = "https://078news.azurewebsites.net";

        public const string TitleAfter = SystemName + " " + SystemBaseUrl;

        public const string SystemSlogan = "Новините от Кюстендилска област.";

        public const string AdministratorRoleName = "Administrator";
        public const string EditorRoleName = "Editor";
        public const string ReporterRoleName = "Reporter";

        public const string DefaultUserAgent =
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/72.0.3626.119 Safari/537.36";

        public static string SystemVersion { get; set; }

        public static class imgSizes
        {
            public static List<string> MainNews =>new List<string>
            {
                "430x215",
                "350x175"
            };
        }

    //LOW: Add constants for ui text (Update, Delete and etc.) 



}

}
