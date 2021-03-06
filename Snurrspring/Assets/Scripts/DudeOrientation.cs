﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DudeOrientation : MonoBehaviour
{
    public static Quaternion CalcOrientation(Vector2 normal)
    {
        var p = normal;

        Vector3 vectorToTarget = p;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        return q;
    }

}
