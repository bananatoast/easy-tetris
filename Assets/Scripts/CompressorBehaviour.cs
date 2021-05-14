using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompressorBehaviour : MonoBehaviour
{
  public AudioClip soundDrop;
  public AudioSource audioSource;
  public event EventHandler<EventArgs> CompressedEvent;
  static int NO_EMPTY = 0;
  private static int FrameLimit = 6;
  private int frame;
  private Block block;
  private Cell[,] cells;
  void Update()
  {
    frame++;
    if (frame >= FrameLimit)
    {
      List<Point> compressable = FindCompressable(block.Position);
      if (compressable.Count == 0)
      {
        CompressedEvent(this, null);
        gameObject.SetActive(false);
      }
      else
      {
        foreach (var p in compressable)
        {
          for (int y = p.Y; y < block.Position.Y + 1; y++)
          {
            cells[p.X, y].State = cells[p.X, y + 1].State;
          }
        }
        block.Move(0, -1);
      }
      audioSource.PlayOneShot(soundDrop);
      frame = 0;
    }
  }
  private List<Point> FindCompressable(Point basePosition)
  {
    var compressable = new List<Point>();
    var left = FindCompressable(basePosition.X, basePosition.Y);
    var right = FindCompressable(basePosition.X + 1, basePosition.Y);
    if (left != NO_EMPTY && right != NO_EMPTY)
    {
      compressable.Add(new Point(basePosition.X, left));
      compressable.Add(new Point(basePosition.X + 1, right));
    }
    else if (left != NO_EMPTY || right != NO_EMPTY)
    {
      int y = Math.Max(left, right);
      compressable.Add(new Point(basePosition.X, y));
      compressable.Add(new Point(basePosition.X + 1, y));
    }
    return compressable;
  }
  private int FindCompressable(int x, int baseY)
  {
    for (int y = baseY - 1; y > 0; y--)
    {
      if (cells[x, y].State == State.Empty)
      {
        return y;
      }
    }
    return NO_EMPTY;
  }
  internal void StartCompressing(Block block, Cell[,] cells)
  {
    this.frame = 0;
    this.block = block;
    this.cells = cells;
    gameObject.SetActive(true);
  }
}
