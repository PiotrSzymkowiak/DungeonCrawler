using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapple : MonoBehaviour
{
  public enum eMode { none, gOut, gInMiss, gInHit }

  [Header("Defined in inspection panel")]
  public float grappleSpd = 10;
  public float grappleLength = 7;
  public float grappleInLength = 0.5f;
  public int unsafeTileHealthPenalty = 2;
  public TextAsset mapGrappleable;

  [Header("Defined dynamically")]
  public eMode mode = eMode.none;
  public List<int> grappleTiles;
  public List<int> unsafeTiles;

  private Dray dray;
  private Rigidbody2D rigid;
  private Animator anim;
  private Collider2D drayColld;

  private GameObject grapHead;
  private LineRenderer grapLine;
  private Vector3 p0, p1;
  private int facing;

  private Vector3[] directions = new Vector3[]
  {
    Vector3.right,
    Vector3.up,
    Vector3.left,
    Vector3.down
  };

  void Awake()
  {
    string gTiles = mapGrappleable.text;
    gTiles = Utils.RemoveLineEndings(gTiles);
    grappleTiles = new List<int>();
    unsafeTiles = new List<int>();
    for (int i = 0; i < gTiles.Length; i++)
    {
      switch(gTiles[i])
      {
        case 'S':
          grappleTiles.Add(i);
          break;
        case 'X':
          unsafeTiles.Add(i);
          break;
      }
    }

    dray = GetComponent<Dray>();
    rigid = GetComponent<Rigidbody2D>();
    anim = GetComponent<Animator>();
    drayColld = GetComponent<Collider2D>();

    Transform trans = transform.Find("Grappler");
    grapHead = trans.gameObject;
    grapLine = grapHead.GetComponent<LineRenderer>();
    grapHead.SetActive(false);
  }

  void Update()
  {
    if (!dray.hasGrappler) return;
    
    switch(mode)
    {
      case eMode.none:
        if(Input.GetKeyDown(KeyCode.X))
        {
          StartGrapple();
        }
        break;
    }
  }

  void StartGrapple()
  {
    facing = dray.GetFacing();
    dray.enabled = false;
    anim.CrossFade("Dray_Attack_" + facing, 0);
    drayColld.enabled = false;
    rigid.velocity = Vector3.zero;

    grapHead.SetActive(true);
    p0 = transform.position + (directions[facing] * 0.5f);
    p1 = p0;
    grapHead.transform.position = p1;
    grapHead.transform.rotation = Quaternion.Euler(0, 0, 90 * facing);

    grapLine.positionCount = 2;
    grapLine.SetPosition(0, p0);
    grapLine.SetPosition(1, p1);
    mode = eMode.gOut;
  }

  void FixedUpdate()
  {
    //switch(mode)
    //{
    //  case eMode.gOut: //grapple goes out
    //    p1 += directions[facing] * grappleSpd * Time.fixedDeltaTime;
    //    grapHead.transform.position = p1;
    //    grapLine.SetPosition(1, p1);

    //    //check if hit
    //    int tileNum = TileCamera.GET_MAP(p1.x, p1.y);
    //    if(grappleTiles.IndexOf(tileNum) != -1)
    //    {
    //      mode = eMode.gInHit;
    //      break;
    //    }
    //    if((p1-p0).magnitude >= grappleLength)
    //    {
    //      mode = eMode.gInMiss;
    //    }
    //    break;

    //  case eMode.gInMiss: //miss, comes back with double speed
    //    p1 -= directions[facing] * 2 * grappleSpd * Time.fixedDeltaTime;
    //    if (Vector3.Dot((p1-p0), directions[facing]) > 0)
    //    {
    //      //grapple still before Dray
    //      grapHead.transform.position = p1;
    //      grapLine.SetPosition(1, p1);
    //    }
    //    else
    //    {
    //      StopGrapple();
    //    }
    //    break;

    //  case eMode.gInHit: //hit, pulls Dray to the wall
    //    float dist = grappleInLength + grappleSpd * Time.fixedDeltaTime;
    //    if(dist > (p1-p0).magnitude)
    //    {
    //      p0 = p1 - (directions[facing] * grappleInLength);
    //      transform.position = p0;
    //      StopGrapple();
    //      break;
    //    }
    //    p0 += directions[facing] * grappleSpd * Time.fixedDeltaTime;
    //    transform.position = p0;
    //    grapLine.SetPosition(0, p0);
    //    grapHead.transform.position = p1;
    //    break;
    //}
  }

  void StopGrapple()
  {
    //dray.enabled = true;
    //drayColld.enabled = true;

    ////check if tile is dangerous
    //int tileNum = TileCamera.GET_MAP(p0.x, p0.y);
    //if(mode == eMode.gInHit && unsafeTiles.IndexOf(tileNum) != -1)
    //{
    //  //we're on a dangerous tile
    //  dray.ResetInRoom(unsafeTileHealthPenalty);
    //}

    //grapHead.SetActive(false);

    //mode = eMode.none;
  }

  void OnTriggerEnter2D(Collider2D other)
  {
    Enemy e = drayColld.GetComponent<Enemy>();
    if (e == null) return;

    mode = eMode.gInMiss;
  }
}
