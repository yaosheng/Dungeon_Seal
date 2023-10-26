using UnityEngine;
using System.Collections;

public struct Point
{
    public int x, y;
    public Point( int pointX, int pointY )
    {
        x = pointX;
        y = pointY;
    }
    public override string ToString( )
    {
        return "(" + x + " , " + y + ")";
    }
    public bool IsIdentical( Point point )
    {
        bool tempBool = false;

        if(point.x == x && point.y == y) {
            tempBool = true;
        }
        else {
            tempBool = false;
        }
        return tempBool;
    }
    public bool IsNeighbor( Point point )
    {
        bool tempBool = false;
        if((Mathf.Abs(point.x - x) == 1 && point.y == y) || (Mathf.Abs(point.y - y) == 1 && point.x == x)) {
            tempBool = true;
        }
        else {
            tempBool = false;
        }
        return tempBool;
    }

    public bool IsAround(Point point )
    {
        bool tempBool = false;
        if(point.x != 0 || point.y != 0) {
            if(Mathf.Abs(point.x - x) <= 1 && Mathf.Abs(point.y - y) <= 1) {
                tempBool = true;
            }
        }
        return tempBool;
    }
}

public class Data
{
    public const float moveSpeed = 0.2f;
    public const float turnSpeed = 0.05f;
    public const float readyTime = 0.1f;
    public const float atttTime = 0.1f;
}