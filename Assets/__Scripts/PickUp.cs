﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
  public enum eType {  key, health, grappler }

  public static float COLLIDER_DELAY = 0.5f;

  [Header("Defined in inspection panel")]
  public eType itemType;

  void Awake()
  {
    GetComponent<Collider2D>().enabled = false;
    Invoke("Activate", COLLIDER_DELAY);
  }

  void Activate()
  {
    GetComponent<Collider2D>().enabled = true;
  }
}