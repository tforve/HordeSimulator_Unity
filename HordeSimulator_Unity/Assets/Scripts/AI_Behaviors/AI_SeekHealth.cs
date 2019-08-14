using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_SeekHealth : MonoBehaviour
{
    public string charType = "Healthpotion";

    public float potionSize = 20;
    public float collectingRange = 1.0f;

    private float weight;                                           // weight given to Character for Decision making, different to calculated because of Veto
    public float weightCalculated = 0.0f;                           // weight to calculate

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

    void DoAIBehavior()
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
        // no Potion existing but my weight stays the same because my Mana is the condition
        if (closest == null)
        {
            return;
        }

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
            UIController.MyInstance.SetSliderValue(0.0f);


        }

    }

    private void CalculateWeight()
    {
        // h = max HP, x = current HP
        // 0.5 sin(PI/h * (x + h/2)) + 0.5
        float expoTmp = (0.5f * Mathf.Sin((3.14159f / MyCharacter.maxHealth) * (MyCharacter.health + (MyCharacter.maxHealth * 0.5f))) + 0.5f);
        weightCalculated = expoTmp;
        weight = weightCalculated;
    }
}
