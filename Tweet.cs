using System;
using System.Collections.Generic;
using System.Linq;
using TwitterTrendsForm;

namespace TwitterTrendsForm
{
    public class Tweet
    {
        public List<double> Coordinates { get; set; }
        public string Text { get; set; }
        //public string Region { get { return GetTweetBelonging(); } set { } }
        //public double Mood { get { return CalculateTheMood(); } set { } }

    }
}