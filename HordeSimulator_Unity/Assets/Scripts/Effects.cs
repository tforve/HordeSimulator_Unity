using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effects : MonoBehaviour
{

    public GameObject [] effect;
    public Transform [] effectTransform;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        var physicsMotion = GetComponentInChildren<RFX4_PhysicsMotion>(true);
        if (physicsMotion != null) physicsMotion.CollisionEnter += CollisionEnter;

	    var raycastCollision = GetComponentInChildren<RFX4_RaycastCollision>(true);
        if(raycastCollision != null) raycastCollision.CollisionEnter += CollisionEnter;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("0"))
        {
            animator.SetInteger("SkillNumber", 0);
            animator.SetTrigger("PlayerSkill");
            useEffect(0);
            
        }
        if(Input.GetKeyDown("1"))
        {
            animator.SetInteger("SkillNumber", 1);
            animator.SetTrigger("PlayerSkill");
            useEffect(1);        
        }
        if(Input.GetKeyDown("2"))
        {
            animator.SetInteger("SkillNumber", 2);
            animator.SetTrigger("PlayerSkill");
            useEffect(2);                  
        }
    }

    public void useEffect(int skillNumber)
    {
        Instantiate(effect[skillNumber], effectTransform[skillNumber].position, effectTransform[skillNumber].rotation);
    }

    private void CollisionEnter(object sender, RFX4_PhysicsMotion.RFX4_CollisionInfo e)
    {
        Debug.Log(e.HitPoint); //a collision coordinates in world space
        Debug.Log(e.HitGameObject.name); //a collided gameobject
        Debug.Log(e.HitCollider.name); //a collided collider :)
    }

}
