using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class Ammo : NetworkBehaviour, ITakeDamage
{
    // Start is called before the first frame update
    [SerializeField]
    private Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;

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

    public void TakeDamage(int damage)
    {
        throw new System.NotImplementedException();
    }

    [ServerCallback]
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isServer)
            return;
        spriteRenderer.sprite = null;
        
        EffectExplotion.SetActive(true);
        playEffect();
        if(other.gameObject.CompareTag("Animal"))
        {
            rb.velocity = transform.position*0;
            Animator ani = other.GetComponent<Animator>();
            ani.SetBool("Dead", true);
            other.GetComponent<CircleCollider2D>().enabled = false;
            Destroy(this.gameObject,0.2f);
        }    
    }

    [ClientRpc]
    public void playEffect()
    {
        EffectExplotion.SetActive(true);
    }
}
