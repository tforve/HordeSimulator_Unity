using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public enum CharacterType { ENEMY, HERO, HEALTHPOTION, MANAPOTION }

public class HeroAI_Controller : MonoBehaviour
{
    private static HeroAI_Controller _instance;
    public static HeroAI_Controller MyInstance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<HeroAI_Controller>();
            }
            return _instance;
        }
    }


    [Header("Objects")]                                             // have to be changed autamtic in Range of AI bzw. nearest to 
    public Transform targetLookAt;                                  // Enemyto Target
    public Transform targetMoveTo;                                  // Changed by AI_MoveTo
    public Transform idleObject;                                    // start Object. DELETE ME LATER

    private Character targetEnemy;

    [Header("Scoring System")]
    [SerializeField] private float weight;                           // score calculated to choose action
    [SerializeField] private bool veto = false;                     // if true action can not be executed, if true utility = 0

    //UI Debug Stuff
    [SerializeField] private Text heighestTxt, evadeTxt, healthTxt, manaTxt, shootTxt;                         // for debuging purpose
    // private AI_EvadeEnemy MyAi_Evade;
    // private AI_SeekHealth MyAi_SeekHeal;
    private AI_SeekMana MyAi_SeekMana;
    private AI_ShootEnemy MyAi_Shoot;

    private Character MyCharacter;

    [Header("Sense for lookAt Only")]
    public float checkRadius = 25.0f;
    public float turnSpeed = 5.5f;
    public LayerMask checkLayers;

    // others
    private NavMeshAgent agent;
    [HideInInspector] public Animator animator;

    // Get and Set Target etc
    public float MyWeight
    {
        get { return weight; }
        set { weight = value; }
    }
    public bool MyVeto
    {
        get { return veto; }
        set { veto = value; }
    }
    public Character MyTargetEnemy
    {
        get { return targetEnemy; }
    }

    // ---------------------------------------------

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    void Start()
    {
        targetLookAt = idleObject;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        MyCharacter = GetComponent<Character>();

        //Debug
        // MyAi_Evade = GetComponent<AI_EvadeEnemy>();
        // MyAi_SeekHeal = GetComponent<AI_SeekHealth>();
        MyAi_SeekMana = GetComponent<AI_SeekMana>();
        MyAi_Shoot = GetComponent<AI_ShootEnemy>();

    }

    void Update()
    {
        SearchEnemyTarget();

        // save for Idle
        if (targetLookAt == null)
        {
            targetLookAt = idleObject;
        }

        float distanceToTarget = Vector3.Distance(this.transform.position, targetLookAt.transform.position);


        // Debuggin to get highest Weight/ Score and update Text
        foreach (WeightedDirection wd in MyCharacter.desiredDirections)
        {
            float tmp = wd.weight;
            weight = tmp;
        }
        heighestTxt.text = "Decision: " + weight;
        // evadeTxt.text = "Evade: " + MyAi_Evade.MyWeight;
        // healthTxt.text = "Heal: " + MyAi_SeekHeal.MyWeight;
        manaTxt.text = "Mana: " + MyAi_SeekMana.MyWeight;
        shootTxt.text = "Fight: " + MyAi_Shoot.MyWeight;

    }

    void OnDestroy()
    {
        Debug.Log("Game OVER");
        // Show EndScreen
    }

    // LEAVE IT IN BECAUSE IS DONE ALL THE TIME
    // Search for new Target to LookAt
    public void SearchEnemyTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, checkRadius, checkLayers);
        Array.Sort(colliders, new DistanceComparer(transform));
        if (colliders.Length != 0)
        {
            targetLookAt = colliders[0].transform;
            targetEnemy = colliders[0].GetComponent<Character>();
            LookAt(targetLookAt.transform);
        }
    }
    //Rotate HeroAI to Target
    void LookAt(Transform target)
    {
        Vector3 direction = target.position.normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
    }

    // Draw CheckRadius of Hero Sense for Debugging
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position, checkRadius);
    }
}
