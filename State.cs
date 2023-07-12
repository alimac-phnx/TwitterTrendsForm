using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterTrendsForm
{
    public class State
    {
        public string Name { get; set; }
        public List<object>[,] Coordinates { get; set; }

        public List<double> GetStateCenter()
        {
            double sumX = 0;
            int counterX = 0;
            int counterY = 0;
            double sumY = 0;
            void Calculations(double x, double y)
            {
                sumX += x;
                sumY += y;
                counterX++;
                counterY++;
            }

            foreach (var coos in Coordinates)
            {
                if (coos.All(c => c is double))
                {
                    Calculations((double)coos[0], (double)coos[1]);
                }
                else
                {
                    foreach (var coosCoos in coos)
                    {
                        Calculations(((List<double>)coosCoos)[0], ((List<double>)coosCoos)[1]);
                    }
                }
            }
            return new List<double>() { sumX / counterX, sumY / counterY };
        }
    }
}
