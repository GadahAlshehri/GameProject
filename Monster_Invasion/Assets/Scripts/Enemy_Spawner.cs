using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy_Spawner : MonoBehaviour
{

    [SerializeField]
    private float  SpawnRadius , time;
    public GameObject[] enemies;
    List<GameObject> currentEnemies;
    bool waveIsDone = true;
    public Text waveCountText;
    public int waveCount = 1;
    public int EnemyNum;
   public GameObject WaveCanvas;
    int EnemyCounterWave;
    public List<GameObject> GetCurrentEnemies()
    {
        return currentEnemies;
    }

    void Update()
    {
        waveCountText.text = "Wave: " + waveCount.ToString();
       
        if (waveIsDone == true)
        {
            StartCoroutine(SpawnAnEnemy());
        }
    }
    void Start()
    {
        WaveCanvas.SetActive(false);

            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            currentEnemies = new List<GameObject>();
            for (int i = 0; i < enemies.Length; i++)
            {
                currentEnemies.Add(enemies[i]);
            }

        if (gameObject != null)
        StartCoroutine(SpawnAnEnemy());
        }
    

    public void AddEnemy(GameObject newEnemy)
    {
        currentEnemies.Add(newEnemy);
    }


    IEnumerator SpawnAnEnemy()
    {
        waveIsDone = false;
   if (EnemyCounterWave == 0)
            {
                WaveCanvas.SetActive(true);

            }
        yield return new WaitForSeconds(4);
        WaveCanvas.SetActive(false);

        for (int i = 0; i <= EnemyNum; i++)
        {
            EnemyCounterWave++;


            Vector2 spawnPos = GameObject.FindGameObjectWithTag("Player").transform.position;

            spawnPos += Random.insideUnitCircle.normalized * SpawnRadius;
            GameObject enemy = null;
            do { enemy = Instantiate(enemies[Random.Range(0, enemies.Length)], spawnPos, Quaternion.identity); if (enemy.GetComponent<Enemy>().startAtWave > waveCount) { Destroy(enemy); } } while (enemy.GetComponent<Enemy>().startAtWave > waveCount);
            AddEnemy(enemy);
            FindObjectOfType<Companion>().UpdateCurrentEnemies();
            EnemyCounterWave--;
            yield return new WaitForSeconds(time);

        //    StartCoroutine(SpawnAnEnemy());


        }


 EnemyNum++;
        waveCount++;

       
        SoundManger.WaveSound();

        GameObject.FindObjectOfType<PlayerController>().RestorHealth();
        GameObject.FindObjectOfType<Companion>().RestorHealth();
        yield return new WaitForSeconds(6);
       
   
        StartCoroutine(SpawnAnEnemy());
        


    }
}