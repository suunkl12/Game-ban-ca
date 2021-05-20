/*
 * Date: October 20, 2015.
 * Author: Guillermo Bauza.
 * E-mail: guilei4910@yahoo.com
 * Description: Follow any path created from PathDefinition Script. 
*/
/// <summary>
/// Please refer to the guide that came with the plugin for better understanding.
/// First of all if you have any problem with the default script contact me so we can obtain an answer and fix together the problem.
/// You are completely free to modify the script, Warning: I don't get responsible for future errors if the default script get changed or edited. 
/// </summary>
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using APS.PathDefinitionAlgorithm;
using System;
using Mirror;

public class FollowPath : MonoBehaviour
{
    [Header("Follow Path Settings.")]
    #region FIELDS
    public PathDefinition path; // PathDefinition Instance.
    public MovementType MoveDirection = MovementType.MoveForward; // Direction that I move on Path
    public RotationType rotationType = RotationType.NONE;
    public PathBehaviour OnPathBehaviour = PathBehaviour.RESET_OR_LOOP;
    public float angle = 0; // main asset angle while rotating.
    //how far in the path do you want to start? (Each section has length 1)
    public float startingPos = 0f; // my starting position on path.
    public bool stop = false; //stop object on his actual position.
    public float moveSpeed = 1; //How fast the object move one Path.

    // Colletions of the path objects.
    private List<Vector3> mainPoints = new List<Vector3>();
    private List<Vector3> controlsIn = new List<Vector3>();
    private List<Vector3> controlsOut = new List<Vector3>();

    private int totalSections = 0; // how many sections will the line/curve have between main points.
    private int totalPoints = 0; // total of main points on path.
    private bool loop = true; //loop over path.
    private float time = 0; //current time along the path.
    private float _defaultMoveSpeed = 1;
    private APS_PDAlgorithm Algorithm = new APS_PDAlgorithm();

    [Header("Optional Setting")]
    [Header("Custom speed for each CentralPoint following the order.")]
    public List<CentralPoints> mainPointsCustomSpeed = new List<CentralPoints>();

    [Serializable]
    public class CentralPoints
    {
        [Tooltip("Name the object to identify yourself.")]
        public string Name;
        [Tooltip("Movespeed once reached next point in order.")]
        public float pointMoveSpeed;
        [Tooltip("Use the default speed assigned on moveSpeed property.")]
        public bool useDefault;

        public CentralPoints() { }
    }//class
    #endregion

    void Start()
    {
        //Error Exceptions
        Exceptions();
        _defaultMoveSpeed = moveSpeed;
        //Get loop setting
        loop = path.loopPath;

        //get default coordinates of curve
        if (!loop && MoveDirection == MovementType.MoveBackward)
            MoveDirection = MovementType.MoveForward;

        if (MoveDirection == MovementType.MoveForward)
            getBackCoordinates(path.controlInObjectName, path.controlOutObjectName);

        else if (loop && MoveDirection == MovementType.MoveBackward)
            getBackCoordinates(path.controlOutObjectName, path.controlInObjectName);

        time = startingPos;
        totalPoints = mainPoints.Count;
        totalSections = mainPoints.Count - (loop ? 0 : 1);

    }//Start()

    void Update()
    {
        moveSpeedErrorException();

        if (stop) return;

        #region TIME VALUE
        time += Time.deltaTime * moveSpeed;
        if (time >= totalSections)
        {
            switch (OnPathBehaviour)
            {
                case PathBehaviour.RESET_OR_LOOP:
                    time -= totalSections;
                    break;
                case PathBehaviour.DESTROY_ONCE_PATH_END:
                    Destroy(gameObject);
                    return;

                case PathBehaviour.STOP_ONCE_PATH_END:
                    return;

                case PathBehaviour.RESUME_MOVEMENT:
                    time = startingPos;
                    break;

                default:
                    time -= totalSections;
                    break;
            }
        }
        #endregion

        #region TRANSFORM SETUP
        int i = (int)Mathf.Floor(time);
        float t = time - i;
        int a = (i + 1) % totalPoints;

        TwickMoveSpeed(i);
        setTransformPosition(i, a, t);

        #endregion

    }//Update()

