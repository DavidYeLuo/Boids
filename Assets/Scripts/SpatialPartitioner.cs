using UnityEngine;
using Boids;

namespace BOptimizer {
public class SpatialPartitioner {
  public float SpaceUnit { get; private set; }
  public float ScreenWidth { get; private set; }
  public float ScreenHeight { get; private set; }
  public int SpaceMaxEntity {
      get; private set; } /// How much unit can one space occupy

  public int MaxRowIndex {
    get { return (int)(ScreenWidth / SpaceUnit); }
  }

  public int MaxColIndex {
    get { return (int)(ScreenHeight / SpaceUnit); }
  }

  private BComponent<int>[
    ,
  ] indexMap;

  /// division unit is the nxn box that represents the space
  /// space_max_unit is the max entity that can occupy a unit space
  public SpatialPartitioner(float spaceUnit, float screen_width,
                            float screen_height, int space_max_unit) {
    this.SpaceUnit = spaceUnit;
    this.ScreenWidth = screen_width;
    this.ScreenHeight = screen_height;
    this.SpaceMaxEntity = space_max_unit;

    /// We add 1 to row and col to include (0,0) aswell
    indexMap = MallocSpace<int>();
  }

  public BComponent<K>[
    ,
  ] MallocSpace<K>() {
    BComponent<K>[
      ,
    ] ret = new BComponent<K>[MaxRowIndex + 1, MaxColIndex + 1];
    for (int _row = 0; _row <= MaxRowIndex; _row++) {
      for (int _col = 0; _col <= MaxColIndex; _col++) {
        ret[_row, _col] = new BComponent<K>(new K[SpaceMaxEntity], 0);
      }
    }
    return ret;
  }

  public BComponent<int>[
    ,
  ] UpdateIndex(BComponent<Vector3> positions) {
    for (int i = 0; i < positions.Length; i++) {
      int row = (int)(positions.Data[i].x / SpaceUnit);
      int col = (int)(positions.Data[i].y / SpaceUnit);
      int _length = indexMap[row, col].Length;
      indexMap[row, col].Data[_length] = i;
      indexMap[row, col].Length++;
    }
    return indexMap;
  }

  public BComponent<int>[
    ,
  ] ResetIndex() {
    for (int row = 0; row <= MaxRowIndex; row++) {
      for (int col = 0; col <= MaxColIndex; col++) {
        indexMap[row, col].Length = 0;
      }
    }
    return indexMap;
  }

  /// Maps data to the partitioned data using computed indeces
  public BComponent<T>[
    ,
  ] Partition<T>(BComponent<T> data,
                 BComponent<T>[
                   ,
                 ] output) {
    for (int row = 0; row <= MaxRowIndex; row++) {
      for (int col = 0; col <= MaxColIndex; col++) {
        output[row, col].Length = indexMap[row, col].Length;
        for (int i = 0; i < output[row, col].Length; i++) {
          output[row, col].Data[i] = data.Data[indexMap[row, col].Data[i]];
        }
      }
    }
    return output;
  }

  /// Writes the partitioned data back to the original data
  public void Write<T>(
      BComponent<T>[
        ,
      ] writeFrom,
      BComponent<T> writeTo) {
    for (int row = 0; row <= MaxRowIndex; row++) {
      for (int col = 0; col <= MaxColIndex; col++) {
        for (int i = 0; i < writeFrom[row, col].Length; i++) {
          writeTo.Data[indexMap[row, col].Data[i]] =
              writeFrom[row, col].Data[i];
        }
      }
    }
  }

  public void Resize<T>(BComponent<T>[
    ,
  ] component) {
    for (int row = 0; row <= MaxRowIndex; row++) {
      for (int col = 0; col <= MaxColIndex; col++) {
        component[row, col].Length = indexMap[row, col].Length;
      }
    }
  }
}
}
