using Service.Services.AppAccount;
using System.Drawing;
using System.Security.Claims;

namespace RazorAucionWebapp.BackgroundServices
{
    public static class ImageHandler
    {
        public static void SaveImage(IFormFile image, string path, string filename)
        {
            byte[] bytes = null;
            if (image != null)
            {
                using (Stream fs = image.OpenReadStream())
                {
                    using (BinaryReader br = new BinaryReader(fs))
                    {
                        var stream = br.ReadBytes((Int32)fs.Length);
                        using (var ms = new MemoryStream(stream))
                        {
                            using (var img = System.Drawing.Image.FromStream(ms))
                            {
                                int maxWidth = 800;
                                int maxHeight = 800;

                                int newWidth, newHeight;

                                if (img.Width > maxWidth || img.Height > maxHeight)
                                {
                                    float aspectRatio = (float)img.Width / (float)img.Height;

                                    if (img.Width > maxWidth)
                                    {
                                        newWidth = maxWidth;
                                        newHeight = (int)(newWidth / aspectRatio);
                                    }
                                    else
                                    {
                                        newHeight = maxHeight;
                                        newWidth = (int)(newHeight / aspectRatio);
                                    }
                                }
                                else
                                {
                                    newWidth = img.Width;
                                    newHeight = img.Height;
                                }
                                using (Bitmap resizedMap = new Bitmap(newWidth, newHeight))
                                {
                                    using (Graphics graphics = Graphics.FromImage(resizedMap))
                                    {
                                        graphics.DrawImage(img, 0, 0, newWidth, newHeight);
                                    }
                                }
                                CreateDirectory(path);
                                img.Save(Path.Combine(path,filename), System.Drawing.Imaging.ImageFormat.Png);
                            }
                        }
                    }
                }
            }
        }

        public static void CreateDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public static string RemoveWhiteSpace(string s)
        {
            return new String(s.ToCharArray().Where(c => !Char.IsWhiteSpace(c)).ToArray());
        }
    }
}
