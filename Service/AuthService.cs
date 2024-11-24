namespace Terra.Service
{
    public class AuthService
    {
        public bool IsAuthenticated { get; private set; }

        public Task Login()
        {
            IsAuthenticated = true;
            return Task.CompletedTask;
        }

        public Task Logout()
        {
            IsAuthenticated = false;
            return Task.CompletedTask;
        }
    }
}
