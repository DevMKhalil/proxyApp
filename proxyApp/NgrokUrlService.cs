namespace proxyApp
{
    public interface INgrokUrlService
    {
        string GetNgrokUrl();
        void UpdateNgrokUrl(string newUrl);
    }

    public class NgrokUrlService : INgrokUrlService
    {
        private string _ngrokUrl;

        public NgrokUrlService(IConfiguration configuration)
        {
            // Initialize from configuration (from appsettings.json or environment variables)
            _ngrokUrl = configuration.GetSection("TargetSettings")["NgrokUrl"];
        }

        public string GetNgrokUrl() => _ngrokUrl;

        public void UpdateNgrokUrl(string newUrl)
        {
            _ngrokUrl = newUrl;
        }
    }
}
