using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class Ammo : NetworkBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    public Gun playerFrom;
    private Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;
    public int damage;
    public GameObject EffectExplotion;

    float speedRotate = 500f;
    public bool rotate;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if(rotate)
            transform.Rotate(Vector3.forward * speedRotate * Time.deltaTime);
        
    }
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }


    [ServerCallback]
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(EffectExplotion != null)
            EffectExplotion.transform.parent = null;

        if(other.CompareTag("Animal") && other.isTrigger)
        {
            other.GetComponent<AnimalHealth>().TakeDamage(playerFrom, damage);
            Debug.Log(damage);
            //Destroy(other.gameObject);
        }
        Destroy(gameObject);
    }
    [ClientRpc]
    public void PlayEffect()
    {
        EffectExplotion.SetActive(true);
    }
}
