using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
  public static int Width = 12;
  public static int Height = 24;
  public GameObject prefab;
  public DeleteBehaviour deleteBehaviour;
  public AudioClip soundDrop;
  public AudioClip soundDelete;
  public AudioSource audioSource;
  public event EventHandler GameOverEvent;
  public event EventHandler<DeletedEventArgs> DeletedEvent;
  private Cell[,] cells;
  private Block block;
  // Next next = new Next();
  bool insert;
  private int frame;
  internal int FastFrame { get; set; }
  internal int DropFrame { get; set; }
  internal void Init(int dropFrame)
  {
    DropFrame = dropFrame;
    FastFrame = DropFrame / 2;
    cells = new Cell[Width, Height + 3];
    BuildStage();
    deleteBehaviour.DeletedEvent += OnDeletedEvent;
    // next.Init(c.next);
    insert = false;
    frame = 0;
    Next();
  }
  private void BuildStage()
  {
    for (int y = 0; y < Height; y++)
    {
      for (int x = 0; x < Width; x++)
      {
        GameObject obj = Instantiate(prefab);
        var position = new Vector2(-2f + x * 0.355f, -3.790f + y * 0.355f);
        cells[x, y] = (x == 0 || x == Width - 1 || y == 0) ?
          new Cell(State.Wall, obj, position) : new Cell(State.Empty, obj, position);
      }
    }
  }
  internal void Reset()
  {
    insert = false;
    frame = 0;
    // next.Hide();
    // next.Reset();
    DeleteAll();

    Next();
    gameObject.SetActive(false);
  }
  void Update()
  {
    frame++;
    HandleInput();
    if (frame >= DropFrame)
    {
      Drop();
    }
  }
  internal void HandleInput()
  {
    Key.Handle();
    if (Key.PressingDown())
    {
      if (!insert) frame += FastFrame;
    }
    else
    {
      insert = false;
    }
    if (Key.Left())
    {
      Move(-1, 0);
    }
    else if (Key.Right())
    {
      Move(1, 0);
    }
    else if (Key.Rotate())
    {
      Rotate();
    }
  }
  private void Transcribe()
  {
    foreach (var point in block.Current)
    {
      var cell = cells[block.Position.X + point.X, block.Position.Y + point.Y];
      cell.State = (State)block.Type;
    }
  }
  private bool IsEmpty(Point position, Point[] shape)
  {
    bool isEmpty = true;
    foreach (var point in shape)
    {
      var cell = cells[position.X + point.X, position.Y + point.Y];
      isEmpty = isEmpty && (cell == null || cell.State == State.Empty);
    }
    return isEmpty;
  }
  private bool Move(int dx, int dy)
  {
    Point nextPosition = block.Position.Add(dx, dy);
    if (IsEmpty(nextPosition, block.Current))
    {
      block.Move(dx, dy);
      return true;
    }
    return false;
  }

  private bool Rotate()
  {
    if (IsEmpty(block.Position, block.Rotate(false)))
    {
      block.Rotate();
      return true;
    }
    return false;
  }
  internal void Next()
  {
    // TODO pull from Next stage
    GameObject[] objects = { Instantiate(prefab), Instantiate(prefab), Instantiate(prefab), Instantiate(prefab) };
    block = gameObject.AddComponent<Block>() as Block;
    block.Init(objects, new Point(Width / 2 - 1, Height - 2));
    if (!IsEmpty(block.Position, block.Current))
    {
      block.Destroy();
      gameObject.SetActive(false);
      GameOverEvent(this, null);
    }
  }
  private void Drop()
  {
    frame = 0;
    if (!Move(0, -1))
    {
      Transcribe();
      block.Destroy();
      if (deleteBehaviour.TryDeleting(cells))
      {
        gameObject.SetActive(false);
        audioSource.PlayOneShot(soundDelete);
        deleteBehaviour.StartDeleting();
      }
      else
      {
        audioSource.PlayOneShot(soundDrop);
      }
      Next();
    }
  }
  void OnDeletedEvent(object sender, DeletedEventArgs e)
  {
    gameObject.SetActive(true);
    DeletedEvent(this, e);
  }
  private void DeleteAll()
  {
    for (int y = 1; y < Height; y++)
    {
      for (int x = 1; x < Width - 1; x++)
      {
        cells[x, y].State = State.Empty;
      }
    }
  }
}
