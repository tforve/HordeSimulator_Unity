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

    Vector3 currenPos;


    static public Dictionary<string, List<Character>> characterByType;          // Dictionary to select CharTypes only
    public List<WeightedDirection> enemyAIList;
    public List<WeightedDirection> desiredWeights;                              // list of weights to calculate highest Weight

    // Animation
    public Animator animator;


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

        // animation
        animator = GetComponent<Animator>();
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
        // so enemy move and can use same class
        enemyAIList = new List<WeightedDirection>();
        BroadcastMessage("DoEnemyBehavior", SendMessageOptions.DontRequireReceiver);

        Vector3 dir = Vector3.zero;

        foreach (WeightedDirection wd in desiredWeights)
        {
            // Check for Blending HERE
            if (desiredWeights.Count == 0) { return; }
            if (wd.weight > HeroAI_Controller.MyInstance.MyMaxWeight)
            {
                HeroAI_Controller.MyInstance.MyMaxWeight = wd.weight;
                dir = wd.direction * wd.weight;
            }
        }
        // Move to direction set by Behaviors 
        MoveTo(dir);


        // EnemyAI Only
        Vector3 enemyDir = Vector3.zero;
        foreach (WeightedDirection wd in enemyAIList)
        {
            enemyDir = wd.direction * wd.weight;
        }

        MoveTo(enemyDir);

        if (HeroAI_Controller.MyInstance != null)
        {
            HeroAI_Controller.MyInstance.MyMaxWeight = 0.0f;
        }

        // DMG
        if (health <= 0.0f)
        {            
            moveTransform.transform.position = currenPos;
        }

    }


    // ------------- METHODES FOR AI -----------------
    // have to check to not get to fast
    public void MoveTo(Vector3 dir)
    {
        animator.SetBool("isWalking", true);

        velocity = Vector3.Lerp(velocity, dir.normalized * runSpeed, Time.deltaTime * 5f);
        moveTransform.transform.Translate(velocity * Time.deltaTime);
    }


    /* Here Look At nochmal betrachten und anpassen */
    public void LookAt(Transform target)
    {
        Vector3 direction = target.position.normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        moveTransform.transform.rotation = Quaternion.Slerp(moveTransform.transform.rotation, lookRotation, Time.deltaTime * 5.5f);
    }

    public void Hit(Character target, float dmg)
    {
        health -= dmg;
        float currentHealthPct = health / maxHealth;
        OnHealthChanged(currentHealthPct);

        if (health <= 0.0f)
        {
            UIController.MyInstance.killCount += 1;
            animator.SetTrigger("isDead");
            currenPos = moveTransform.transform.position;
            StartCoroutine(WaitDeath());
        }
    }

    IEnumerator WaitDeath()
    {
        yield return new WaitForSeconds(2.0f);
        Destroy(transform.parent.gameObject);
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
