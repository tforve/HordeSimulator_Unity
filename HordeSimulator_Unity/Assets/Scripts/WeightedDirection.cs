using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeightedDirection// : IComparer<float>
{
	//public readonly Vector3 direction;
	public readonly float weight;

	public WeightedDirection(float wgt) 
    {
		//direction = dir.normalized;
		weight = wgt;
	}

    // public int Compare(float x, float y)
    // {
    //     return null;
    // }
}