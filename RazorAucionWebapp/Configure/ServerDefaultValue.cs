namespace RazorAucionWebapp.Configure
{
    public class ServerDefaultValue
    {
        public string ImagesPath { get; }
        public ServerDefaultValue(IWebHostEnvironment environment, IConfiguration configuration) 
        {
            ImagesPath = environment.WebRootPath + "PublicImages/";
        }

    }
}
