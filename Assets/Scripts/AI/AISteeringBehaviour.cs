using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISteeringBehaviour {

    //SEEK OR FREE

    public static Vector2 SeekOrFlee(Vector2 CurrentPosition, Vector2 TargetPosition, float MaxSpeed, Vector2 CurrentDirection, bool TrueIfSeekFalseIfFlee)
    {
        //Get Desired Velocity
        Vector2 DesiredVelocity;

        //Seek Or Flee Algorithm
        if (TrueIfSeekFalseIfFlee == true)
        {
            DesiredVelocity = VectorArithmetic.VectorNormalized(TargetPosition - CurrentPosition) * MaxSpeed;
        }
        else
        {
            DesiredVelocity = VectorArithmetic.VectorNormalized(CurrentPosition - TargetPosition) * MaxSpeed;
        }

        //Get Current Velocity
        Vector2 CurrentVelocity = CurrentDirection;

        //Return Steering Force
        return DesiredVelocity - CurrentVelocity; 
    }

    //ARRIVE

    public static Vector2 Arrive(Vector2 CurrentPosition, Vector2 TargetPosition, float MaxSpeed, Vector2 CurrentDirection, float Threshhold)
    {
        //Get ToTarget vector
        Vector2 ToTarget = TargetPosition - CurrentPosition;

        //Get Distance to Target
        float ToTargetMagnitude = VectorArithmetic.VectorLength(ToTarget);

        //If Distance is Less than Threshold...
        Vector2 DesiredVelocity = VectorArithmetic.VectorNormalized(ToTarget) * MaxSpeed * (ToTargetMagnitude / Threshhold);

        //Get Current Velocity
        Vector2 CurrentVelocity = CurrentDirection;

        //Return Steering Force
        return DesiredVelocity - CurrentVelocity;
    }

    //PURSUIT OR EVADE

    public static Vector2 PursuitOrEvade(Vector2 SelfCurrentPosition, Vector2 EnemyCurrentPosition, Vector2 SelfCurrentDirection, 
                           Vector2 EnemyCurrentDirection, float SelfMaxSpeed, float EnemyMaxSpeed, bool TrueIfPursuitFalseIfEvade)
    {
        //Default Values
        Vector2 ReturnThis;
        float RelativeHeading = 0;
        Vector2 ToEnemy = EnemyCurrentPosition - SelfCurrentPosition;

        //Find Relative Heading for Pursuit
        if (TrueIfPursuitFalseIfEvade == true)
        {
            RelativeHeading = VectorArithmetic.DotProduct(VectorArithmetic.VectorNormalized(SelfCurrentDirection), VectorArithmetic.VectorNormalized(EnemyCurrentDirection));
        }

        //If Both Self and Enemy facing the same Direction?
        if (RelativeHeading > 0 && TrueIfPursuitFalseIfEvade == true)
        {
            ReturnThis = SeekOrFlee(SelfCurrentPosition, EnemyCurrentPosition, SelfMaxSpeed, SelfCurrentDirection, true);
        }
        else
        {
            //Where the Enemy will be...
            float LookAheadTime = VectorArithmetic.VectorLength(ToEnemy) / (SelfMaxSpeed + EnemyMaxSpeed);
            Vector2 EnemyFuturePosition = EnemyCurrentPosition + EnemyCurrentDirection * LookAheadTime;

            //Pursuit or Evade Future Position
            if (TrueIfPursuitFalseIfEvade == true)
            {
                ReturnThis = SeekOrFlee(SelfCurrentPosition, EnemyFuturePosition, SelfMaxSpeed, SelfCurrentDirection, true);
            }
            else
            {
                ReturnThis = SeekOrFlee(SelfCurrentPosition, EnemyFuturePosition, SelfMaxSpeed, SelfCurrentDirection, false);
            }
        }

        return ReturnThis;
    }
}
