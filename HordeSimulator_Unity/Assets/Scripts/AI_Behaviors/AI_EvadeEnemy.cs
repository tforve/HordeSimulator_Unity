using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_EvadeEnemy : MonoBehaviour
{
    public CharacterType charType = CharacterType.ENEMY;
    public ImportanceLevel importanceLevel = ImportanceLevel.ALLWAYS;
    public float rangeOfCare = 3.0f;
    public float weight = 2.0f;

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
        Character closestChar = null;
        float dist = Mathf.Infinity;

        foreach (Character c in Character.characterByType[charType])
        {
            float d = Vector3.Distance(this.transform.position, c.transform.position);
            if(closestChar == null || d<dist)
            {
                closestChar = c;
                dist = d;
            }

        }
        // no Potion existing
        if(closestChar == null){ return;}
        Vector3 dir = closestChar.transform.position - this.transform.position;
        dir *= -1;
        
        // Do weight Calculation HERE 
        // 10/dist^2
        weight = 10 / (dist*dist);        
        
        WeightedDirection wd = new WeightedDirection( dir, weight ); //1 is the weight
		MyCharacter.desiredDirections.Add( wd );
        
    }
}