    #region Rotation Functions
    private void Rotate2D(int i, int a, float t)
    {
        Vector3 CA = Algorithm.CurveAlgorithm(mainPoints[i], controlsOut[i], controlsIn[a], mainPoints[a], t);
        float curveAngle = Mathf.Atan2(CA.y, CA.x) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, 0, curveAngle + angle);
    }

    private void Rotate3D_Z(int i, int a, float t)
    {
        Vector3 CA = Algorithm.CurveAlgorithm(mainPoints[i], controlsOut[i], controlsIn[a], mainPoints[a], t);
        float curveAngley = Mathf.Atan2(CA.x, CA.z) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(transform.rotation.x, curveAngley + angle, 0);
    }
    #endregion

    #region ENUMERATOR CLASS'S
    //Enumerator Type of rotations.
    public enum RotationType
    {
        NONE, ON_2D_PATH, ON_3D_PATH
    }

    // Enumerator type of path behaviours
    public enum PathBehaviour
    {
        RESET_OR_LOOP,
        DESTROY_ONCE_PATH_END,
        STOP_ONCE_PATH_END,
        RESUME_MOVEMENT
    }

    //Enumerator Type of movement
    public enum MovementType
    {
        MoveForward,
        MoveBackward
    }//Enumerator{}

    #endregion

    #region Algorithm [NO TOUCH]
    private List<Vector3> getBackCoordinates(List<Vector3> points)
    {
        List<Vector3> tempMain = new List<Vector3>();
        tempMain.Add(points[0]);

        for (int i = points.Count - 1; i > 0; i--)
            tempMain.Add(points[i]);

        return tempMain;
    }//Back Cordiante method

    private void getBackCoordinates(string str1, string str2)
    {
        foreach (Transform child in path.gameObject.transform)
        {
            mainPoints.Add(child.transform.position);
            foreach (Transform child2 in child.transform)
            {
                if (child2.name == str1) controlsIn.Add(child2.transform.position);
                if (child2.name == str2) controlsOut.Add(child2.transform.position);
            }//second foreach
        }//foreach

        if (str1 == path.controlOutObjectName)
        {
            mainPoints = getBackCoordinates(mainPoints);
            controlsIn = getBackCoordinates(controlsIn);
            controlsOut = getBackCoordinates(controlsOut);
        }//if - path is out
    }//Back Cordiante method

    #endregion

    #region LOGS
    private void Exceptions()
    {
        moveSpeedErrorException();
        pathEmptyErrorException();
    }

    private void moveSpeedErrorException()
    {
        if (moveSpeed < 0)
        {
            Debug.LogWarning(gameObject.name + ": Move Speed field value cannot be less than 0, Move Speed field value was set to positive.");
            moveSpeed *= -1;
        }
    }

    private void pathEmptyErrorException()
    {
        if (path == null)
        {
            Debug.LogError("Attach the object with the script PathDefinition.cs");
            return;
        }//if null
    }

    #endregion

    #region FUNCTIONS

    private void TwickMoveSpeed(int index)
    {
        if (mainPointsCustomSpeed.Count == 0)
            moveSpeed = _defaultMoveSpeed;
        else if (index >= mainPointsCustomSpeed.Count)
            moveSpeed = _defaultMoveSpeed;
        else
            moveSpeed = (mainPointsCustomSpeed[index].useDefault) ? _defaultMoveSpeed : mainPointsCustomSpeed[index].pointMoveSpeed;
        
    }//function

    private void setTransformPosition(int i, int a, float t)
    {
        Vector3 Cpos = Algorithm.GetCurvePosition(mainPoints[i], controlsOut[i], controlsIn[a], mainPoints[a], t);

        if (rotationType == RotationType.ON_2D_PATH)
            Rotate2D(i, a, t);
        else if (rotationType == RotationType.ON_3D_PATH)
            Rotate3D_Z(i, a, t);

        transform.position = Cpos;
    }
    #endregion
}//class
