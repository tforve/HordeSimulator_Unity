using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public string charType = "Hero";

    [SerializeField] private Transform moveTransform;

    [SerializeField] private float attackRange = 2.0f;
    [SerializeField] private float attackSpeed = 1.5f;
    private float attackCooldown;
    [SerializeField] private float damage = 10.0f;

    private Vector3 velocity;
    public float health;
    public float runSpeed;

    // Update is called once per frame
    void Update()
    {
        CalcTarget();
    }

    void CalcTarget()
    {
        if (Character.characterByType.ContainsKey(charType) == false)
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
            if (closestChar == null || d < dist)
            {
                closestChar = c;
                dist = d;
            }

        }
        // no Hero existing
        if (closestChar == null) { return; }

        if (dist <= attackRange)
        {
            attackCooldown -= Time.deltaTime;
            if (attackCooldown <= 0.0f)
            {
                attackCooldown = attackSpeed;
                closestChar.Hit(closestChar, damage);
            }
        }
        else
        {

            Vector3 dir = closestChar.transform.position - this.transform.position;
            moveTransform.transform.Translate(velocity * Time.deltaTime);

        }
    }

    public void Hit( float dmg)
    {
        health -= dmg;
        if(health <= 0.0f)
        {
            Destroy(transform.parent.gameObject);
        }
    }
}
