using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCamera : MonoBehaviour
{
  static private int W, H;
  static private int[,] MAP;
  static public Sprite[] SPRITES;
  static public Transform TILE_ANCHOR;
  static public Tile[,] TILES;
  static public string COLLISIONS;

  [Header("Defined in inspection panel")]
  public TextAsset mapData;
  public Texture2D mapTiles;
  public TextAsset mapCollisions;
  public Tile tilePrefab;

  void Awake()
  {
    COLLISIONS = Utils.RemoveLineEndings(mapCollisions.text);
    LoadMap();   
  }

  public void LoadMap()
  {
    //TILE_ANCHOR - parent of all tiles
    GameObject go = new GameObject("TILE_ANCHOR");
    TILE_ANCHOR = go.transform;
    //Sprite loading
    SPRITES = Resources.LoadAll<Sprite>(mapTiles.name);

    //Load map data
    string[] lines = mapData.text.Split('\n');
    H = lines.Length;
    string[] tileNums = lines[0].Split(' ');
    W = tileNums.Length;

    System.Globalization.NumberStyles hexNum;
    hexNum = System.Globalization.NumberStyles.HexNumber;
    //Set map data to 2D array because of faster access
    MAP = new int[W, H];
    for (int j = 0; j < H; j++)
    {
      tileNums = lines[j].Split(' ');
      for(int i = 0; i < W; i++)
      {
        if(tileNums[i] == "..")
        {
          MAP[i, j] = 0;
        }
        else
        {
          MAP[i, j] = int.Parse(tileNums[i], hexNum);
        }
      }
    }

    print("Processed " + SPRITES.Length + " sprites");
    print("Map size: " + W + " - width, " + H + " - height");

    ShowMap();
  }

  private void ShowMap()
  {
    TILES = new Tile[W, H];

    for (int j = 0; j < H; j++)
    {
      for (int i = 0; i < W; i++)
      {
        if(MAP[i,j] != 0)
        {
          Tile ti = Instantiate<Tile>(tilePrefab);
          ti.transform.SetParent(TILE_ANCHOR);
          ti.SetTile(i, j);
          TILES[i, j] = ti;
        }
      }
    }
  }

  static public int GET_MAP(int x, int y)
  {
    if(AreCoordsOutOfBounds(x, y))
    {
      return -1;
    }
    return MAP[x, y];
  }

  static public int GET_MAP(float x, float y)
  {
    int tX = Mathf.RoundToInt(x);
    int tY = Mathf.RoundToInt(y - 0.25f);
    return GET_MAP(tX, tY);
  }

  static public void SET_MAP(int x, int y, int tNum)
  {
    if (AreCoordsOutOfBounds(x,y))
    {
      return;
    }

    MAP[x, y] = tNum;
  }

  static private bool AreCoordsOutOfBounds(int x, int y)
  {
    return (x < 0 || x >= W || y < 0 || y >= H);
  }
}
