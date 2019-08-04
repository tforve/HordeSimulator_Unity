using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Character : MonoBehaviour
{
    [Header("Character Resources")]
    public float health = 100.0f;
    public float maxHealth = 100.0f;
    public float mana = 50.0f;
    public float maxMana = 100.0f;
    [Space]

    public float runSpeed = 3.0f;
    private Vector3 velocity;       // for moving calculation

    public CharacterType characterType;
    
    static public Dictionary<CharacterType, List<Character>> characterByType;   // Dictionary to select CharTypes
    public List<WeightedDirection> desiredDirections;                           // direction character wants to move to

    public ImportanceLevel importanceLevel = ImportanceLevel.NORMAL;            // to order the Behaviours by more importend to less


    // Start is called before the first frame update
    void Start()
    {
        if(characterByType == null)
        {
            characterByType = new Dictionary<CharacterType, List<Character>>();
        }
        if(characterByType.ContainsKey(characterType)==false)
        {
            characterByType[characterType] = new List<Character>();
        }
        characterByType[characterType].Add(this);
    }

    void OnDestroy()
    {
        characterByType[characterType].Remove(this);
    }

    void Update()
    {
        //Save checkers
        if(mana <= 0.0f) { mana = 0.0f; }

        //Ask all ot our AI Scripts to tell us what to do
        desiredDirections = new List<WeightedDirection>();
        BroadcastMessage("DoAIBehaviour", SendMessageOptions.DontRequireReceiver);

       // MoveTo();
        
    }

    public void MoveTo()
    {
        // Add up all the desired directions by weight
		Vector3 dir = Vector3.zero;
		foreach(WeightedDirection wd in desiredDirections) 
        {
            if(importanceLevel == ImportanceLevel.ALLWAYS)
            {
                // DO THIS OVER ALL
            }
            else if( importanceLevel == ImportanceLevel.ATLAST)
            {
                // do this only if nothing else is going on
            }
            else
            {
			    dir += wd.direction * wd.weight;
            }
		}
        // smooth out movement
		velocity = Vector3.Lerp(velocity, dir.normalized * runSpeed, Time.deltaTime * 5f);

		// Move in the desired direction at our top speed.
		transform.Translate( velocity * Time.deltaTime );
    }


    // Methodes to call from AI_Behaviours
    public void Hit(Character target, float dmg)
    {
        Debug.Log("Char:" + this.name + " got hit for: " + dmg);
        health -= dmg;
        if(health <= 0.0f)  { Destroy(gameObject); return; }
    }

    public void RestoreHealth(float amount)
    {   
        health += amount;
        if(health > maxHealth) { health = maxHealth; }
    }

    public void RestoreMana(float amount)
    {
        mana += amount;
        if(mana > maxMana) { mana = maxMana; }
    }
}
