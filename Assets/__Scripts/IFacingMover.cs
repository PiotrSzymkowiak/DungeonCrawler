using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFacingMover
{
  int GetFacing();
  bool moving { get; }
  float GetSpeed();
}
