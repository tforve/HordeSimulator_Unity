using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_SeekMana : MonoBehaviour
{
    public CharacterType charType = CharacterType.MANAPOTION;

    public float potionSize = 100.0f;
    public float collectingRange = 1.0f;

    private float weight;                                       // weight given to Character for Decision making, different to calculated because of Veto
    public float weightCalculated = 2.0f;                       // weight to calculate
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
            weight = weightCalculated;
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
        // no Potion existing
        if (closest == null) { return; }

        CalculateWeight();

        if (dist < collectingRange)
        {
            MyCharacter.RestoreMana(potionSize);
            Destroy(closest.gameObject);
        }
        else
        {
            Vector3 dir = closest.transform.position - this.transform.position;
            MyCharacter.MyDirection = dir;

            float wd = weight;
            MyCharacter.desiredWeights.Add(wd);
            Debug.Log("AI_SeekMana Triggered");

        }

    }

    private float CalculateWeight()
    {
        return weightCalculated;
    }
}
