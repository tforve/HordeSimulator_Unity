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
}
