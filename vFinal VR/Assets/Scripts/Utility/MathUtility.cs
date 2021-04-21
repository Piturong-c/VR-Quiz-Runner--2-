using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathUtility
{
    public static float Zigmoid(float x, float shift)
    {
        return 1f/(1 + Mathf.Exp(x - shift));
    }
}
