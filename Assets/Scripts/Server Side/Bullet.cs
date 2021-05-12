using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    bool isHit;
    int damage =50;
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D other) {
        try{
        other.gameObject.GetComponent<ITakeDamage>().TakeDamage(damage);
        }catch(System.Exception ex){
            Debug.LogError(ex.Message);
        }
    }
}
