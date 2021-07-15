using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalHealth : MonoBehaviour
{
    public int health;
    public int currentHealth;
    public int score;
    Animator ani;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = health;
        ani = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(currentHealth != health)
        {
            //TODO gửi về server máu của animal
            currentHealth = health;
        }    

    }

    public void TakeDamage(Gun playerFrom, int damage)
    {
        health -= damage;
        //playerFrom.GetComponent<Gun>().Score += score;
        if (health <= 0)
        {
            //playerFrom.GetComponent<Gun>().Score += score;
        }
    }

    public void StartDead()
    {
        ani.SetBool("Dead", true);
    }
    public void Dead()
    {
        Destroy(gameObject);
    }

}
