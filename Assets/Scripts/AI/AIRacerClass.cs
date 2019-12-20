using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class AIRacerClass : MonoBehaviour {

    //Racer Properties
    [System.Serializable]
    public struct AIRacer
    {
        public string RacerName;
        public Vector2 DesiredPosition;
        public Vector2 CurrentPosition;
        public int DesiredRank;
        public int CalculatedRank;
        public int Lap;
        public string StatusAilment;
        public float StatusAffectUntil;
        public int CurrentWaypoint;
        public Vector2 CurrentDirection;
        public Vector2 SteeringForce;
        public float OriginalMaxSpeed;
        public float CurrentMaxSpeed;
        public float RubberbandingSpeed;
        public string DriverGO;
    }

    public static int NumberOfRacers = 4;
    public int TestVariable;
    public AIRacer[] AIRacerList;

    AIRacerBehaviourTree BehaviourTreeScript;
    public string[] DriverGOList;
    WaypointClass WaypointScript;
    public int[] CalculatedRanksList = new int[NumberOfRacers];

    //Default Values
    //AIRacerArray[]

    //Set Status Effect
    public void SetStatus(string StatusAilment, int i, float FearDuration)
    {
        //Set Status Effect
        AIRacerList[i].StatusAilment = StatusAilment;

        //Set Length of Status Effect
        float StatusUntil = Time.time + FearDuration;
        AIRacerList[i].StatusAffectUntil = StatusUntil;
    }


    // Use this for initialization
    void Start() {
        BehaviourTreeScript = GameObject.Find("MainGame").GetComponent<AIRacerBehaviourTree>();
        WaypointScript = GameObject.Find("MainGame").GetComponent<WaypointClass>();

        for (int i = 0; i < AIRacerList.Length; i++)
        {
            AIRacerList[i].StatusAilment = "Normal";
            AIRacerList[i].Lap = 1;
            AIRacerList[i].CalculatedRank = 0;
            AIRacerList[i].CurrentPosition = GameObject.Find(DriverGOList[i]).transform.position;
            AIRacerList[i].CurrentWaypoint = 0;
            AIRacerList[i].CurrentDirection = new Vector2(0, 1);
            AIRacerList[i].DriverGO = DriverGOList[i];
            AIRacerList[i].CurrentMaxSpeed = AIRacerList[i].OriginalMaxSpeed;
            AIRacerList[i].RubberbandingSpeed = AIRacerList[i].OriginalMaxSpeed * 2f;
            CalculatedRanksList[i] = AIRacerList[i].CalculatedRank;
        }
    }

    // Update is called once per frame
    void Update() {

        //Calculate Positions of Racers based on Race
        CalculateRanks();

        //Stop 1st Place
        if (Input.GetKeyDown("space"))
        {
            SetStatus("Stop", 0, 3);
        }

        for (int i = 0; i < AIRacerList.Length; i++)
        {
            //BehaviourTree
            BehaviourTreeScript.BehaviourTree(i);

            //Transform Object
            DriveTransform(i);
        }



    }

    //Rank Calculating

    public void CalculateRanks()
    {
        for (int i = 0; i < AIRacerList.Length; i++)
        {          
            AIRacerList[i].CalculatedRank = AIRacerList[i].Lap * 100 + AIRacerList[i].CurrentWaypoint;
            CalculatedRanksList[i] = AIRacerList[i].CalculatedRank;
        }
    }

    public bool Drive(int i)
    {
        //Rubberbanding

        int DesiredRankCalculated = (from number in CalculatedRanksList
                                     orderby number descending
                                     select number).Skip(AIRacerList[i].DesiredRank - 1).First();
                                     //select number).Skip(1).First();

        //If entire Waypoint Behind Desired Rank, Speed Up
        if (AIRacerList[i].CalculatedRank < DesiredRankCalculated)
        {
            AIRacerList[i].CurrentMaxSpeed = AIRacerList[i].RubberbandingSpeed;
        }
        else AIRacerList[i].CurrentMaxSpeed = AIRacerList[i].OriginalMaxSpeed;


        //Set Current Waypoint
        AIRacerList[i].CurrentWaypoint = WaypointScript.CheckWaypointCollision(WaypointScript.WaypointList[AIRacerList[i].CurrentWaypoint], true,
                                                                               AIRacerList[i].CurrentPosition, AIRacerList[i].CurrentWaypoint, i);
        
        //DRIVE
        WaypointScript.SeekWaypoint(i, WaypointScript.WaypointList[AIRacerList[i].CurrentWaypoint]);
       
        return true;
    }

    public void DriveTransform(int i)
    {
        //Apply Driving
        AIRacerList[i].CurrentDirection += AIRacerList[i].SteeringForce;
        AIRacerList[i].CurrentPosition += AIRacerList[i].CurrentDirection * Time.deltaTime;

        //Transform Object
        GameObject.Find(AIRacerList[i].DriverGO).transform.position = AIRacerList[i].CurrentPosition;
    }

}
