using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HeroAI_Controller : MonoBehaviour
{
    /* To Do: Fire, run, Reload, Heal, Suicide */
    [Header("Decision Related")]
    public Vector3 destination;                                     // if flee or anything, walk to destination   
            
    [Header("Objects")] // have to be changed autamtic in Range of AI bzw. nearest to 
    public Enemy targetEnemy;                                       // Enemyto Target 
    public GameObject targetItem;                                   // Item to get

    [Header("Scoring System")]
    [Range(0,1)][SerializeField]    private float score;            // score calculated to choose action
    [SerializeField]                private bool veto = false;      // if true action can not be executed, if true utility = 0

    private NavMeshAgent agent;

    // ---------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        destination = targetEnemy.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToTarget = Vector3.Distance(this.transform.position , targetEnemy.transform.position); // calculate distance to target need ifstatement for range
        this.LookAt(this.transform, targetEnemy.transform);

        if(distanceToTarget < 3.0f)
        {
            //ENEMY ATTACK PLAYER OR/AND PLAYER CAN CAST AT ENEMY
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

    /* AI Action Methodes */
    // Move to target
    void MoveTo(Vector3 destination)
    {

    }

    // attack target
    void Attack(Enemy target)
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
