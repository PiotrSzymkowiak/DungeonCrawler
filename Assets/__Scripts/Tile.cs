using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
  [Header("Defined dynamically")]
  public int x;
  public int y;
  public int tileNum;

  private BoxCollider bColl;

  void Awake()
  {
    bColl = GetComponent<BoxCollider>(); 
  }

  public void SetTile(int eX, int eY, int eTileNum = -1)
  {
    x = eX;
    y = eY;
    transform.localPosition = new Vector3(x, y, 0);
    gameObject.name = x.ToString("D3") + "x" + y.ToString("D3");

    if(eTileNum == -1)
    {
      eTileNum = TileCamera.GET_MAP(x, y);
    }
    else
    {
      TileCamera.SET_MAP(x, y, eTileNum);
    }
    tileNum = eTileNum;
    GetComponent<SpriteRenderer>().sprite = TileCamera.SPRITES[tileNum];

    SetCollider();
  }

  void SetCollider()
  {
    bColl.enabled = true;
    char c = TileCamera.COLLISIONS[tileNum];
    switch(c)
    {
      case 'S': //whole tile
        bColl.center = Vector3.zero;
        bColl.size = Vector3.one;
        break;
      case 'W': //upper part
        bColl.center = new Vector3(0, 0.25f, 0);
        bColl.size = new Vector3(1, 0.5f, 1);
        break;
      case 'A'://left part
        bColl.center = new Vector3(-0.25f, 0, 0);
        bColl.size = new Vector3(0.5f, 1, 1);
        break;
      case 'D'://right part
        bColl.center = new Vector3(0.25f, 0, 0);
        bColl.size = new Vector3(0.5f, 1, 1);
        break;
      //---------------- optional (not used in this game) ----------------------------
      case 'Q'://upper left part
        bColl.center = new Vector3(-0.25f, 0.25f, 0);
        bColl.size = new Vector3(0.5f, 0.5f, 1);
        break;
      case 'E'://upper right part
        bColl.center = new Vector3(0.25f, 0.25f, 0);
        bColl.size = new Vector3(0.5f, 0.5f, 1);
        break;
      case 'Z'://bottom left part
        bColl.center = new Vector3(-0.25f, -0.25f, 0);
        bColl.size = new Vector3(0.5f, 0.5f, 1);
        break;
      case 'X'://bottom part
        bColl.center = new Vector3(0, -0.25f, 0);
        bColl.size = new Vector3(1, 0.5f, 1);
        break;
      case 'C'://bottom right part
        bColl.center = new Vector3(0.25f, -0.25f, 0);
        bColl.size = new Vector3(0.5f, 0.5f, 1);
        break;

      default:
        bColl.enabled = false;
        break;
    }
  }
}
