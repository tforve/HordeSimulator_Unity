using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float healthPoints = 20.0f;
    [SerializeField] private float damage;

    void Start()
    {
        
    }

    void Update()
    {
        if(healthPoints <= 0.0f)
        {
            Destroy(this);
        }
    }
}
