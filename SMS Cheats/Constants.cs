namespace SMS_Cheats
{
    public static class Constants
    {
        public static class SearchKeys
        {
            public const string Money = "\"Money\"";
            public const string CurrentStoreLevel = "\"CurrentStoreLevel\"";
            public const string CurrentStorePoint = "\"CurrentStorePoint\"";
            public const string StoreUpgradeLevel = "\"StoreUpgradeLevel\"";
            public const string CompletedCheckoutCount = "\"CompletedCheckoutCount\"";
            public const string CurrentDay = "\"CurrentDay\"";
            public const string VehicleGasLevel = "\"GasLevel\"";
            public const string StoreName = "\"ShopName\"";
            public const string UnlockedLicenses = "\"UnlockedLicenses\"";
            // Loan fields only occur inside the LoanDatas array
            public const string LoanTermLength = "\"TermLength\"";
            public const string LoanRemainingPayments = "\"RemainingPayments\"";
            public const string LoanTaken = "\"Taken\"";
        }

        public static class GameValues
        {
            public const double MaxFuel = 200;

            // License IDs start at 21; IDs below that break license
            // progression on the in-game computer
            public const int FirstLicenseId = 21;
            public const int LastLicenseId = 47;
        }
    }
}
