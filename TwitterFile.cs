﻿using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TwitterTrendsForm
{
    public class TwitterFile
    {
        private string Path { get; set; }
        public TwitterFile(string filePath)
        {
            Path = filePath;
        }

        public List<Tweet> GetTweets()
        {
            List<Tweet> tweets = (File.ReadLines(Path)).Select(x => new Tweet
            {
                Coordinates = ReadData(x, 1, '\t'),
                Text = ReadData(x, '\t')
            }).ToList();

            return tweets;
        }

        private string ReadData(string line, char parameter)
        {
            string data = line.Substring((line.LastIndexOf(parameter) + 1));//[(line.LastIndexOf(parameter) + 1)..];

            return data;
        }

        private List<double> ReadData(string line, int parameter1, char parameter2)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            var data = line.Substring(parameter1, (line.IndexOf(parameter2) - 2))/*[parameter1..(line.IndexOf(parameter2) - 1)]*/.Replace(" ", "").Split(',').Select(double.Parse).ToList();

            return data;
        }
    }
}