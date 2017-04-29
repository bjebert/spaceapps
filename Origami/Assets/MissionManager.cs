using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    // Use this for initialization
    void Start ()
    {
        DataModel myModel = this.gameObject.AddComponent<DataModel>();
        LineRenderer lineRendererShuttle = this.gameObject.AddComponent<LineRenderer>();
        lineRendererShuttle.positionCount = 20;
        lineRendererShuttle.widthMultiplier = 0.01f;
        lineRendererShuttle.material = new Material(Shader.Find("Unlit/Texture"));
        lineRendererShuttle.startColor = Color.white;
        lineRendererShuttle.endColor = Color.white;

        //LineRenderer lineRendererMoon = this.gameObject.AddComponent<LineRenderer>();
        //lineRendererShuttle.positionCount = 20;

        readData(myModel);
        plotMissionLines(myModel);

        myModel.trajectoryGameObjects.Add("shuttle", GameObject.Find("SpaceModelsCollection/shut"));
        myModel.trajectoryGameObjects.Add("moon", GameObject.Find("SpaceModelsCollection/moon"));
    }

    // Update is called once per frame
    void Update ()
    {
        DataModel myModel = this.gameObject.GetComponent<DataModel>();

        double secs = myModel.playbackSpeed * Time.deltaTime * 1000;
        myModel.playbackTime = myModel.playbackTime.AddSeconds(secs);

        foreach (csvReader.Trajectory body in myModel.object_List)
        {
            int c = body.waypoints.Count;
            if (c > 0)
            {
                GameObject sprite = null;
                if (myModel.trajectoryGameObjects.TryGetValue(body.modelId, out sprite))
                {
                    DateTime tsS = body.waypoints[0].zuluDate;
                    DateTime tsE = body.waypoints[c -1].zuluDate;

                    // Boundaries
                    Vector3 target = sprite.transform.position;

                    if (myModel.playbackTime <= tsS)
                    {
                        target = fromWayPoint(body.waypoints[0]);
                    }
                    else if (myModel.playbackTime >= tsE)
                    {
                        target = fromWayPoint(body.waypoints[c - 1]);
                    }
                    else
                    {
                        csvReader.Waypoint wp1 = body.waypoints[0];
                        csvReader.Waypoint wp2 = body.waypoints[0];

                        bool skiped = false;
                        foreach (csvReader.Waypoint wp in body.waypoints)
                        {
                            if (myModel.playbackTime == wp.zuluDate)
                            {
                                target = fromWayPoint(wp);
                                skiped = true;
                            }
                            else
                            {
                                wp1 = wp2;
                                wp2 = wp;

                                if (wp.zuluDate > myModel.playbackTime)
                                {
                                    break;
                                }
                            }
                        }

                        if(!skiped)
                        {
                            TimeSpan diffA = myModel.playbackTime - wp1.zuluDate;
                            TimeSpan diffB = wp2.zuluDate - wp1.zuluDate;
                            if (diffB.TotalMilliseconds > 0)
                            {
                                double mag = diffA.TotalMilliseconds / diffB.TotalMilliseconds;

                                Vector3 p1 = fromWayPoint(wp1);
                                Vector3 p2 = fromWayPoint(wp2);

                                Vector3 v = p2 - p1;
                                Vector3 s = v * (float)mag;

                                target = p1 + s;
                            }
                        }
                    }
                    sprite.transform.position = target;
                }
            }
        }
    }

    private void plotMissionLines(DataModel model)
    {
        List<csvReader.Waypoint> moonTrajectories = new List<csvReader.Waypoint>();
        List<csvReader.Waypoint> shuttleTrajectories = new List<csvReader.Waypoint>();

        List<csvReader.Trajectory> trajectories = model.object_List;
        foreach (csvReader.Trajectory trajectory in trajectories)
        {
            if (trajectory.modelId == "shuttle")
            {
                shuttleTrajectories = trajectory.waypoints;
            } else if (trajectory.modelId == "moon")
            {
                moonTrajectories = trajectory.waypoints;
            }
        }

        // Plot lines
        int index = 0;
        LineRenderer lineRendererShuttle = this.gameObject.GetComponent<LineRenderer>();
        foreach (csvReader.Waypoint trajectory in shuttleTrajectories)
        {
            Vector3 coords = fromWayPoint(trajectory);
            lineRendererShuttle.SetPosition(index, coords);

            index += 1;
            if (index == 19)
            {
                break;
            }
        }

        index = 0;
        /**LineRenderer lineRendererMoon = this.gameObject.GetComponent<LineRenderer>();
        foreach (csvReader.Waypoint trajectory in moonTrajectories)
        {
            Vector3 coords = fromWayPoint(trajectory);
            lineRendererMoon.SetPosition(index, coords);

            index += 1;
        }*/

    }

    private void readData(DataModel model)
    {
        // Read from file
        string fileLocation = Application.dataPath + "/StreamingAssets/lunar_probe.txt";
        model.object_List.Add(csvReader.createTrajectory("shuttle", fileLocation));

        fileLocation = Application.dataPath + "/StreamingAssets/lunar_orbit.txt";
        model.object_List.Add(csvReader.createTrajectory("moon", fileLocation));
    }

    private static Vector3 fromWayPoint(csvReader.Waypoint wp)
    {
        return new Vector3((float)wp.X / (float)100000000.0, (float)wp.Y / (float)100000000.0, (float)wp.Z / (float)100000000.0);
    }
}
