﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
  public static int Width = 12;
  public static int Height = 24;
  public GameObject prefab;
  public DeleteBehaviour deleter;
  public CompressorBehaviour compressor;
  public AudioClip soundDrop;
  public AudioSource audioSource;
  public event EventHandler GameOverEvent;
  public event EventHandler<DeletedEventArgs> DeletedEvent;
  private static Point NextLanePosition = new Point(Width + 2, Height - 7);
  private Cell[,] cells;
  private Block block;
  private List<Block> nextQueue;
  private int frame;
  internal int FastFrame { get; set; }
  internal int DropFrame { get; set; }
  internal void Init(int dropFrame)
  {
    DropFrame = dropFrame;
    FastFrame = DropFrame / 2;
    cells = new Cell[Width, Height + 3];
    BuildStage();
    deleter.DeletedEvent += OnDeletedEvent;
    compressor.CompressedEvent += OnCompressedEvent;
    frame = 0;
    nextQueue = new List<Block>();
    for (int i = 0; i < 3; i++) EnqueueBlock();
    Next();
  }
  private void EnqueueBlock()
  {
    var order = nextQueue.Count;
    GameObject[] objects = { Instantiate(prefab), Instantiate(prefab), Instantiate(prefab), Instantiate(prefab) };
    var newBlock = gameObject.AddComponent<Block>() as Block;
    newBlock.Init(objects, NextLanePosition.Add(0, -3 * order));
    nextQueue.Add(newBlock);
  }
  private Block DequeueBlock()
  {
    var next = nextQueue[0];
    nextQueue.RemoveAt(0);
    nextQueue.ForEach(b => b.Move(0, 3));
    EnqueueBlock();
    return next;
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
    frame = 0;
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
      frame += FastFrame;
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
    block = DequeueBlock();
    block.Move(Width / 2 - 1 - NextLanePosition.X, Height - 2 - NextLanePosition.Y);
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
      if (block.Type == BlockType.COMPRESSOR)
      {
        compressor.StartCompressing(block, cells);
      }
      else
      {
        Transcribe();
        block.Destroy();
        if (deleter.TryDeleting(cells))
        {
          gameObject.SetActive(false);
          deleter.StartDeleting();
        }
        else
        {
          audioSource.PlayOneShot(soundDrop);
          Next();
        }
      }
    }
  }
  void OnDeletedEvent(object sender, DeletedEventArgs e)
  {
    gameObject.SetActive(true);
    Next();
    DeletedEvent(this, e);
  }
  void OnCompressedEvent(object sender, EventArgs e)
  {
    block.Destroy();
    gameObject.SetActive(true);
    if (deleter.TryDeleting(cells))
    {
      gameObject.SetActive(false);
      deleter.StartDeleting();
    }
    else
    {
      Next();
    }
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
