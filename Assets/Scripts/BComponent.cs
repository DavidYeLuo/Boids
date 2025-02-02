namespace Boids {
public struct BComponent<T> {
  public T[] Data;
  public int Length;
  public int Capacity;
  public BComponent(T[] data, int length) {
    this.Data = data;
    this.Length = length;
    this.Capacity = data.Length;
  }
}
}
