using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_SeekMana : MonoBehaviour
{
    public CharacterType charType = CharacterType.MANAPOTION;

    public float potionSize = 20;
    public float collectingRange = 1.0f;

    public float weight = 2.0f;

    public float MyWeight
    {
        get {return weight;}
    }

    Character MyCharacter;

    void Start()
    {
        MyCharacter = GetComponent<Character>();
    }

    void DoAIBehaviour()
    {
        if(Character.characterByType.ContainsKey(charType) == false)
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
            if(closest == null || d<dist)
            {
                closest = c;
                dist = d;
            }

        }
        // no Potion existing
        if(closest == null){ return;}

        CalculateWeight();

        if(dist < collectingRange)
        {
            MyCharacter.RestoreMana(potionSize);
            Destroy(closest.gameObject);
        }
        else
        {
            Vector3 dir = closest.transform.position - this.transform.position;
			WeightedDirection wd = new WeightedDirection( dir, weight );
			MyCharacter.desiredDirections.Add( wd );
           // MyCharacter.MoveTo();
        }
    }

    private float CalculateWeight()
    {
        return weight;
    }
}
