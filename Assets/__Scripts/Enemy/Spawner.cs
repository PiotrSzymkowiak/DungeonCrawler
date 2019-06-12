using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
  public GameObject spawnPrefab;

  float startSpawning = 0;

  private void Start()
  {
    startSpawning = Time.time;
  }

  private void Update()
  {
    if(Time.time > startSpawning + 1f )
    {
      GameObject monster = Instantiate(spawnPrefab, transform.position, transform.rotation);
      startSpawning = Time.time;
    }
  }
}
