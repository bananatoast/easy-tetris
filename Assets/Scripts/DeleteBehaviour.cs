using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeletedEventArgs : EventArgs
{
  public DeletedEventArgs(int lines)
  {
    Lines = lines;
  }
  public int Lines { get; set; }
}
public class DeleteBehaviour : MonoBehaviour
{
  public event EventHandler<DeletedEventArgs> DeletedEvent;
  Board board;
  int frame;
  int deleteFrame = 30;
  List<int> lines = new List<int>();
  internal void Init(Board board)
  {
    this.board = board;
    frame = 0;
  }
  internal void Update()
  {
    frame++;
    if (frame >= deleteFrame)
    {
      Complete();
    }
    else
    {
      Deleting();
    }
  }
  void Deleting()
  {
    // foreach (int y in lines)
    // {
    //   for (int x = board.minX; x < board.maxX; x++)
    //   {
    //     board.cells[x, y].AddAlpha(-0.03f);
    //   }
    // }
  }
  void Complete()
  {
    int count = lines.Count;
    // for (int i = 0; i < count; i++)
    // {
    //   for (int y = lines[i] - i; y < board.maxY; y++)
    //   {
    //     for (int x = board.minX; x < board.maxX; x++)
    //     {
    //       board.cells[x, y].id = board.cells[x, y + 1].id;
    //     }
    //   }
    // }
    frame = 0;
    lines.Clear();
    gameObject.SetActive(false);

    DeletedEvent(this, new DeletedEventArgs(count));

    board.gameObject.SetActive(true);
    board.Next();
  }
  void Enable()
  {
    gameObject.SetActive(true);
    board.gameObject.SetActive(false);
  }
  internal bool Check()
  {
    // for (int y = board.minY; y < board.maxY; y++)
    // {
    //   for (int x = board.minX; x < board.maxX; x++)
    //   {
    //     if (board.cells[x, y].id == Block.empty) break;
    //     if (x == 10) lines.Add(y);
    //   }
    // }
    if (lines.Count == 0)
    {
      board.Next();
      return false;
    }
    else
    {
      Enable();
      return true;
    }
  }
}
