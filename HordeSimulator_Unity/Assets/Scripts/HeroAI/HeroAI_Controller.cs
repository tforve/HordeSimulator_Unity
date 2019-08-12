using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


//public enum CharacterType {ENEMY, HERO, HEALTHPOTION, MANAPOTION }

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
    public Transform idleObject;                                    // start Object. DELETE ME LATER

    [SerializeField] private Character targetEnemy;                 // closest Enemy and biggest Threat
    public List<Character> listOfEnemies;                           // List of all Enemies in Radius

    private float maxWeight = 0.0f;                                 // score calculated to choose action

    private Character MyCharacter;

    [Header("Sense for lookAt Only")]
    public float checkRadius = 25.0f;
    public float turnSpeed = 5.5f;
    public LayerMask checkLayers;

    // others
    private NavMeshAgent agent;
    [HideInInspector] public Animator animator;

    // Get and Set Target etc
    public float MyMaxWeight
    {
        get { return maxWeight; }
        set { maxWeight = value; }
    }

    public Character MyTargetEnemy // is used in AI_ShootEnemy to get target to shoot at
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
    }

    void Update()
    {
        SearchEnemyTarget();

        // save for Idle
        if (targetLookAt == null)
        {
            targetLookAt = idleObject;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    void OnDestroy()
    {
        UIController.MyInstance.SetBtnActive();
    }

    // LEAVE IT IN BECAUSE IS DONE ALL THE TIME
    // Search for new Target to LookAt
    public void SearchEnemyTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, checkRadius, checkLayers);

        listOfEnemies.Clear();
        foreach (Collider c in colliders)
        {
            listOfEnemies.Add(c.GetComponent<Character>());
        }

        Array.Sort(colliders, new DistanceComparer(transform));
        if (colliders.Length != 0)
        {
            targetLookAt = colliders[0].transform;
            targetEnemy = colliders[0].GetComponent<Character>();
        }
    }
    //Rotate HeroAI to Target
    void LookAt(Transform target)
    {
        Vector3 direction = target.position.normalized;
        Quaternion lookRotation = Quaternion.LookRotation(target.position);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
    }

    // Draw CheckRadius of Hero Sense for Debugging
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position, checkRadius);
    }
}
