using System.Collections;
using System;
using UnityEngine;

public class DistanceComparer : IComparer
{
    private Transform compareTransform;

    public DistanceComparer(Transform compTrans)
    {
        compareTransform = compTrans;
    }

    public int Compare(object x, object y)
    {
        Collider xCollider = x as Collider;
        Collider yCollider = y as Collider;

        Vector3 offset = xCollider.transform.position - compareTransform.position;
        float xDistance = offset.sqrMagnitude;

        offset = yCollider.transform.position - compareTransform.position;
        float yDistance = offset.sqrMagnitude;

        return xDistance.CompareTo(yDistance);
    }
}
