/*
 * Date: October 20, 2015.
 * Author: Guillermo Bauza.
 * E-mail: guilei4910@yahoo.com
 * Description: Create a path to be followed using FollowPath Script . 
*/
/// <summary>
/// First of all if you have any problem with the default script contact me so we can obtain an answer and fix together the problem.
/// You are completely free to modify the script, Warning: I don't get responsible for future errors if the default script get changed or edited. 
/// </summary>
using UnityEngine;
using System.Collections.Generic;
using APS.PathDefinitionAlgorithm;
using Mirror;
using System;


public class PathDefinition:MonoBehaviour
{
    [Header("Path Definition Settings.")]
    #region FIELDS
    // how many steps will the line/curve have between main points. (More steps points = Better Accuracy on curves).
    public int steps = 30; //How many steps to reach next point.
    public bool showSections = true;//Show divisions between points
    public float sizeOfSections = 0.05f; // How big will be the sections GUI be.
    public bool loopPath = false; //loop over path.
    public bool hidePathOnRuntime = true; // Hide the Path once Game starts
    public string controlInObjectName = "In", controlOutObjectName = "Out"; // Name of the children objects in the MainPoints
    private Color guiColor = Color.yellow;
    public Color pathColor = Color.cyan;
    private APS_PDAlgorithm Algorithm = new APS_PDAlgorithm();
    #endregion

    void Start()
    {
        //hide Path in-game
        if (hidePathOnRuntime)
            HidePath();
    }//Start

    private void HidePath()
    {
        Transform[] transforms = GetComponentsInChildren<Transform>();
        foreach (Transform obj in transforms)
            obj.gameObject.SetActive(false);
    }//Hide > start

    void OnDrawGizmos()
    {
        List<Vector3> mainPoints = new List<Vector3>();
        List<Vector3> In = new List<Vector3>();
        List<Vector3> Out = new List<Vector3>();

        foreach (Transform child in gameObject.transform)
        {
            mainPoints.Add(child.transform.position);
            foreach (Transform child2 in child.transform)
            {
                if (child2.name == controlInObjectName) In.Add(child2.transform.position);
                if (child2.name == controlOutObjectName) Out.Add(child2.transform.position);
            }//second foreach
        }//foreach

        Gizmos.color = guiColor;
        //draw Path Lines
        for (int i = 0; i < mainPoints.Count; i++)
        {
            Gizmos.DrawLine(mainPoints[i], In[i]);
            Gizmos.DrawLine(mainPoints[i], Out[i]);
        }

        Gizmos.color = pathColor;
        if (In.Count >= mainPoints.Count && Out.Count >= mainPoints.Count)
            for (int i = 0; i < mainPoints.Count - (loopPath ? 0 : 1); i++)
            {
                //draw curves
                int i2 = (i + 1) % mainPoints.Count;
                Vector3 P2 = new Vector3(0, 0, 0);
                float step = 1.0f / steps;
                if (step > 0.01f)
                    for (float t = 0; t < 1 + step; t += step)
                    {
                        Vector3 P1 = P2;
                        P2 = Algorithm.DrawPath(mainPoints[i], Out[i], In[i2], mainPoints[i2], t);
                        if (t > 0)
                        {
                            Gizmos.DrawLine(P1, P2);
                            if (showSections)
                            {
                                Vector3 PA = Algorithm.PathAlgorithm(mainPoints[i], Out[i], In[i2], mainPoints[i2], t);
                                Vector3 pinned = Vector3.Cross(PA, Vector3.forward).normalized;

                                Gizmos.DrawLine(P2 + (pinned * sizeOfSections), P2 - (pinned * sizeOfSections));
                            }//if sections
                        }//if t > 0
                    }//for 2
            }//for 1
    }//OnDrawGizmos


}//class
