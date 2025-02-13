using UnityEngine;
using Boids;

namespace BOptimizer {
public class SpatialPartitioner<T> {
  public float SpaceUnit { get; private set; }
  public float ScreenWidth { get; private set; }
  public float ScreenHeight { get; private set; }
  public int SpaceMaxEntity {
    get; private set;
  } /// How much unit can one space occupy

  public int MaxRowIndex {
    get { return (int)(ScreenWidth / SpaceUnit); }
  }

  public int MaxColIndex {
    get { return (int)(ScreenHeight / SpaceUnit); }
  }

  private BComponent<T>[
    ,
  ] space;
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
    indexMap = new BComponent<int>[MaxRowIndex + 1, MaxColIndex + 1];
    space = new BComponent<T>[MaxRowIndex + 1, MaxColIndex + 1];
    for (int _row = 0; _row <= MaxRowIndex; _row++) {
      for (int _col = 0; _col <= MaxColIndex; _col++) {
        space[_row, _col] = new BComponent<T>(new T[SpaceMaxEntity], 0);
        indexMap[_row, _col] = new BComponent<int>(new int[SpaceMaxEntity], 0);
      }
    }
  }

  // Returns the up to date list
  private BComponent<K> Add<K>(BComponent<K> component, K element) {
    component.Data[component.Length] = element;
    component.Length++;
    return component;
  }
  public BComponent<T>[
    ,
  ] Partition(BComponent<Vector3> positions, BComponent<T> data) {
    for (int i = 0; i < positions.Length; i++) {
      int row = (int)(positions.Data[i].x / SpaceUnit);
      int col = (int)(positions.Data[i].y / SpaceUnit);
      indexMap[row, col] = Add<int>(indexMap[row, col], i);
      space[row, col] = Add<T>(space[row, col], data.Data[i]);
    }
    return this.space;
  }

  public void WriteTo(BComponent<T> buffer) {
    for (int row = 0; row <= MaxRowIndex; row++) {
      for (int col = 0; col <= MaxColIndex; col++) {
        for (int i = 0; i < space[row, col].Length; i++) {
          buffer.Data[indexMap[row, col].Data[i]] = space[row, col].Data[i];
        }
      }
    }
  }
}
}
