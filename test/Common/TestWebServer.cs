using System;
using System.Threading.Tasks;
using Unosquare.Labs.EmbedIO;

namespace SeleniumExtras.Environment
{
    public class TestWebServer
    {
        private WebServer webServer;

        private string url;
        private string htmlPath;

        public TestWebServer(string url, string htmlPath)
        {
            this.url = url;
            this.htmlPath = htmlPath;
        }

        public void Start()
        {
            if (webServer != null)
            {
                throw new InvalidOperationException("WebServer is already started!");
            }

            webServer = WebServer
                   .Create(url)
                   .WithStaticFolderAt(htmlPath);
            Task.Run(() => webServer.RunAsync());
        }

        public void Stop()
        {
            if (webServer != null)
            {
                webServer.Dispose();
                webServer = null;
            }
        }
    }
}
