using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ChangePosition : NetworkBehaviour
{
    // Start is called before the first frame update

    
    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            transform.position = (Vector2)Camera.main.ScreenToWorldPoint((Vector2)Input.mousePosition);
        }
    }

    
    
}
