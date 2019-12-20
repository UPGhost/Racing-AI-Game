using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorArithmetic
{

    /*Extra Maths

    Radians = Degrees / (180 / Mathf.PI)

    Degrees = Radians * (180 / Mathf.PI)

    */

    public static Vector2 VectorAdd(Vector2 A, Vector2 B)
    {
        float Cx = A.x + B.x;
        float Cy = A.y + B.y;

        return new Vector2(Cx, Cy);
    }

    public static Vector3 VectorSubtract(Vector2 A, Vector2 B)
    {
        float Cx = A.x - B.x;
        float Cy = A.y - B.y;

        return new Vector2(Cx, Cy);
    }

    public static float VectorMagnitude(Vector2 A)
    {
        return A.x / A.y;
    }

    public static float LengthSq(Vector2 A)
    {
        float sq = A.x * A.x + A.y * A.y;

        return sq;
    }

    public static float VectorLength(Vector2 A)
    {
        float squareRoot = Mathf.Sqrt(LengthSq(A));

        return squareRoot;
    }

    public static Vector2 MultiplyVector(Vector2 A, float scalar)
    {
        float newAx = A.x * scalar;
        float newAy = A.y * scalar;

        return new Vector2(newAx, newAy);
    }

    public static Vector2 DivideVector(Vector3 A, float divisor)
    {
        float newAx = A.x / divisor;
        float newAy = A.y / divisor;

        return new Vector2(newAx, newAy);
    }

    public static Vector2 VectorNormalized(Vector2 A)
    {
        return DivideVector(A, VectorLength(A));
    }

    public static float DotProduct(Vector2 A, Vector2 B)
    {
        Vector3 newA = VectorNormalized(A);
        Vector3 newB = VectorNormalized(B);

        float dotProduct = newA.x * newB.x + newA.y * newB.y;

        return dotProduct;

    }

    public static float VectorLengthAandB(Vector2 A, Vector2 B)
    {
        float E = A.x - B.x;
        float F = A.y - B.y;
        float test = E * E + F * F;

        float squareRoot = Mathf.Sqrt(test);

        return squareRoot;

    }

    public static float findAngle(Vector2 A)
    {
        //Vector to Angle
        float OppOverAdj = A.y / A.x;

        float angle = Mathf.Atan(OppOverAdj);

        //Radians to Degrees
        float angleDegrees = angle * (180 / Mathf.PI);

        return angleDegrees;
    }

    public static Vector2 findVector(float angleDegrees)
    {
        //Degrees to Radians
        float angleRadians = angleDegrees / (180 / Mathf.PI);

        //Angle to Vector
        float x = Mathf.Cos(angleRadians);
        float y = Mathf.Sin(angleRadians);

        return new Vector2(x, y);
    }

}
