namespace ASNClub.Common
{
    public static class EntityValidationConstants
    {
        public static class Comment
        {
            public const int TextMinLength = 2;
            public const int TextMaxLength = 1000;
        }
        public static class Adress
        {
            public const int CityMinLength = 2;
            public const int CityMaxLength = 50;

            public const int StreetMinLength = 2;
            public const int StreetMaxLength = 50;
        }
        public static class ShoppingCartItem
        {
            public const int QuantityMinCount = 1;
            public const int QuantityMaxCount = 100;
        }
        public static class User
        {
            public const int FirstNameMinLength = 1;
            public const int FirstNameMaxLength = 16;

            public const int SurNameMinLength = 1;
            public const int SurNameMaxLength = 16;
        }
    }
}
