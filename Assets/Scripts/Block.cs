using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum BlockType : int
{
  I = 2,
  O = 3,
  S = 4,
  Z = 5,
  J = 6,
  L = 7,
  T = 8,
  COMPRESSOR = 9
}
struct Point
{
  internal Point(int x, int y)
  {
    X = x;
    Y = y;
  }

  internal int X { get; }
  internal int Y { get; }

  internal Point Add(int dx, int dy)
  {
    return new Point(X + dx, Y + dy);
  }
  internal Point Add(Point d)
  {
    return Add(d.X, d.Y);
  }
  public override string ToString()
  {
    return "(" + X + "," + Y + ")";
  }
}
class Block : MonoBehaviour
{
  private Color color;
  private BlockType type;
  private Point position;
  private Point[][] rotations;
  private int rotation;
  private List<Cell> cells = new List<Cell>();
  internal Color Color { get { return color; } }
  internal BlockType Type { get { return type; } }
  internal Point[] Current { get { return rotations[rotation]; } }
  internal Point Position { get { return position; } }
  void Update()
  {
    if (type == BlockType.COMPRESSOR)
    {
      cells.ForEach(c => c.State = (c.State >= State.T) ? State.I : c.State + 1);
    }
  }
  internal Point[] Rotate(bool destructive = true)
  {
    int r = (rotation == rotations.Length - 1) ? 0 : rotation + 1;
    if (destructive)
    {
      rotation = r;
      for (var i = 0; i < Current.Length; i++)
      {
        cells[i].Position = cells[0].Position + new Vector2(0.355f * Current[i].X, 0.355f * Current[i].Y);
      }
    }
    return rotations[r];
  }
  internal void Move(int dx, int dy)
  {
    foreach (var cell in cells)
    {
      cell.Position += new Vector2(0.355f * dx, 0.355f * dy);
    }
    position = new Point(position.X + dx, position.Y + dy);
  }
  private static Dictionary<BlockType, Point[][]> shapes = new Dictionary<BlockType, Point[][]>()
  {
    {
      BlockType.I,
      new Point[][] {
          new Point[] { new Point(0, 0), new Point(-1, 0), new Point(1, 0), new Point(2, 0) },
          new Point[] { new Point(0, 0), new Point(0, -1), new Point(0, 1), new Point(0, 2) }
      }
    },
    {
      BlockType.O,
      new Point[][] {
        new Point[] { new Point(0, 0), new Point(1, 0), new Point(0, 1), new Point(1, 1) }
      }
    },
    {
      BlockType.S,
      new Point[][] {
        new Point[] { new Point(0, 0), new Point(-1, 0), new Point(1, 1), new Point(0, 1) },
        new Point[] { new Point(0, 0), new Point(1, -1), new Point(1, 0), new Point(0, 1) }
      }
    },
    {
      BlockType.Z,
      new Point[][] {
        new Point[] { new Point(0, 0), new Point(1, 0), new Point(-1, 1), new Point(0, 1) },
        new Point[] { new Point(0, 0), new Point(0, -1), new Point(1, 0), new Point(1, 1) }
      }
    },
    {
      BlockType.J,
      new Point[][] {
        new Point[] { new Point(0, 0), new Point(-1, 0), new Point(1, 0), new Point(-1, 1) },
        new Point[] { new Point(0, 0), new Point(0, -1), new Point(0, 1), new Point(1, 1) },
        new Point[] { new Point(0, 0), new Point(1, -1), new Point(1, 0), new Point(-1, 0) },
        new Point[] { new Point(0, 0), new Point(-1, -1), new Point(0, -1), new Point(0, 1) }
      }
    },
    {
      BlockType.L,
      new Point[][] {
        new Point[] { new Point(0, 0), new Point(-1, 0), new Point(1, 0), new Point(1, 1) },
        new Point[] { new Point(0, 0), new Point(0, -1), new Point(1, -1), new Point(0, 1) },
        new Point[] { new Point(0, 0), new Point(-1, -1), new Point(-1, 0), new Point(1, 0) },
        new Point[] { new Point(0, 0), new Point(0, -1), new Point(-1, 1), new Point(0, 1) }
      }
    },
    {
      BlockType.T,
      new Point[][] {
        new Point[] { new Point(0, 0), new Point(-1, 0), new Point(1, 0), new Point(0, 1) },
        new Point[] { new Point(0, 0), new Point(0, -1), new Point(1, 0), new Point(0, 1) },
        new Point[] { new Point(0, 0), new Point(0, -1), new Point(-1, 0), new Point(1, 0) },
        new Point[] { new Point(0, 0), new Point(0, -1), new Point(-1, 0), new Point(0, 1) }
      }
    },
    {
      BlockType.COMPRESSOR,
      new Point[][] {
        new Point[] { new Point(0, 0), new Point(1, 0), new Point(0, 1), new Point(1, 1) }
      }
    }
  };
  internal void Init(GameObject[] objects, Point position, float compressorChance = 0.0f)
  {
    this.type = (Random.Range(0.0f, 1.0f) < compressorChance) ? BlockType.COMPRESSOR : (BlockType)Random.Range(2, 9);
    this.rotations = shapes[this.type];
    this.rotation = 0;
    this.color = Colors.Get(this.type);
    this.position = position;
    for (var i = 0; i < Current.Length; i++)
    {
      var vec = new Vector2(-2f + (position.X + Current[i].X) * 0.355f, -3.790f + (position.Y + Current[i].Y) * 0.355f);
      var cell = new Cell((State)type, objects[i], vec);
      cells.Add(cell);
    }
  }
  internal void Destroy()
  {
    cells.ForEach(cell => Destroy(cell.GameObject));
    Destroy(this);
  }
}