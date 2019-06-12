using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
  public GameObject[] spawnPrefabs;
  public Transform parent;


  public float spawnRateTime = 30f;
  float startSpawning = 0;

  private void Start()
  {
    startSpawning = Time.time;
  }

  private void Update()
  {
    if (Time.time > startSpawning + spawnRateTime)
    {

      int index = Random.Range(0, spawnPrefabs.Length);
      GameObject monster = Instantiate(spawnPrefabs[index], transform.position, transform.rotation, parent);
      startSpawning = Time.time;
    }
  }
}
