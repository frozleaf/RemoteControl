using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace RemoteControl.Client.Downloader
{
    class Program
    {
        static string url;
        static string filePath;
        static string filePathInDownloading;

        static void Main(string[] args)
        {
            if (args.Length != 2)
                return;

            url = args[0];
            filePath = args[1];
            try
            {
                System.Net.WebClient client = new System.Net.WebClient();
                client.DownloadProgressChanged += new System.Net.DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
                client.DownloadFileCompleted += new System.ComponentModel.AsyncCompletedEventHandler(client_DownloadFileCompleted);
                filePathInDownloading = filePath + ".tmp";
                client.DownloadFileAsync(new Uri(url), filePathInDownloading);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            while (true)
            {
                Thread.Sleep(1000);
            }
        }

        static void client_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            try
            {
                System.IO.File.Delete(filePath);
                System.IO.File.Move(filePathInDownloading, filePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.WriteLine("完成!");
            Environment.Exit(0);
        }

        static void client_DownloadProgressChanged(object sender, System.Net.DownloadProgressChangedEventArgs e)
        {
            Console.WriteLine(e.ProgressPercentage);
        }
    }
}
