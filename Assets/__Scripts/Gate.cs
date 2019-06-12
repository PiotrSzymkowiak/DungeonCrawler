using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
  public Transform GateTo;

  private void OnTriggerEnter2D(Collider2D collision)
  {
    collision.transform.position = GateTo.position;
  }
}
