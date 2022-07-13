using System;
using System.Collections.Generic;
using UnityEngine;

namespace AwgenCore.Voxel
{
  /// <summary>
  /// A utility iterator class that determines the order in which chunks should
  /// be loaded around a given point in the world based off of common factors
  /// such as viewing direction, velocity, and chunk occlusion.
  /// </summary>
  public class ChunkLoadingAnchor
  {
    /// <summary>
    /// A small helper class that represents a chunk position and a chunk load
    /// priority value for sorting the chunk loading order.
    /// </summary>
    private class ChunkLoadIndex : IComparable<ChunkLoadIndex>
    {
      /// <summary>
      /// Gets the priority value of this chunk load index. The chunk position
      /// with the lowest priority is intended to be loaded first while chunks
      /// with the highest priority are loaded last.
      /// </summary>
      public float Priority { get; set; }


      /// <summary>
      /// Gets the distance value of this chunk from the world origin position.
      /// </summary>
      public float Distance { get; set; }


      /// <summary>
      /// Gets the relative chunk position of this chunk load index. This value
      /// assumes that the loading anchor is placed at the world coordinates of
      /// 0, 0, 0.
      /// </summary>
      public BlockPos Position { get; private set; }


      /// <summary>
      /// Creates a new ChunkLoadIndex instance.
      /// </summary>
      /// <param name="pos">The chunk position.</param>
      public ChunkLoadIndex(BlockPos pos)
      {
        Position = pos;
      }


      /// <inheritdoc/>
      public int CompareTo(ChunkLoadIndex other)
      {
        if (other == null) return 1;
        return Priority.CompareTo(other.Priority);
      }
    }


    private readonly List<ChunkLoadIndex> chunkLoadOrder = new List<ChunkLoadIndex>();
    private BlockPos center = new BlockPos(0, 0, 0);
    private int viewDistance;


    /// <summary>
    /// Gets or sets the block position of the chunk loading anchor. Changing
    /// the position of the anchor will reset the iterator.
    /// </summary>
    public BlockPos Position
    {
      get => this.center;
      set
      {
        this.center = value;
        RecalculateChunkLoadOrder();
      }
    }


    /// <summary>
    /// Gets or sets the view distance of this chunk loading anchor. The view
    /// distance is the number of chunks to be loaded, as a circe around the
    /// center position with the view distance being the radius of the circle.
    /// Updating this value with reset the iterator.
    /// </summary>
    public int ViewDistance
    {
      get => this.viewDistance;
      set
      {
        this.viewDistance = value;
        RecalculateViewDistance();
      }
    }


    /// <summary>
    /// Gets the current chunk index within the iterator.
    /// </summary>
    public int IteratorIndex { get; private set; }


    /// <summary>
    /// Creates a new ChunkLoadingAnchor centered at 0, 0, 0, with a view
    /// distance of 10 chunks.
    /// </summary>
    public ChunkLoadingAnchor()
    {
      ViewDistance = 10;
    }


    /// <summary>
    /// Clears all objects from the chunk load order list and regenerates them.
    /// This process may be slightly computationally expensive.
    /// </summary>
    private void RecalculateViewDistance()
    {
      this.chunkLoadOrder.Clear();

      for (var x = -this.viewDistance; x <= this.viewDistance; x++)
      {
        for (var y = -this.viewDistance; y <= this.viewDistance; y++)
        {
          for (var z = -this.viewDistance; z <= this.viewDistance; z++)
          {
            if (new Vector3(x, y, z).magnitude > this.viewDistance) continue;
            var pos = new BlockPos(x, y, z) * 16;
            var index = new ChunkLoadIndex(pos);
            this.chunkLoadOrder.Add(index);
          }
        }
      }

      RecalculateChunkLoadOrder();
    }


    /// <summary>
    /// Recalculates the priority value of all chunks within the chunk load
    /// order list and sorts them. This also resets the iterator index.
    /// </summary>
    private void RecalculateChunkLoadOrder()
    {
      foreach (var chunk in this.chunkLoadOrder)
      {
        var nearestBlock = BlockPos.Max(Position, chunk.Position);
        nearestBlock = BlockPos.Min(nearestBlock, chunk.Position + 15);

        chunk.Distance = Vector3.Distance(nearestBlock.AsVector3, Position.AsVector3);
        chunk.Priority = chunk.Distance;
        // TODO Account for viewing direction
        // TODO Account for velocity
        // TODO Account for chunk occlusion
      }

      ResetIterator();
      this.chunkLoadOrder.Sort();
    }


    /// <summary>
    /// Resets the iterator index back to 0.
    /// </summary>
    public void ResetIterator()
    {
      IteratorIndex = 0;
    }


    /// <summary>
    /// Gets the next chunk position within the internal chunk loading index
    /// iterator.
    /// </summary>
    /// <param name="chunkPos">The next chunk position, or default if the iterator is finished.</param>
    /// <returns>True if the iterator has remaining chunks to load, or false if the iterator was unable to return a new chunk position.</returns>
    public bool NextChunk(out BlockPos chunkPos)
    {
      chunkPos = default;
      if (IteratorIndex >= this.chunkLoadOrder.Count) return false;

      chunkPos = this.chunkLoadOrder[IteratorIndex].Position + Position;
      IteratorIndex++;
      return true;
    }
  }
}
