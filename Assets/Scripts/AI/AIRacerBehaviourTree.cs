using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIRacerBehaviourTree : MonoBehaviour
{
    AIRacerClass AIRacerScript;
    FearMachine FearMachineScript;

    // Use this for initialization
    void Start()
    {
        AIRacerScript = GameObject.Find("MainGame").GetComponent<AIRacerClass>();
        FearMachineScript = GameObject.Find("MainGame").GetComponent<FearMachine>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //ACTIONS

    //STATUS

    //Status Check

    bool CheckIfStatusNormal(int i)
    {
        if (AIRacerScript.AIRacerList[i].StatusAilment == "Normal")
        {
            return true;
        }

        return false;
    }

    bool CheckIfStatusFear(int i)
    {
        if (AIRacerScript.AIRacerList[i].StatusAilment == "Fear")
        {
            return true;
        }

        return false;
    }

    bool CheckIfStatusStop(int i)
    {
        if (AIRacerScript.AIRacerList[i].StatusAilment == "Stop")
        {
            return true;
        }

        return false;
    }

    //If Status Do:

    bool WhenFearedDo(int i)
    {
        //Run from Target
        AIRacerScript.AIRacerList[i].SteeringForce = AISteeringBehaviour.SeekOrFlee(AIRacerScript.AIRacerList[i].CurrentPosition, FearMachineScript.TheFearMachine.CurrentPosition, AIRacerScript.AIRacerList[i].CurrentMaxSpeed / 1.3f, AIRacerScript.AIRacerList[i].CurrentDirection, false);
        return true;

    }

    bool WhenStoppedDo(int i)
    {
        //Stop
        AIRacerScript.AIRacerList[i].CurrentMaxSpeed = 0;
        AIRacerScript.AIRacerList[i].SteeringForce = AISteeringBehaviour.SeekOrFlee(AIRacerScript.AIRacerList[i].CurrentPosition, AIRacerScript.AIRacerList[i].DesiredPosition, AIRacerScript.AIRacerList[i].CurrentMaxSpeed / 1.3f, AIRacerScript.AIRacerList[i].CurrentDirection, false);
        return true;
    }

    //Reset Status if Duration Over:

    bool ResetStatusIf(int i)
    {
        if (Time.time >= AIRacerScript.AIRacerList[i].StatusAffectUntil && AIRacerScript.AIRacerList[i].StatusAilment != "Normal")
        {
            ResetStatus(AIRacerScript.AIRacerList[i].StatusAilment = "Normal");
            return true;
        }
        return false;
    }
    
    //Set Status Back to Normal

    void ResetStatus(string Status)
    {
        Status = "Normal";
    }

    //Check Fear Machine Collision

    bool CheckIfFearMachineCollision(int i)
    {
        if (FearMachineScript.CheckFMtoDriverCollision(FearMachineScript.TheFearMachine, AIRacerScript.AIRacerList[i].CurrentPosition) == true)
        {
            AIRacerScript.SetStatus("Fear", i, 2);
            return false;
        }

        return true;
    }

    //SEQUENCES FORMAT

    /*
    bool Sequences(bool[] FunctionList)
    {
        for (int i = 0; i < FunctionList.Length; i++)
        {
            if (FunctionList[i] == false)
            {
                return false; 
            }
        }
        return true;
    }
    */



    //SELECTORS FORMAT

    /*
    bool Selectors(bool[] FunctionList)
    {
        for (int i = 0; i < FunctionList.Length; i++)
        {
            if (FunctionList[i] == true)
            {
                return true; //If one Function Succeeds, returns Success
            }
        }
        return false;
    }
    */
      
    //BEHAVIOUR TREE

    //ROOT - Selector

    //Status Ailment? - Selector
    //Race if Normal Status, Else React based on Status

    public bool BehaviourTree(int i)
    {
        //SELECTOR
        if (RACESequence(i) == true) //Race if "Normal"
        {
            return true;
        }
        if(SelectStatusSelector(i) == true) //Check Through Status
        {
            return true;
        }

        return false;
    }

    //RACE - Sequence


    bool RACESequence(int i)
    {
        //SEQUENCE
        if (CheckIfStatusNormal(i) == false) //Check if "Normal"
        {
            return false;
        }
        if (CheckIfFearMachineCollision(i) == false)
        {
            return false;
        }
        if (AIRacerScript.Drive(i) == false) //Race
        {
            return false;
        }

        return true;
    }

    //Select Status - Selector

    //Go through Status' and inflict based on current Status

    bool SelectStatusSelector(int i)
    {
        //SELECTOR
        if (FearSequence(i) == true) //If Fear then...
        {
            return true;
        }
        if (StopSequence(i) == true) //If Stop then...
        {
            return true;
        }

        return false;
    }

    //Fear - Sequence

    bool FearSequence(int i)
    {
        //SEQUENCE
        if (CheckIfStatusFear(i) == false) //Check if Feared
        {
            return false;
        }
        if (WhenFearedDo(i) == false) //Do Fear Function
        {
            return false;
        }
        if (ResetStatusIf(i) == false) //Reset to Normal if Time Duration done
        {
            return false;
        }

        return true;
    }

    //Stop - Sequence

    bool StopSequence(int i)
    {
        //SEQUENCE
        if (CheckIfStatusStop(i) == false) //Check if Stopped
        {
            return false;
        }
        if (WhenStoppedDo(i) == false) //Stop Racer
        {
            return false;
        }
        if (ResetStatusIf(i) == false) //Reset to Normal if Time Duration done
        {
            return false;
        }

        return true;
    }

}
