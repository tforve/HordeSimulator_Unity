using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeightedDirection// : IComparer<float>
{
	public readonly Vector3 direction;
	public readonly float weight;

	public WeightedDirection(Vector3 dir, float wgt) {
		direction = dir.normalized;
		weight = wgt;
	}

	public float MyWeight{ get{return weight;}}
}