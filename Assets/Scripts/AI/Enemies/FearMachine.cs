using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FearMachine : MonoBehaviour
{
    [System.Serializable]
    public struct FearMach
    {
        public Vector2 DesiredPosition;
        public Vector2 CurrentPosition;
        public int CurrentWaypoint;
        public Vector2 CurrentDirection;
        public Vector2 DesiredDirection;
        public Vector2 SteeringForce;
        public float OriginalMaxSpeed;
        public float CurrentMaxSpeed;
        public string EnemyGO;
        public float BoundingBoxSize;
    }

    public FearMach TheFearMachine;
    public static float BoundingSquareRadius = 1.5f;

    WaypointClass WaypointScript;

    // Use this for initialization
    void Start()
    {
        WaypointScript = GameObject.Find("MainGame").GetComponent<WaypointClass>();

        //Initialise FearMachine Variables
        TheFearMachine.EnemyGO = "Fear Machine";
        TheFearMachine.CurrentPosition = GameObject.Find(TheFearMachine.EnemyGO).transform.position;
        TheFearMachine.CurrentWaypoint = 10;
        TheFearMachine.CurrentDirection = new Vector2(0, -1);
        TheFearMachine.CurrentMaxSpeed = TheFearMachine.OriginalMaxSpeed;
        TheFearMachine.BoundingBoxSize = BoundingSquareRadius;

    }

    // Update is called once per frame
    void Update()
    {
        Drive();
        DriveTransform();


    }

    public bool Drive()
    {
        //If Rubberbanding Do X, for now do Y
        TheFearMachine.CurrentMaxSpeed = TheFearMachine.OriginalMaxSpeed;
        TheFearMachine.CurrentWaypoint = WaypointScript.CheckWaypointCollision(WaypointScript.WaypointList[TheFearMachine.CurrentWaypoint], false,
                                                                               TheFearMachine.CurrentPosition, TheFearMachine.CurrentWaypoint);
        //Drive Calculation
        TheFearMachine.DesiredPosition = WaypointScript.WaypointList[TheFearMachine.CurrentWaypoint].CentrePosition;
        TheFearMachine.SteeringForce = AISteeringBehaviour.SeekOrFlee(TheFearMachine.CurrentPosition, TheFearMachine.DesiredPosition,
                                                                                       TheFearMachine.CurrentMaxSpeed, TheFearMachine.CurrentDirection, true);

        return true;
    }

    public void DriveTransform()
    {
        //Apply Driving
        TheFearMachine.CurrentDirection += TheFearMachine.SteeringForce;
        TheFearMachine.CurrentPosition += TheFearMachine.CurrentDirection * Time.deltaTime;

        //Transform Object
        GameObject.Find(TheFearMachine.EnemyGO).transform.position = TheFearMachine.CurrentPosition;
    }

    public bool CheckFMtoDriverCollision(FearMach FearMachine, Vector2 CurrentPosition)
    {
        float xMin = FearMachine.CurrentPosition.x - FearMachine.BoundingBoxSize / 2;
        float xMax = FearMachine.CurrentPosition.x + FearMachine.BoundingBoxSize / 2;
        float yMin = FearMachine.CurrentPosition.y - FearMachine.BoundingBoxSize / 2;
        float yMax = FearMachine.CurrentPosition.y + FearMachine.BoundingBoxSize / 2;

        if (CurrentPosition.x > xMin && CurrentPosition.x < xMax && CurrentPosition.y > yMin && CurrentPosition.y < yMax)
        {
            return true;
        }

        return false;
    }
}
