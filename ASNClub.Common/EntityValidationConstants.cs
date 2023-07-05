namespace ASNClub.Common
{
    public static class EntityValidationConstants
    {
        public static class Comment
        {
            public const int TextMaxLength = 2;
            public const int TextMinLength = 1000;
        }
        public static class Adress
        {
            public const int CityMinLength = 2;
            public const int CityMaxLength = 50;

            public const int StreetMinLength = 2;
            public const int StreetMaxLength = 50;
        }
    }
}
