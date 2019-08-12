using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_EvadeEnemy : MonoBehaviour
{
    public string charType = "Enemy";
    public float rangeOfCare = 5.0f;
    private float weight;                                    // weight given to Character for Decision making, different to calculated because of Veto
    public float weightCalculated = 0.0f;                   // weight to calculate
    public bool veto = false;                               // if true AI Action not executed

    public float MyWeight                                   // get the Max Weight 
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
            weight = 0.0f; return;
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
        if (closest == null)
        {
            weight = 0.0f;
            weightCalculated = 0.0f;
            Vector3 dir = Vector3.zero;
            WeightedDirection wd = new WeightedDirection(-dir, weight);
            return;
        }

        CalculateWeight(dist);

        // evade if
        if (dist > rangeOfCare)
        {
            return;
        }
        else // if in care Range
        {

            Vector3 dir = closest.transform.position - this.transform.position;
            WeightedDirection wd = new WeightedDirection(-dir, weight);
            MyCharacter.desiredWeights.Add(wd);

            UIController.MyInstance.SetSliderValue(1.0f);
        }

    }

    private void CalculateWeight(float distanceToEnemey)
    {
        weightCalculated = Mathf.InverseLerp(0, 1, 10 / (distanceToEnemey * distanceToEnemey));
        weight = weightCalculated;
    }
}
