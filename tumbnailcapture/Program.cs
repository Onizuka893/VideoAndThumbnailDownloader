using NReco.VideoConverter;
using System;

namespace tumbnailcapture // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Video Downloader + thumbnail");
            Console.WriteLine("Video URL must contain : '.webm', '.ogv', '.mp4', '.ogg', '.avi', '.wmv'");
            Console.WriteLine("Enter the url of the video to download");
            var url = Console.ReadLine();
            bool urlValid = checkURL(url);
            Console.WriteLine("Take thumbnail from video? (y/n)");
            var takeThumbnail = Console.ReadLine();
            bool takeThumbnailBool = false;
            if (takeThumbnail == "y")
            {
                takeThumbnailBool = true;
            }
            Console.WriteLine("Enter the name of the video to save");
            var name = Console.ReadLine();
            if(!string.IsNullOrEmpty(name))
            {
                if (name.Contains(" "))
                {
                    name.Replace(" ", "_");
                }
            }
            else
            {
                name = "video";
            }

            if (urlValid)
            {
                Console.WriteLine("Downloading...");
                var output = await DownloadFile(url);
                if(output != null)
                {
                    File.WriteAllBytes($"{name}.mp4", output);
                    if (takeThumbnailBool)
                    {
                        Console.WriteLine("Taking thumbnail...");
                        getTumbnail($"{name}.mp4");
                    }
                    Console.WriteLine("Done");
                }
                else
                {
                    Console.WriteLine("Error");
                }
            }
            else
            {
                Console.WriteLine("Invalid URL");
            }
        }

        public static void getTumbnail(string pathToVideoFile)
        {
            var ffMpeg = new NReco.VideoConverter.FFMpegConverter();
            ffMpeg.GetVideoThumbnail(pathToVideoFile, $"video_thumbnail_{pathToVideoFile}.jpg");
        }

        public static async Task<byte[]> DownloadFile(string url)
        {
            using (var client = new HttpClient())
            {

                using (var result = await client.GetAsync(url))
                {
                    if (result.IsSuccessStatusCode)
                    {
                        return await result.Content.ReadAsByteArrayAsync();
                    }
                }
            }
            return null;
        }

        public static bool checkURL(string url)
        {
            if (url.Contains("youtube.com"))
            {
                return false;
            }
            else if (url.Contains("mp4") || url.Contains("webm") || url.Contains("ogv") || url.Contains("ogg") || url.Contains("avi") || url.Contains(".wmv"))
            {
                if(url.Contains("http://") || url.Contains("https://"))
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }
        }
    }
}