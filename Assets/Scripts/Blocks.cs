using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point
{
  internal int x, y;
  internal Point(int x, int y)
  {
    this.x = x;
    this.y = y;
  }
}
public class Blocks : MonoBehaviour
{
  static Point XY(int x, int y) { return new Point(x, y); }
  static Point[] XYs(Point p1, Point p2, Point p3)
  {
    return new Point[] { p1, p2, p3 };
  }
  //-> relative points (for rotation)
  static readonly Point[][] emptyR = null, wallR = null;
  static readonly Point[]
    iR0 = XYs(XY(-1, 0), XY(1, 0), XY(2, 0)),
    iR1 = XYs(XY(0, -1), XY(0, 1), XY(0, 2));
  static readonly Point[][] iR = { iR0, iR1, iR0, iR1 };
  static readonly Point[]
    oR0 = XYs(XY(1, 0), XY(0, 1), XY(1, 1));
  static readonly Point[][] oR = { oR0, oR0, oR0, oR0 };
  static readonly Point[]
    sR0 = XYs(XY(-1, 0), XY(1, 1), XY(0, 1)),
    sR1 = XYs(XY(1, -1), XY(1, 0), XY(0, 1));
  static readonly Point[][] sR = { sR0, sR1, sR0, sR1 };
  static readonly Point[]
    zR0 = XYs(XY(1, 0), XY(-1, 1), XY(0, 1)),
    zR1 = XYs(XY(0, -1), XY(1, 0), XY(1, 1));
  static readonly Point[][] zR = { zR0, zR1, zR0, zR1 };
  static readonly Point[][] jR = new Point[][] {
    XYs(XY(-1, 0), XY(1, 0), XY(-1, 1)),
    XYs(XY(0, -1), XY(0, 1), XY(1, 1)),
    XYs(XY(1, -1), XY(1, 0), XY(-1, 0)),
    XYs(XY(-1, -1), XY(0, -1), XY(0, 1))
  };
  static readonly Point[][] lR = new Point[][] {
    XYs(XY(-1, 0), XY(1, 0), XY(1, 1)),
    XYs(XY(0, -1), XY(1, -1), XY(0, 1)),
    XYs(XY(-1, -1), XY(-1, 0), XY(1, 0)),
    XYs(XY(0, -1), XY(-1, 1), XY(0, 1))
  };
  static readonly Point[][] tR = new Point[][] {
    XYs(XY(-1, 0), XY(1, 0), XY(0, 1)),
    XYs(XY(0, -1), XY(1, 0), XY(0, 1)),
    XYs(XY(0, -1), XY(-1, 0), XY(1, 0)),
    XYs(XY(0, -1), XY(-1, 0), XY(0, 1))
  };
  //-> id
  internal static readonly int
    empty = 0, wall = 1,
    i = 2, o = 3, s = 4, z = 5, j = 6, l = 7, t = 8;
  internal static readonly int[] drops = { i, o, s, z, j, l, t };
  //-> color (same order as id)
  internal static Color[] Colors(
    Color empty, Color wall,
    Color i, Color o, Color s, Color z, Color j, Color l, Color t
  )
  {
    return new Color[] { empty, wall, i, o, s, z, j, l, t };
  }
  //-> rotation (same order as id)
  static readonly Point[][][] rotations = new Point[][][] {
    emptyR, wallR, iR, oR, sR, zR, jR, lR, tR
  };
  internal static Point[] Relatives(Status s)
  {
    Point[][] r = rotations[s.id];
    return r[s.rotate];
  }
}
