﻿using System;
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

    }

    // Update is called once per frame
    void Update()
    {
        DataModel myModel = this.gameObject.GetComponent<DataModel>();

        double secs = myModel.playbackSpeed * Time.deltaTime * 1000;
        myModel.playbackTime = myModel.playbackTime.AddSeconds(secs);

        updateTrajectories(myModel);
    }

    void updateTrajectories(DataModel myModel)
    { 
        foreach (csvReader.Trajectory body in myModel.object_List)
        {
            int c = body.waypoints.Count;
            if (c > 0)
            {
                GameObject sprite = null;
                myModel.trajectoryGameObjects.TryGetValue(body.uid, out sprite);

                if(sprite != null)
                {
                    DateTime tsS = body.waypoints[0].zuluDate;
                    DateTime tsE = body.waypoints[c -1].zuluDate;

                    // Boundaries
                    Vector3 target = sprite.transform.position;

                    if (myModel.playbackTime <= tsS)
                    {
                        target = fromWayPoint(body.waypoints[0], myModel.galacticScale);
                    }
                    else if (myModel.playbackTime >= tsE)
                    {
                        target = fromWayPoint(body.waypoints[c - 1], myModel.galacticScale);
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
                                target = fromWayPoint(wp, myModel.galacticScale);
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

                                Vector3 p1 = fromWayPoint(wp1, myModel.galacticScale);
                                Vector3 p2 = fromWayPoint(wp2, myModel.galacticScale);

                                Vector3 v = p2 - p1;
                                Vector3 s = v * (float)mag;

                                target = p1 + s;
                            }
                        }
                    }
                    sprite.transform.position = target;
                    updateGameObjectSize(sprite, (float)body.size, myModel.galacticScale);
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
        List<String> fileLocations = new List<string>();
        fileLocations.Add(Application.dataPath + "/StreamingAssets/lunar_probe.txt");
        fileLocations.Add(Application.dataPath + "/StreamingAssets/lunar_orbit.txt");
        fileLocations.Add(Application.dataPath + "/StreamingAssets/earth_orbit.txt");

        foreach (String fileLocation in fileLocations)
        {
            csvReader.Trajectory body = csvReader.createTrajectory(fileLocation);
            model.object_List.Add(body);
            GameObject sprite = GameObject.Find(body.modelId);
            model.trajectoryGameObjects.Add(body.uid, sprite);
        }
    }

    private static Vector3 fromWayPoint(csvReader.Waypoint wp, float galacticScale)
    {
        return new Vector3((float)wp.X / galacticScale, (float)wp.Y / galacticScale, (float)wp.Z / galacticScale);
    }

    private static void updateGameObjectSize(GameObject sprite, float size, float galacticScale)
    {
        Renderer[] spriteRender = sprite.GetComponentsInChildren<Renderer>();
        if (spriteRender.Length > 0)
        {
            Vector3 initScale = sprite.transform.localScale;
            Vector3 spriteSize = spriteRender[0].bounds.size;

            float spriteMag = spriteSize.magnitude;
            float scaleMag = (size / galacticScale) / spriteMag;

            sprite.transform.localScale = initScale * scaleMag;
        }
    }
}
