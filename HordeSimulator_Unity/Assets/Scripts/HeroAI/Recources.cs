using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recources : MonoBehaviour
{

    [Header("scorable Values")]
    public float health = 100.0f;
    public float ammo = 60.0f;
    public float magazineCapacity = 20.0f;
    public float fear = 0.0f;

    [SerializeField]private bool isDead = false;
    private bool canReload = true;
    private bool canHeal = true;

    void Start()
    {

    }

    public void ReloadWeapon()
    {
        
        if(ammo >=20)
        {
            ammo -= 20;
            magazineCapacity = 20;
        }
        else if (ammo < 20)
        {
            magazineCapacity = ammo;
        }
        if(ammo == 0)
        {
            Debug.Log("No Ammo");
        }
    }

    public void Heal(float potion)
    {
        if(canHeal)
        {
            health += potion;
            if(health > 100.0f) health = 100.0f;
        }
    }

    public void Death()
    {
        isDead = true;
    }
}
