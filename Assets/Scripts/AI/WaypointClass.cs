using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaypointClass : MonoBehaviour {

    public static int WaypointMinimum = 0;
    public static int WaypointMaximum = 12;
    public static float BoundingSquareRadius = 0.5f;
    AIRacerClass AIRacerScript;

    [System.Serializable]
    public struct Waypoints
    {
        public int WaypointNumber;
        public Vector2 CentrePosition;
        public float BoundingBoxXsize;
        public float BoundingBoxYsize;
    }

    public Waypoints[] WaypointList = new Waypoints[12];
    public string[] WaypointNameList = { "Waypoint 0", "Waypoint 1", "Waypoint 2",
                                         "Waypoint 3", "Waypoint 4", "Waypoint 5",
                                         "Waypoint 6", "Waypoint 7", "Waypoint 8",
                                         "Waypoint 9", "Waypoint 10", "Waypoint 11"};



    // Use this for initialization
    void Start () {

        AIRacerScript = GameObject.Find("MainGame").GetComponent<AIRacerClass>();

        //Set WaypointNumber from 0 to Waypoint Maximum (0 to 9)

        for (int w = 0; w < WaypointList.Length; w++)
        {
            WaypointList[w].WaypointNumber = w;
            WaypointList[w].BoundingBoxXsize = BoundingSquareRadius;
            WaypointList[w].BoundingBoxYsize = BoundingSquareRadius;
            WaypointList[w].CentrePosition = GameObject.Find(WaypointNameList[w]).transform.position;

        }	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    /*
    public void CheckWaypointCollision(int i, Waypoints Waypoint)
    {
        float xMin = Waypoint.CentrePosition.x - Waypoint.BoundingBoxXsize / 2;
        float xMax = Waypoint.CentrePosition.x + Waypoint.BoundingBoxXsize / 2;
        float yMin = Waypoint.CentrePosition.y - Waypoint.BoundingBoxYsize / 2;
        float yMax = Waypoint.CentrePosition.y + Waypoint.BoundingBoxYsize / 2;

        Vector2 RacerPos = AIRacerScript.AIRacerList[i].CurrentPosition;

        if (RacerPos.x > xMin && RacerPos.x < xMax && RacerPos.y > yMin && RacerPos.y < yMax)
        {
            AIRacerScript.AIRacerList[i].CurrentWaypoint = WaypointIncrease(AIRacerScript.AIRacerList[i].CurrentWaypoint);
        }

    }
    */
    public int WaypointIncrease(int CurrentWaypoint, int i)
    {
        int NewWaypoint = CurrentWaypoint;

        if (NewWaypoint == WaypointMaximum - 1)
        {
            NewWaypoint = WaypointMinimum;
            AIRacerScript.AIRacerList[i].Lap += 1;
        }
        else
        {
            NewWaypoint++;
            AIRacerScript.AIRacerList[i].CalculatedRank += 10;
        }

        return NewWaypoint;
    }


        public int WaypointIncrease(int CurrentWaypoint)
    {
        int NewWaypoint = CurrentWaypoint;

        if (NewWaypoint == WaypointMaximum - 1)
        {
            NewWaypoint = WaypointMinimum;
        }
        else NewWaypoint++;

        return NewWaypoint;
    }

    public int WaypointDecrease(int CurrentWaypoint)
    {
        int NewWaypoint = CurrentWaypoint;

        if (NewWaypoint == WaypointMinimum)
        {
            NewWaypoint = WaypointMaximum - 1;
        }
        else NewWaypoint--;

        return NewWaypoint;
    }



    public void SeekWaypoint(int i, Waypoints Waypoint)
    {
        AIRacerScript.AIRacerList[i].DesiredPosition = Waypoint.CentrePosition;
        AIRacerScript.AIRacerList[i].SteeringForce = AISteeringBehaviour.SeekOrFlee(AIRacerScript.AIRacerList[i].CurrentPosition, AIRacerScript.AIRacerList[i].DesiredPosition, 
                                                                                       AIRacerScript.AIRacerList[i].CurrentMaxSpeed, AIRacerScript.AIRacerList[i].CurrentDirection, true);
    }

    //Enemies

    public int CheckWaypointCollision(Waypoints Waypoint, bool TrueIfIncrease, Vector2 CurrentPosition, int CurrentWaypoint)
    {
        float xMin = Waypoint.CentrePosition.x - Waypoint.BoundingBoxXsize / 2;
        float xMax = Waypoint.CentrePosition.x + Waypoint.BoundingBoxXsize / 2;
        float yMin = Waypoint.CentrePosition.y - Waypoint.BoundingBoxYsize / 2;
        float yMax = Waypoint.CentrePosition.y + Waypoint.BoundingBoxYsize / 2;

        if (CurrentPosition.x > xMin && CurrentPosition.x < xMax && CurrentPosition.y > yMin && CurrentPosition.y < yMax)
        {
            if (TrueIfIncrease == true)
            {
                CurrentWaypoint = WaypointIncrease(CurrentWaypoint);
            }
            else
            {
                CurrentWaypoint = WaypointDecrease(CurrentWaypoint);
            }
        }

        return CurrentWaypoint;
    }

    public int CheckWaypointCollision(Waypoints Waypoint, bool TrueIfIncrease, Vector2 CurrentPosition, int CurrentWaypoint, int i)
    {
        float xMin = Waypoint.CentrePosition.x - Waypoint.BoundingBoxXsize / 2;
        float xMax = Waypoint.CentrePosition.x + Waypoint.BoundingBoxXsize / 2;
        float yMin = Waypoint.CentrePosition.y - Waypoint.BoundingBoxYsize / 2;
        float yMax = Waypoint.CentrePosition.y + Waypoint.BoundingBoxYsize / 2;

        if (CurrentPosition.x > xMin && CurrentPosition.x < xMax && CurrentPosition.y > yMin && CurrentPosition.y < yMax)
        {
            if (TrueIfIncrease == true)
            {
                CurrentWaypoint = WaypointIncrease(CurrentWaypoint, i);
            }
            else
            {
                CurrentWaypoint = WaypointDecrease(CurrentWaypoint);
            }
        }

        return CurrentWaypoint;
    }
}
