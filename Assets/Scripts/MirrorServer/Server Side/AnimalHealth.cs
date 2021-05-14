using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalHealth : MonoBehaviour
{
    public int health;
    public int currentHealth;
    public int score;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentHealth != health)
        {
            //TODO gửi về server máu của animal
            currentHealth = health;
        }    
        if(health <= 0)
        {
            Animator ani = GetComponent<Animator>();
            ani.SetBool("Dead", true);
        }
    }

    public void TakeDamage(GameObject playerFrom, int damage)
    {
        health -= damage;
        playerFrom.GetComponent<Gun>().Score += score;
    }    
}
