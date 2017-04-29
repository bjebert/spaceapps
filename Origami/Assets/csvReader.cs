using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csvReader : MonoBehaviour {

    public class Waypoint
    {
        public DateTime zuluDate;
        public int deltaTime;
        public double X;
        public double Y;
        public double Z;

        public Waypoint(DateTime zd, int dt, double x, double y, double z)
        {
            zuluDate = zd;
            deltaTime = dt; // Time since first waypoint
            X = x;
            Y = y;
            Z = z;
        }
    }

    public class Trajectory
    {
        public String modelId;
        public List<Waypoint> waypoints;

        public Trajectory(String mid, List<Waypoint> wp)
        {
            modelId = mid;
            waypoints = wp;
        }
    }

    // Use this for initialization
    void Start () {
        Trajectory moon = createTrajectory("moon", "C:/Users/Blake/Projects/spaceapps/JSON/lunar_orbit.txt");
        Trajectory probe = createTrajectory("probe", "C:/Users/Blake/Projects/spaceapps/JSON/lunar_probe.txt");
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public static Trajectory createTrajectory(String modelId, String fileLocation)
    {
        List<Waypoint> waypoints = new List<Waypoint>();

        string[] lines = System.IO.File.ReadAllLines(@fileLocation);
        DateTime initialTime = new DateTime();

        for (int i = 0; i < lines.Length; i++)
        {
            string[] position = lines[i].Split(',');
            int year = Int32.Parse(position[0].Substring(1, 4));
            int month = Int32.Parse(position[0].Substring(6, 2));
            int day = Int32.Parse(position[0].Substring(9, 2));
            int hour = Int32.Parse(position[0].Substring(12, 2));
            int minute = Int32.Parse(position[0].Substring(15, 2));
            int second = Int32.Parse(position[0].Substring(18, 2));

            DateTime zulu = new DateTime(year, month, day, hour, minute, second);

            int delta = 0;

            if (i == 0)
            {
                initialTime = zulu;
            }
            else {
                TimeSpan duration = zulu - initialTime;
                delta = (int) duration.TotalSeconds;
            }

            double X = double.Parse(position[1]);
            double Y = double.Parse(position[2]);
            double Z = double.Parse(position[3]);

            waypoints.Add(new Waypoint(zulu, delta, X, Y, Z));
        }

        return new Trajectory(modelId, waypoints);
    }


}
