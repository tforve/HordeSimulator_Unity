﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_EvadeEnemy : MonoBehaviour
{
    public CharacterType charType = CharacterType.ENEMY;
    public float rangeOfCare = 5.0f;
    public float weight;                                    // weight given to Character for Decision making, different to calculated because of Veto
    public float weightCalculated = 2.0f;                   // weight to calculate
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
            HeroAI_Controller.MyInstance.weightList.Add(weight);

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

            if (weightCalculated == HeroAI_Controller.MyInstance.MyMaxWeight)
            {
                Vector3 dir = closest.transform.position - this.transform.position;
                WeightedDirection wd = new WeightedDirection(-dir, weight);
                MyCharacter.desiredWeights.Add(wd);
            }
            else
            {
                Debug.Log("not allowed: Evade");
            }

            // //Caculate Direction for move to 
            // Vector3 dir = closest.transform.position - this.transform.position;
            // MyCharacter.MyDirection = -dir;

            // // return weight in desiredWeights List 
            // float wd = weight;
            // MyCharacter.desiredWeights.Add(wd);
            // Debug.Log("Evade triggered");
        }

    }

    private void CalculateWeight(float distanceToEnemey)
    {
        weightCalculated = Mathf.InverseLerp(0, 1, 10 / (distanceToEnemey * distanceToEnemey));
        weight = weightCalculated;
        HeroAI_Controller.MyInstance.weightList.Add(weight);
    }
}
