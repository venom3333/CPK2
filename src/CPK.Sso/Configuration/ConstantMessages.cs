namespace CPK.Sso.Configuration
{
    public static class ConstantMessages
    {
        public static string RegisterConfirmationMessage(string email) => $"На {email} выслано письмо с подтверждением регистрации. Вы должны подтвердить регистрацию прежде чем сможете войти.";
    }
}