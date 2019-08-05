using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_ShootEnemy : MonoBehaviour
{
    public CharacterType charType = CharacterType.ENEMY;

    [SerializeField] private float damage = 10.0f; //  projectile later
    [SerializeField] private float manaCost = 5.0f;
    [SerializeField] private float attackCooldown = 2.15f;
    private bool canAttack = true;

    public float weight = 2.0f;

    public float MyWeight
    {
        get {return weight;}
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

        Vector3 dir = Vector3.zero;
        WeightedDirection wd = new WeightedDirection( dir, weight );
		MyCharacter.desiredDirections.Add( wd );
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
