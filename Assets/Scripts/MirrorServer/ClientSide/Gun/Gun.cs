using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Mirror;
public class Gun : NetworkBehaviour
{
    [SyncVar]
    public bool inUsed = false;

    public Game_Manager game_Manager;
    public GameObject bullet;
    public float bulletSpeed;
    public Transform firePoint;
    Rigidbody2D rb2d;

    [SyncVar]
    private int bulletAmount;

    public int maxBulletAmount;
    //float speedRotate = 5f;
    public float fireRate = 0.1f;
    private float nextFire = 0.0f;
    public Animator ani;

    //Cái listener để khi bắn, sẽ gửi data về server
    public GameEvent gameEvent;

    public int BulletAmount { get => bulletAmount; set => bulletAmount = value; }

    // Start is called before the first frame update
    void Start()
    {
        if (isServer)
        {
            bulletAmount = maxBulletAmount;
        }
        rb2d = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()

    {   if (isServer||!hasAuthority){
            return;
        }
        if (game_Manager.playerIn == this && Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(0).phase == TouchPhase.Stationary || Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                {
                    Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                    if (bulletAmount <= 0)
                        Debug.LogWarning("There is no bullet left to fire");
                    CmdShoot(touchPos);
                }
            }
        }
        else if (ani.GetBool("shoot"))
        {
            CmdStopAnimation();
        }
    }

    [Command]
    private void CmdStopAnimation()
    {
        ani.SetBool("shoot", false);
    }

    [Command]
    public void CmdShoot(Vector2 touchPos)
    {
        
        if ((float)NetworkTime.time > nextFire && bulletAmount > 0)
        {
            ani.SetBool("shoot", true);
            nextFire = (float)NetworkTime.time + fireRate;
            
            Vector2 lookdir = touchPos - rb2d.position;
            float angle = Mathf.Atan2(lookdir.y, lookdir.x) * Mathf.Rad2Deg - 90f;
            rb2d.rotation = angle;

            Vector2 firedir = touchPos - (new Vector2(firePoint.position.x, firePoint.position.y));
            firedir.Normalize();
            GameObject bullet_ = Instantiate(bullet, firePoint.transform.position, Quaternion.identity);
            NetworkServer.Spawn(bullet_);
            bullet_.GetComponent<Rigidbody2D>().velocity = firedir * bulletSpeed;
            bulletAmount--;
        }
    }



    public void SetGunInUsed()
	{
        inUsed = true;
    }
    public void ResetGun()
    {
        inUsed = false;
        bulletAmount = maxBulletAmount;
    }

    public void RotateGun(float rotation)
    {
        
        rb2d.rotation = rotation;
    }
}
