using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_EvadeEnemy : MonoBehaviour
{
    public CharacterType charType = CharacterType.ENEMY;
    public float rangeOfCare = 5.0f;
    public float weight;                                    // weight given to Character for Decision making, different to calculated because of Veto
    public float weightCalculated = 2.0f;                   // weight to calculate
    public bool veto = false;                               // if true AI Action not executed

    public float MyWeight
    {
        get { return weight; }
    }

    Character MyCharacter;

    void Start()
    {
        MyCharacter = GetComponent<Character>();
    }

    void DoAIBehaviour()
    {
        // Check Veto to not execute 
        if (veto)
        {
            weight = 0.0f;
        }
        // Go on and execute AI_Behavior
        else
        {
            weight = weightCalculated;
        }

        if (Character.characterByType.ContainsKey(charType) == false) { return; }

        // calculate nearest
        Character closest = null;
        float dist = Mathf.Infinity;

        foreach (Character c in Character.characterByType[charType])
        {
            float d = Vector3.Distance(this.transform.position, c.transform.position);
            if (closest == null || d < dist)
            {
                closest = c;
                dist = d;
            }
        }
        // no Enemy existing
        if (closest == null) { return; }


        // evade if
        if (dist > rangeOfCare)
        {
            return;
        }
        else // if in care Range
        {
            CalculateWeight();
            Vector3 dir = closest.transform.position - this.transform.position;
            WeightedDirection wd = new WeightedDirection(weight);
            MyCharacter.desiredDirections.Add(wd);
            MyCharacter.MoveTo(dir);
        }

    }

    private float CalculateWeight()
    {
        return weight;
    }
}
