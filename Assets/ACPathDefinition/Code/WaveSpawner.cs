/*
 * Date: October 20, 2015.
 * Author: Guillermo Bauza.
 * E-mail: guilei4910@yahoo.com
 * Description: Spawn an Object for x amount in order to create a wave. Multiples waves can be created. 
*/
/// <summary>
/// First of all if you have any problem with the default script contact me so we can obtain an answer and fix together the problem.
/// You are completely free to modify the script, Warning: I don't get responsible for future errors if the default script get changed or edited. 
/// </summary>

using UnityEngine;
using System.Collections;

public class WaveSpawner : MonoBehaviour
{
    #region INSIDE CLASSES
    //Wave Spawner State on Runtime
    public enum SpawnState { SPAWNING, WAITING, COUNTING }

    [System.Serializable]
    public class Wave
    {
        public string name;//name of the wave that is currently spawning.
        public Transform SpawnObject; // what Object are we gonna spawn.
        public int count; // How many object are we gonna spawn
        public float rate; // at what rate we gonna spawn all the object per wave.
    }
    #endregion

    #region FIELDS
    public Wave[] waves; //public class instance
    public float timeBetweenWaves = 3f; //How much time will take to spawn next wave.
    public bool SpawnOnceDestroyed = true; // Spawn your objects at the set timer or once the entire current wave is destroyed.
    public bool LoopWaves = true;
    public string ObjectTagName = "Enemy"; //Tag name of the object we are spawning.

    private int nextWave = 0; //counter
    private float waveCountDown; // time counter left.
    private float searchCountDown = 1f; // at what rate will check if theres active object of the current wave in game.
    private SpawnState state = SpawnState.COUNTING; // set default state.
    #endregion

    void Start() { waveCountDown = timeBetweenWaves; }

    void Update()
    {
        if (nextWave != -1)
        {
            if (state == SpawnState.WAITING)
            { 
                if (SpawnOnceDestroyed)
                {
                    //Check if enemies are still alive
                    if (!EnemyIsAlive())
                        nextRound();//Begin a new Round
                    else
                        return;
                }
                else nextRound();

            }
            if (waveCountDown <= 0)
            {
                if (state != SpawnState.SPAWNING)
                    StartCoroutine(SpawnWave(waves[nextWave])); //Start spawning
            }
            else
                waveCountDown -= Time.deltaTime;
        }
    }

    #region PRIVATE FUNCTIONS
    private bool EnemyIsAlive()
    {
        searchCountDown -= Time.deltaTime;
        if (searchCountDown <= 0f)
        {
            searchCountDown = 1f;
            if (GameObject.FindGameObjectWithTag(ObjectTagName) == null)
                return false;
        }
        return true;
    }//Enemy is alive method

    /// <summary>
    /// Loop over all waves or load next Scene once all waves are spawned.
    /// </summary>

    private void nextRound()
    {
        state = SpawnState.COUNTING;
        waveCountDown = timeBetweenWaves;

        if (nextWave + 1 > waves.Length - 1)
        {
            if (LoopWaves)
                nextWave = 0;
            else
                nextWave = -1;

            ///---------------OR--------------------
            // You can use: Application.LoadLevel("Next_Scene");
            // instead of nextWave = 0;           
        }
        else
            nextWave++;
    }//Advance over waves or scenes method

    IEnumerator SpawnWave(Wave _wave)
    {
        //Spawn
        state = SpawnState.SPAWNING;
        for (int i = 0; i < _wave.count; i++)
        {
            SpawnEnemy(_wave.SpawnObject);
            yield return new WaitForSeconds(1f / _wave.rate);
        }
        //Wait
        state = SpawnState.WAITING;
        yield break;
    }// SpawnWave enumerator

    /// <summary>
    /// Spawn the objects at spawner position or modify this method to spawn at given points randomly.
    /// </summary>
    /// <param name="_enemy"></param>
    private void SpawnEnemy(Transform _enemy)
    {
        //spawn enemy
        Instantiate(_enemy, transform.position, Quaternion.identity);
    }//Spawn EnemyMethod
    #endregion

}//class
