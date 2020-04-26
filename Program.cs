using System;
using System.Collections.Generic;
using System.Net;
using FChan.Library;
using System.IO;
using Newtonsoft.Json;

namespace chandl
{
    class Program
    {
        public static string localDirectory = Environment.CurrentDirectory;

        public static Tuple<List<string>, int, string> GetThreadImages(string board, int threadNumber)
        {
            List<string> imgList = new List<string>();
            try
            {
                var a = Chan.GetThread(board, threadNumber);
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("Invalid thread");
                throw new NullReferenceException();
            }
            Thread threadObject = Chan.GetThread(board, threadNumber);
            foreach (Post postObject in threadObject.Posts)
            {
                if (postObject.FileName != 0)
                {
                    imgList.Add(postObject.FileName + postObject.FileExtension);
                }
            }
            return Tuple.Create(imgList, threadNumber, board);
        }
        public static void DownloadImages(List<string> imageList, int directoryName, string boardName)
        {
            string rootThreadDir = localDirectory + "\\threads";
            string threadDir = rootThreadDir + "\\" + directoryName.ToString();
            if (!Directory.Exists(rootThreadDir))
            {
                Directory.CreateDirectory(rootThreadDir);
                Console.WriteLine("Root thread directory was not found. Creating it now.");
            }
            if (!Directory.Exists(threadDir))
            {
                Directory.CreateDirectory(threadDir);
            }
            else
            {
                Console.WriteLine("Thread directory "+threadDir+" already exists. Continue? [y/n]");
                string response = Console.ReadLine();
                if (response == "n")
                {
                    Console.WriteLine("Stopping...");
                    System.Environment.Exit(1);
                }
            }
            WebClient webclient = new WebClient();
            foreach (string file in imageList)
            {
                string fileURL = "https://i.4cdn.org/" + boardName + "/" + file;
                Console.WriteLine("Attempting to download " + fileURL);
                webclient.DownloadFile(fileURL, threadDir + "\\" + file);
                Console.WriteLine("File " + file + " downloaded.");
            }
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Running from " + localDirectory);
            Console.WriteLine("Downloading images from thread " + args[0] + " on board " + Int32.Parse(args[1]));
            var images = GetThreadImages(args[0], Int32.Parse(args[1]));
            DownloadImages(images.Item1, images.Item2, images.Item3);
            Console.WriteLine("Job Complete!");
            Console.Read();
        }
    }
}
