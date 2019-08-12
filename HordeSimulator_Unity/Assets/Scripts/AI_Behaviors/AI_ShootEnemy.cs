using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_ShootEnemy : MonoBehaviour
{
    public string charType = "Enemy";

    [SerializeField] private float damage = 10.0f;          //  projectile later
    [SerializeField] private float manaCost = 5.0f;
    [SerializeField] private float attackCooldown = 2.15f;
    private bool canAttack = true;                          // for coolDown

    private float weight;                                   // weight given to Character for Decision making, different to calculated because of Veto
    public float weightCalculated = 0.0f;                   // weight to calculate
    public bool veto = false;                               // if true AI Action not executed

    public float MyWeight
    {
        get { return weight; }
    }

    Character MyCharacter;
    Character target;


    void Start()
    {
        MyCharacter = GetComponent<Character>();
    }

    void DoAIBehaviour()
    {
        // Check Veto to not execute.... MAYBE RETURN IS OK, to not calc everything
        if (veto)
        {
            weight = 0.0f;
        }
        // Go on and execute AI_Behavior
        else
        {
            CalculateWeight();
        }

        // get Target from HeroAI_Controller LookAt()
        target = HeroAI_Controller.MyInstance.MyTargetEnemy;

        if (Character.characterByType.ContainsKey(charType) == false) { return; }
        
        if (target == null)
        {
            weight = 0.0f;
            weightCalculated = 0.0f;
            Vector3 dir = Vector3.zero;
            WeightedDirection wd = new WeightedDirection(dir, weight);
        }
        else
        {
            AttackTarget();
            Vector3 dir = Vector3.zero;
            WeightedDirection wd = new WeightedDirection(dir, weight);
            MyCharacter.desiredWeights.Add(wd);

        }

    }

    private void AttackTarget()
    {
        // Attack target
        if (canAttack && MyCharacter.mana >= manaCost)
        {
            // play Animations
            HeroAI_Controller.MyInstance.animator.SetTrigger("UseSkill");
            HeroAI_Controller.MyInstance.animator.SetInteger("SkillNumber", 0);

            // deal damage. has to be changed to hit collider            
            target.Hit(target, damage);
            canAttack = false;
            // start cooldown
            StartCoroutine(StartCooldown());
            MyCharacter.RestoreMana(-manaCost);
        }
    }

    private void CalculateWeight()
    {
        weightCalculated = 0.0f;
        float tmp = 0.0f;
        foreach (Character enemy in HeroAI_Controller.MyInstance.listOfEnemies)
        {
            tmp += 0.2f; // need function here
        }

        weightCalculated = Mathf.InverseLerp(0, 1, tmp);
        weight = weightCalculated;
    }

    IEnumerator StartCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
        yield break;
    }
}
