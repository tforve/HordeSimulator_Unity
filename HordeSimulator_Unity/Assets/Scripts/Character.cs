using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("Character Resources")]
    public float health;
    public float maxHealth = 100.0f;
    public float mana;
    public float maxMana = 100.0f;
    [Space]
    public float runSpeed = 3.0f;
    private Vector3 velocity;       // for moving calculation

    // UI related 
    public event Action<float> OnHealthChanged = delegate { };
    public event Action<float> OnManaChanged = delegate { };

    // For AI and Movement
    public CharacterType characterType;
    public Transform moveTransform;                                             // Transform just for Walking so Rotation dont mess up dir of Behaviours
   // public Vector3 dir;                                                        // overall direction set with setter 

    static public Dictionary<CharacterType, List<Character>> characterByType;   // Dictionary to select CharTypes only
    
    public List<WeightedDirection> desiredWeights;                                          // list of weights to calculate highest Weight

    // public Vector3 MyDirection
    // {
    //     set{dir = value;}
    // }

    // Start is called before the first frame update
    void Start()
    {
        if (characterByType == null)
        {
            characterByType = new Dictionary<CharacterType, List<Character>>();
        }
        if (characterByType.ContainsKey(characterType) == false)
        {
            characterByType[characterType] = new List<Character>();
        }
        characterByType[characterType].Add(this);

        // StartValues
        health = maxHealth;
        mana = maxMana;
    }

    void OnDestroy()
    {
        characterByType[characterType].Remove(this);
    }

    void Update()
    {
        //Save checkers
        if (mana <= 0.0f) { mana = 0.0f; }

        desiredWeights = new List<WeightedDirection>();

        //Ask all ot our AI Scripts to tell us what to do
        BroadcastMessage("DoAIBehaviour", SendMessageOptions.DontRequireReceiver);

        // Move to direction set by Behaviors 
        MoveTo();
        //dir = Vector3.zero;
    }
    

    // ------------- METHODES FOR AI -----------------
    // have to check to not get to fast
    public void MoveTo()
    {
        Vector3 dir = Vector3.zero;
		foreach(WeightedDirection wd in desiredWeights) {
			// NOTE: If you are implementing EXCLUSIVE/FALLBACK blend modes, check here.

			dir += wd.direction * wd.weight;
		}


		velocity = Vector3.Lerp(velocity, dir.normalized * runSpeed, Time.deltaTime * 5f);
		moveTransform.transform.Translate( velocity * Time.deltaTime );

    }
    
    public void Hit(Character target, float dmg)
    {
        health -= dmg;
        float currentHealthPct = health / maxHealth;
        OnHealthChanged(currentHealthPct);

        if (health <= 0.0f) 
        { 
            Destroy(transform.parent.gameObject);
            return; 
        }
    }

    public void RestoreHealth(float amount)
    {
        health += amount;
        float currentHealthPct = health / maxHealth;
        OnHealthChanged(currentHealthPct);
        if (health > maxHealth) { health = maxHealth; }
    }

    public void RestoreMana(float amount)
    {
        mana += amount;
        float currentManaPct = mana / maxMana;
        OnManaChanged(currentManaPct);
        if (mana > maxMana) { mana = maxMana; }
    }
}
