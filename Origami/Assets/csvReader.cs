using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csvReader
{

    public class Waypoint
    {
        public DateTime zuluDate;
        public double X;
        public double Y;
        public double Z;

        public Waypoint(DateTime zd, double x, double y, double z)
        {
            zuluDate = zd;
            X = x;
            Y = y;
            Z = z;
        }
    }

    public class Trajectory
    {
        public String uid;
        public String modelId;
        public double size = 1;
        public double rotationSpeed;
        public Color colour;

        public List<Waypoint> waypoints;

        public Trajectory(String id, String mid, double s, List<Waypoint> wp, Color c)
        {
            uid = id;
            modelId = mid;
            size = s;
            waypoints = wp;
            colour = c;
        }
    }

    public static Trajectory createTrajectory(String fileLocation)
    {
        Trajectory body = null;
        List<Waypoint> waypoints = new List<Waypoint>();

        string[] lines = System.IO.File.ReadAllLines(@fileLocation);
        DateTime initialTime = new DateTime();

        if (lines.Length > 0)
        {
            string[] tradData = lines[0].Split(',');

            String uId = tradData[0].Replace("\"","").Trim();
            String modelId = tradData[1].Replace("\"", "").Trim();
            double size = Double.Parse(tradData[2]);
            Color colour = getColour(tradData[3].Trim());

            body = new Trajectory(uId, modelId, size, waypoints, colour);

            for (int i = 1; i < lines.Length; i++)
            {
                string[] position = lines[i].Split(',');
                int year = Int32.Parse(position[0].Substring(1, 4));
                int month = Int32.Parse(position[0].Substring(6, 2));
                int day = Int32.Parse(position[0].Substring(9, 2));
                int hour = Int32.Parse(position[0].Substring(12, 2));
                int minute = Int32.Parse(position[0].Substring(15, 2));
                int second = Int32.Parse(position[0].Substring(18, 2));

                DateTime zulu = new DateTime(year, month, day, hour, minute, second);

                if (i == 0)
                {
                    initialTime = zulu;
                }
                else
                {
                    TimeSpan duration = zulu - initialTime;
                }

                double X = double.Parse(position[1]);
                double Y = double.Parse(position[2]);
                double Z = double.Parse(position[3]);

                waypoints.Add(new Waypoint(zulu, X, Y, Z));
            }
        }
        return body;
    }

    public static Color getColour(string inputColour)
    {
        if (inputColour == "white") {
            return Color.white;
        }
        else if (inputColour == "black")
        {
            return Color.black;
        }
        else if (inputColour == "grey")
        {
            return Color.grey;
        }
        else if (inputColour == "red")
        {
            return Color.red;
        }
        else if (inputColour == "blue")
        {
            return Color.blue;
        }
        else if (inputColour == "green")
        {
            return Color.green;
        }
        else if (inputColour == "yellow")
        {
            return Color.yellow;
        } else
        {
            return Color.white; // Default
        }
    }


}
