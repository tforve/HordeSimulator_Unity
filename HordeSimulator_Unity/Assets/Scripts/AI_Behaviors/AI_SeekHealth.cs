using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_SeekHealth : MonoBehaviour
{
    public CharacterType charType = CharacterType.HEALTHPOTION;

    public float potionSize = 20;
    public float collectingRange = 1.0f;

    private float weight;                                           // weight given to Character for Decision making, different to calculated because of Veto
    public float weightCalculated = 1.0f;                           // weight to calculate

    public bool veto = false;                                       // if true AI Action not executed

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
        // no Potion existing
        if (closest == null) { return; }



        if (dist < collectingRange)
        {
            MyCharacter.RestoreHealth(potionSize);
            Destroy(closest.gameObject);
        }
        else
        {
                Vector3 dir = closest.transform.position - this.transform.position;
                WeightedDirection wd = new WeightedDirection(dir, weight);
                MyCharacter.desiredWeights.Add(wd);
            if (weightCalculated == HeroAI_Controller.MyInstance.MyMaxWeight)
            {


            }
            else
            {
                Debug.Log("not allowed: Seek Heal");
            }


            // Vector3 dir = closest.transform.position - this.transform.position;
            // MyCharacter.MyDirection = dir;

            // float wd = weight;
            // MyCharacter.desiredWeights.Add(wd);
            Debug.Log("AI_SeekHealth Triggered");

        }

    }

    private float CalculateWeight()
    {
        return weightCalculated;
    }
}
