using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_ShootEnemy : MonoBehaviour
{
    public CharacterType charType = CharacterType.ENEMY;

    [SerializeField] private float damage = 10.0f;          //  projectile later
    [SerializeField] private float manaCost = 5.0f;
    [SerializeField] private float attackCooldown = 2.15f;
    private bool canAttack = true;                          // for coolDown

    private float weight;                                   // weight given to Character for Decision making, different to calculated because of Veto
    public float weightCalculated = 2.0f;                   // weight to calculate
    public bool veto = false;                               // if true AI Action not executed

    public float MyWeight
    {
        get { return weight; }
    }

    Character MyCharacter;
    HeroAI_Controller HeroAI;
    Character target;

    void Start()
    {
        MyCharacter = GetComponent<Character>();
        HeroAI = GetComponent<HeroAI_Controller>();
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
            weight = weightCalculated;
        }

        // get Target from HeroAI_Controller LookAt()
        target = HeroAI.MyTargetEnemy;

        if (Character.characterByType.ContainsKey(charType) == false) { return; }
        if (target == null) { return; }

        // calculate weight
        CalculateWeight();

        // Attack target
        if (canAttack && MyCharacter.mana >= manaCost)
        {
            // play Animations
            HeroAI.animator.SetTrigger("UseSkill");
            HeroAI.animator.SetInteger("SkillNumber", 0);

            // deal damage. has to be changed to hit collider
            target.Hit(target, damage);
            canAttack = false;
            // start cooldown
            StartCoroutine(StartCooldown());
            MyCharacter.RestoreMana(-manaCost);
        }

        WeightedDirection wd = new WeightedDirection(weight);
        MyCharacter.desiredDirections.Add(wd);
        Debug.Log("AI_ShootEnemy Triggered");

    }

    private float CalculateWeight()
    {
        return weight;
    }

    IEnumerator StartCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
        yield break;
    }
}
