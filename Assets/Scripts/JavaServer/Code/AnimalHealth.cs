using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalHealth : MonoBehaviour
{
    private int id;
    public int health;
    public int currentHealth;
    public int score;
    Animator ani;

    public int Id { get => id; set => id = value; }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = health;
        ani = GetComponent<Animator>();
        StartCoroutine(timeOutDestroyFish(id));
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

    IEnumerator timeOutDestroyFish(int id)
    {
        yield return new WaitForSeconds(40f);

        if (!gameObject.activeSelf) yield break;
        if (Client.instance.animals.ContainsKey(id)) Client.instance.animals.Remove(id);
        StartDead();
        

    }

}
