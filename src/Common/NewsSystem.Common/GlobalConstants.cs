namespace NewsSystem.Common
{
    public static class GlobalConstants
    {
        public const string SystemName = "0722.news";
        public const string SystemBaseUrl = "https://0722.news";

        public const string SystemSlogan = "Истината за Самоков и региона!";

        public const string AdministratorRoleName = "Administrator";
        public const string EditorRoleName = "Editor";
        public const string ReporterRoleName = "Reporter";

        public const string DefaultUserAgent =
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/72.0.3626.119 Safari/537.36";

        public static string SystemVersion { get; set; }

        //LOW: Add constants for ui text (Update, Delete and etc.) 
    }
}
