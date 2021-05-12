using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalManager : NetworkBehaviour
{
    // Start is called before the first frame update
    
    public List<GameObject> AnimalUnit;
    public float reSpawnWaitTime;
    public List<Transform> nodes;
    private int rnd;
    private bool wt= false;

    void Start()
    {
        
        StartCoroutine(WaitFor());
    }
    IEnumerator WaitFor()
    {
        wt = true;
        // process pre-yield
        yield return new WaitForSeconds( reSpawnWaitTime );
        // process post-yield
        wt = false;
    }
    // Update is called once per frame
    
    void Update()
    {
        if(!wt && isServer)
        {

            foreach (Transform t in nodes)
            {
                // server gửi về tên animal, target( vector3 x,y,z), speed float , position x,y
                RpcSpawnAnimal(t.position);

            }
            StartCoroutine( WaitFor() );
        }
        //yield on a new YieldInstruction that waits for 5 seconds.
            
    }

    private void RpcSpawnAnimal(Vector3 SpawnPosition)
    {
        rnd = Random.Range(0, AnimalUnit.Count);
        var animal = Instantiate(AnimalUnit[rnd], SpawnPosition, Quaternion.identity);
        NetworkServer.Spawn(animal);
    }

    public void RpcMoveToTarget(Transform animal, Vector2 target, float speed)
    {
        while (animal.position != (Vector3)target)
        {
            animal.position = Vector2.MoveTowards(animal.position, target, speed * Time.deltaTime);
            if (animal.position == (Vector3)target)
            {
                Destroy(gameObject);
                return;
            }
        }
    }
}
