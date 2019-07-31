using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HeroAI_Controller : MonoBehaviour
{
    /* To Do: Fire, run, Reload, Heal, Suicide */
    [Header("Testing only")]
    public Vector3 destination;     
            
    [Header("Objects")]
    public GameObject targetObject;                                 //Object to Target (enemy to shoot, ammo to get, health to collect)

    [Header("Scoring System")]
    [Range(0,1)][SerializeField]    private float score;            // score calculated to choose action
    [SerializeField]                private bool veto = false;      // if true action can not be executed, if true utility = 0

    private NavMeshAgent agent;

    // ---------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        destination = targetObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(destination);
    }

    /* AI Action Methodes */
    // Move to target
    void MoveTo(Vector3 destination)
    {

    }

    // attack target
    void Attack(GameObject target)
    {
        targetObject = target;
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
