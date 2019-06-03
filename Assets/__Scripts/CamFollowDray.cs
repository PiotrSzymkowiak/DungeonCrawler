using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollowDray : MonoBehaviour
{
  public Dray dray;

  void Update()
  {
    Vector3 drayXYPosition = new Vector3(dray.transform.position.x, dray.transform.position.y, transform.position.z);
    transform.position = drayXYPosition;
  }
}
