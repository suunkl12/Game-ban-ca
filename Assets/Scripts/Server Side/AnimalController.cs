using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class AnimalController : NetworkBehaviour
{
    public Animator animator;
    [SerializeField]
    private float speed = 10.0f;
    private Vector2 target;
    [SerializeField]
    private Transform tempTrans;
    private Vector2 position;
    [SerializeField]
    private int tmpX = 0, tmpY = 0;
    private bool Checked = false;
    private bool Ways = false;
    private void Awake() {
        animator= GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        if (isServer)
        {
            InitizalizeAnimalTarget();
        }
    }


    private void InitizalizeAnimalTarget()
    {
        speed = Random.Range(2f, 4f);
        //target = RandomUnitVector(-11,-11,-6,6);
        //tempTrans.GetCompoment<Transform>.CompareTag("sss");
        Checked = CheckTarget();
        target = RandomUnitVector(tempTrans.position.x - tmpX, tempTrans.position.x + tmpX, tempTrans.position.y - tmpY, tempTrans.position.y + tmpY);
    }

    // Update is called once per frame
    void Update()
    {
        if (isServer)
        {
            ToTarGetOrDead();
        }
    }

    private void ToTarGetOrDead()
    {
        if (!animator.GetBool("Dead"))
            transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
        if (transform.position == (Vector3)target)
        {
            Destroy(gameObject);
        }
    }

    public Vector2 RandomUnitVector(float formX, float toX, float formY, float toY)
    {
        float randomX = Random.Range(formX, toX);
        float randomY = Random.Range(formY, toY);
        return new Vector2(randomX, randomY);
    }
    public bool CheckTarget()
    {
        if (Ways&&!Checked)
        {
            if (!Checked && transform.position.x >= 0 && transform.position.y < 6 || transform.position.y < -6)
            {
                tempTrans = GameObject.FindGameObjectWithTag("Left").GetComponent<Transform>();
                tmpY = 5;
                return Checked = true;
            }
            else if (!Checked)
            {
                tempTrans = GameObject.FindGameObjectWithTag("Right").GetComponent<Transform>();
                tmpY = 5;
                return Checked = true;
            }
            Ways=false;
            return Checked = false;
        }
        else if(!Ways&&!Checked)
        {
            if (!Checked && transform.position.y >= 0 && transform.position.x < 11 || transform.position.x < -11)
            {
                tempTrans = GameObject.FindGameObjectWithTag("Bot").GetComponent<Transform>();
                tmpY = 0;
                tmpX = 9;
                Checked = true;
            }
            else if (!Ways && !Checked)
            {
                tempTrans = GameObject.FindGameObjectWithTag("Top").GetComponent<Transform>();
                tmpY = 0;
                tmpX = 9;
                Checked = true;
            }
            return Checked = true;
        }else{
            return Checked = true;
        }

    }
    public void Dead(){
        Destroy(gameObject);
    }

}
