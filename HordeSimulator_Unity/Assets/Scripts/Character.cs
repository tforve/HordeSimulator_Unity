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
    private Vector3 velocity;                                                   // for moving calculation

    // UI related 
    public event Action<float> OnHealthChanged = delegate { };
    public event Action<float> OnManaChanged = delegate { };

    // For AI and Movement
    public string characterType;
    public Transform moveTransform;                                             // Transform just for Walking so Rotation dont mess up dir of Behaviours

    static public Dictionary<string, List<Character>> characterByType;          // Dictionary to select CharTypes only
    public List<WeightedDirection> desiredWeights;                              // list of weights to calculate highest Weight


    // Start is called before the first frame update
    void Start()
    {
        if (characterByType == null)
        {
            characterByType = new Dictionary<string, List<Character>>();
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

        //Ask all ot our AI Scripts to tell us what to do
        desiredWeights = new List<WeightedDirection>();
        BroadcastMessage("DoAIBehaviour", SendMessageOptions.DontRequireReceiver);

        Vector3 dir = Vector3.zero;

        foreach (WeightedDirection wd in desiredWeights)
        {
            if (wd.weight >= HeroAI_Controller.MyInstance.MyMaxWeight)
            {
                HeroAI_Controller.MyInstance.MyMaxWeight = wd.weight;
                dir += wd.direction * wd.weight;
            }
        }
        // Move to direction set by Behaviors 
        MoveTo(dir);
        
        HeroAI_Controller.MyInstance.MyMaxWeight = 0.0f;
    }


    // ------------- METHODES FOR AI -----------------
    // have to check to not get to fast
    public void MoveTo(Vector3 dir)
    {
        foreach (WeightedDirection wd in desiredWeights)
        {
            // NOTE: If you are implementing EXCLUSIVE/FALLBACK blend modes, check here.
            dir += wd.direction * wd.weight;

        }
        velocity = Vector3.Lerp(velocity, dir.normalized * runSpeed, Time.deltaTime * 5f);
        moveTransform.transform.Translate(velocity * Time.deltaTime);

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
