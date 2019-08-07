using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_SeekMana : MonoBehaviour
{
    public string charType = "Manapotion";

    public float potionSize = 100.0f;
    public float collectingRange = 1.0f;

    private float weight;                                       // weight given to Character for Decision making, different to calculated because of Veto
    public float weightCalculated = 0.0f;                       // weight to calculate
    public bool veto = false;                                   // if true AI Action not executed

    // just for Debug purpose right now
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
            CalculateWeight();
        }


        if (Character.characterByType.ContainsKey(charType) == false)
        {
            //nothing to do
            return;
        }

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
        // no Potion existing but my weight stays the same because my Mana is the condition
        if (closest == null) { return; }

        if (dist < collectingRange)
        {
            MyCharacter.RestoreMana(potionSize);
            Destroy(closest.gameObject);
        }
        else
        {
            Vector3 dir = closest.transform.position - this.transform.position;
            WeightedDirection wd = new WeightedDirection(dir, weight, "SeekMana");
            MyCharacter.desiredWeights.Add(wd);
        }

    }

    private void CalculateWeight()
    {
        //float linearTmp = ((MyCharacter.maxMana - MyCharacter.mana) / MyCharacter.maxMana); // 100-currentHp / 100
        float expoTmp = (Mathf.Pow(2, MyCharacter.mana *(-0.05f))); // 2^-x
        weightCalculated = expoTmp;//Mathf.InverseLerp(0, 1, expoTmp);
        weight = weightCalculated;
    }
}
