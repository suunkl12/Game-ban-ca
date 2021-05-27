using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    //id dùng để nhận diện súng của người nào
    public int id;
    public bool controllable;

    //float currentTime;
    //float shootCooldown = 0.5f;
    public Animator m_animator;

    private bool isShoot;

    private void Start()
    {
        m_animator = GetComponent<Animator>();
    }
    private void FixedUpdate()
    {
        isShoot = false;
        Shoot();
        StopShoot();
    }

    private void StopShoot()
    {
        if (m_animator != null && !isShoot)
            m_animator.SetBool("shoot", false);
    }

    public void Shoot()
    {
        
        if (!controllable) return;
        if (Input.touchCount <= 0) return;

        var input = Input.GetTouch(0);
        if ( input.phase == TouchPhase.Began || input.phase == TouchPhase.Stationary || input.phase == TouchPhase.Moved)
        {
            var touchPos = Camera.main.ScreenToWorldPoint(input.position);

            var degree = Vector2.SignedAngle(Vector2.right, (touchPos - transform.position)) - 90;
            transform.rotation =Quaternion.Euler(
                transform.rotation.x,
                transform.rotation.y,
                degree);

            isShoot = true;

            if(m_animator!=null)
                m_animator.SetBool("shoot", true);

            new PlayerShootPacket(id, degree).Write();

        }
    }

    public void RpcShoot(float rotation)
    {

        //Debug.Log("Other shooting");
        if (controllable) return;
            transform.rotation = Quaternion.Euler(
                transform.rotation.x,
                transform.rotation.y,
                rotation);

        isShoot = true;

        if (m_animator != null)
            m_animator.SetBool("shoot", true);
    }


}
