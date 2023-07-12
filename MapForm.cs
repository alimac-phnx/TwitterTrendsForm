using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TwitterTrendsForm
{
    public partial class MapForm : Form
    {
        public MapForm()
        {
            InitializeComponent();
            MapSettings();

            ColorTheMap();
            ShowTweetPoints();
        }

        public void MapSettings()
        {
            gMapControl1.MapProvider = GMapProviders.GoogleMap;
            gMapControl1.Position = new PointLatLng(38, -97);
            gMapControl1.MinZoom = 0;
            gMapControl1.MaxZoom = 100;
            gMapControl1.Zoom = 3;
        }

        public void ShowTweetPoints()
        {
            TwitterFile twitterFile = new TwitterFile(@"C:\Users\alimac\Downloads\cali_tweets2014.txt");

            foreach (var tw in twitterFile.GetTweets())
            {
                PointLatLng point = new PointLatLng(tw.Coordinates[0], tw.Coordinates[1]);
                GMapMarker marker = new GMarkerGoogle(point, GMarkerGoogleType.orange);
                GMapOverlay markers = new GMapOverlay("markers");
                markers.Markers.Add(marker);
                gMapControl1.Overlays.Add(markers);
            }
        }

        public GMapOverlay BuildThePolygon(State state, GMapOverlay polyOverlay)
        {
            List<PointLatLng> points = new List<PointLatLng>();
            foreach (var item in state.Coordinates)
            {
                points.Add(new PointLatLng((double)item[1], (double)item[0]));
            }
            GMapPolygon polygon = new GMapPolygon(points, state.Name);
            polygon = ColorThePolygons(polygon, state.Name);

            polyOverlay.Polygons.Add(polygon);

            return polyOverlay;
        }

        public GMapOverlay BuildThePolygons(State state, GMapOverlay polyOverlay)
        {
            for (int i = 0; i < state.Coordinates.Length; i++)
            {
                List<PointLatLng> points = new List<PointLatLng>();
                foreach (List<double> item in state.Coordinates[i, 0])
                {
                    points.Add(new PointLatLng(item[1], item[0]));
                }
                GMapPolygon polygon = new GMapPolygon(points, state.Name);
                polygon = ColorThePolygons(polygon, state.Name);

                polyOverlay.Polygons.Add(polygon);
            }

            return polyOverlay;
        }

        public GMapPolygon ColorThePolygons(GMapPolygon polygon, string stateName)
        {
            foreach (var line in File.ReadLines(@"C:\Users\alimac\Desktop\statesMood1.txt"))
            {
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

                double stateMood = Convert.ToDouble(line.Substring(line.IndexOf("\t")));
                string stN = line.Substring(0, 2);
                if (stN == stateName)
                {
                    if (stateMood == 0)
                    {
                        polygon.Fill = new SolidBrush(Color.FromArgb(50, Color.White));
                        polygon.Stroke = new Pen(Color.White, 1);
                    }
                    else if (stateMood > 0)
                    {
                        if (stateMood == 10000)
                        {
                            polygon.Fill = new SolidBrush(Color.FromArgb(50, Color.Gray));
                            polygon.Stroke = new Pen(Color.Gray, 1);
                        }
                        else
                        {
                            polygon.Fill = new SolidBrush(Color.FromArgb(50, Color.Red));
                            polygon.Stroke = new Pen(Color.Red, 1);
                        }
                    }
                    else
                    {
                        polygon.Fill = new SolidBrush(Color.FromArgb(50, Color.Blue));
                        polygon.Stroke = new Pen(Color.Blue, 1);
                    }
                }
            }
            return polygon;
        }

        public void ColorTheMap()
        {
            JsonFile jsonFile = new JsonFile(@"C:\Users\alimac\Downloads\states.json");
            List<State> states = jsonFile.GetStates();

            GMapOverlay polyOverlay = new GMapOverlay("polygons");
            foreach (var state in states)
            {
                switch (state.Name)
                {
                    case "WA": case "HI": case "VA": case "AK": case "MD": case "MI": case "RI":
                        polyOverlay = BuildThePolygons(state, polyOverlay);
                        break;
                    default:
                        polyOverlay = BuildThePolygon(state, polyOverlay);
                        break;
                }
            }

            gMapControl1.Overlays.Add(polyOverlay);
        }
    }
}
