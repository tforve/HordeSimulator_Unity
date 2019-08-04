using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;

public class HeroAI_Controller : MonoBehaviour
{
    /* To Do: Fire, run, Reload, Heal, Suicide */
    [Header("Decision Related")]
    public Vector3 destination;                                     // if flee or anything, walk to destination   
            
    [Header("Objects")]                                             // have to be changed autamtic in Range of AI bzw. nearest to 
    public Transform targetEnemy;                                   // Enemyto Target 
    public GameObject targetItem;                                   // Item to get
    public Transform idleObject;                                    // start Object. DELETE ME LATER

    [Header("Scoring System")]
    [Range(0,1)][SerializeField]    private float score;            // score calculated to choose action
    [SerializeField]                private bool veto = false;      // if true action can not be executed, if true utility = 0

    private NavMeshAgent agent;

    [Header("Sense")]
    public float checkRadius = 5.0f;
    public LayerMask checkLayers;
    
    private Animator animator;
    // ---------------------------------------------

    void Start()
    {
        targetEnemy = idleObject;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        destination = targetEnemy.transform.position;
    }

    void Update()
    {
        SearchEnemyTarget();
        
        // save for Idle
        if(targetEnemy == null)
        {
            targetEnemy = idleObject;
        }

        float distanceToTarget = Vector3.Distance(this.transform.position , targetEnemy.transform.position); // calculate distance to target need ifstatement for range
        this.LookAt(this.transform, targetEnemy.transform);

        if(distanceToTarget < 3.0f)
        {
            //ENEMY ATTACK PLAYER OR/AND PLAYER CAN CAST AT ENEMY
        }

        if(Input.GetButtonDown("1"))
        {
            animator.SetTrigger("UseSkill");
            animator.SetInteger("SkillNumber",0);
        }
        
    }

    //Rotate HeroAI to Target
    public void LookAt(Transform heroAI, Transform tar)
    {
        float speed = 2.5f;
        Vector3 direction = (heroAI.position - tar.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(-direction); // minus direction... maybe modle is turend wrong 
        heroAI.rotation = Quaternion.Slerp(heroAI.rotation, lookRotation, Time.deltaTime * speed);
    }

    // Search for new Target
    public void SearchEnemyTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, checkRadius, checkLayers);
        Array.Sort(colliders,new DistanceComparer(transform));
        if(colliders.Length != 0){targetEnemy = colliders[0].transform; }
        
    }

    // Draw CheckRadius of Hero Sense for Debugging
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position, checkRadius);
    }


    // --------------------------------------------
    /* AI Action Methodes */
    // Move to target
    void MoveTo(Vector3 destination)
    {

    }

    // attack target
    void Attack(Transform target)
    {
        targetEnemy = target;
    }

    // collect Item
    void CollectItem()
    {
        // mini inventory system? or just walk over it 
    }

    //use Item
    void UseItem(GameObject item)
    {
        //if hp low use healthpotion
    }
}   
