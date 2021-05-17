using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class EffectController : NetworkBehaviour
{
    // Start is called before the first frame update    GameObject smokePuff;
    public Ammo ammo;
    public GameObject Effect;
    void Start()
    {
        ammo = GetComponentInParent<Ammo>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Animal") && other.isTrigger)
        {
            other.GetComponent<AnimalHealth>().TakeDamage(ammo.playerFrom, ammo.damage);
            Debug.Log(ammo.damage);
        }
    }
}