namespace Make_a_Drop.Application.Validators.Drops

{
    public static class DropValidatorConfiguration
    {
        public const int MinimumNameLength = 2;

        public const int MaximumNameLength = 50;

        public const int MinimumSecretKeyLength = 4;

        public const int MaximumSecretKeyLength = 128;

        public const int MinimumFileLength = 1;

        public const int MaximumFileLength = 10;

    }
}
