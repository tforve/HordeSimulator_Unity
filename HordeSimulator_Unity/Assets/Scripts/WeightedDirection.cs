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

	// Not used in this tutorial, but you could set a flag
	// to determine if we are going to be blending this direction
	// with others, or if it's exclusive and should be the ONLY
	// direction applied. If more than one behaviour returns
	// an EXCLUSIVE direction, the one with the highest weight
	// should be used.
	// FALLBACK blending would be used only if there are no other
	// directions desired -- such as a random wander when
	// there's nothing else to do.
	public enum BlendingType { BLEND, EXCLUSIVE, FALLBACK };
	public BlendingType blending = BlendingType.BLEND;	// UNUSED

	// Not used in this tutorial, but scripts could set a desired
	// speed, especially if the energy cost of moving scales
	// exponentially with speed.  Normal movement would be done at
	// a lower, more efficient speed -- but emergencies (escaping
	// a predator, or moving in for the final kill) could request
	// higher, more costly speeds.  This would be blended based on
	// weight.
	public float speed = -1f; // UNUSED
}