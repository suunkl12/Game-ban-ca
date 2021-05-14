using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class EffectController : NetworkBehaviour
{
    // Start is called before the first frame update    GameObject smokePuff;
    public GameObject Effect;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Effect.transform.parent = null;
        if(other.gameObject.CompareTag("Animal"))
        {
            Animator ani = other.GetComponent<Animator>();
            ani.SetBool("Dead", true);
            other.GetComponent<CircleCollider2D>().enabled = false;
        }    
    }
}
