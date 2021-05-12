using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoading : MonoBehaviour
{
    public string gunScene;
    // Start is called before the first frame update
    void Start()
    {
        if (!SceneManager.GetSceneByName("Gun").isLoaded)
        {
            SceneManager.LoadSceneAsync(gunScene, LoadSceneMode.Additive);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
