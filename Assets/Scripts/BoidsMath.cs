using UnityEngine;
using UnityEngine.Assertions;

namespace Boids {
public static class BoidsMath {
  public static BComponent<Vector3>
  GetAvoidanceVector(BComponent<Vector3> objs, BComponent<Vector3> output) {
    float dist = 0.0f;
    // Calculates the sum around its neighbor except itself
    for (int i = 0; i < objs.Length; i++) {
      output.Data[i] = Vector3.zero;
      for (int l = 0; l < i; l++) {
        dist = Vector3.Distance(objs.Data[i], objs.Data[l]);
        output.Data[i] += (objs.Data[i] - objs.Data[l]) / Mathf.Sqrt(dist);
      }
      for (int r = i + 1; r < objs.Length; r++) {
        dist = Vector3.Distance(objs.Data[i], objs.Data[r]);
        output.Data[i] += (objs.Data[i] - objs.Data[r]) / Mathf.Sqrt(dist);
      }
    }
    return output;
  }

  public static BComponent<Vector3>
  GetCoheranceVector(BComponent<Vector3> positions,
                     BComponent<Vector3> output) {
    Vector3 averagePosition = Vector3.zero;
    for (int i = 0; i < positions.Length; i++) {
      averagePosition += positions.Data[i];
    }
    averagePosition = averagePosition / positions.Length;
    for (int i = 0; i < positions.Length; i++) {
      output.Data[i] = averagePosition - positions.Data[i];
    }
    return output;
  }

  public static BComponent<Vector3>
  GetAlignmentVector(BComponent<Vector3> objs, BComponent<Vector3> output) {
    for (int i = 0; i < objs.Length; i++) {
      Vector3 sum = Vector3.zero;
      for (int j = 0; j < objs.Length; j++) {
        sum += objs.Data[j];
      }
      output.Data[i] = sum / objs.Length;
    }
    return output;
  }

  public static BComponent<Vector3> Sum(BComponent<Vector3> left,
                                        BComponent<Vector3> right,
                                        BComponent<Vector3> output) {
    Assert.AreEqual(left.Length, right.Length);
    int count = left.Length;

    for (int i = 0; i < left.Length; i++) {
      output.Data[i] = left.Data[i] + right.Data[i];
    }
    return output;
  }
  public static BComponent<Vector3> Sub(BComponent<Vector3> left,
                                        BComponent<Vector3> right,
                                        BComponent<Vector3> output) {
    Assert.AreEqual(left.Length, right.Length);
    int count = left.Length;

    for (int i = 0; i < left.Length; i++) {
      output.Data[i] = left.Data[i] - right.Data[i];
    }
    return output;
  }

  public static BComponent<Vector3> Scale(BComponent<float> scalar,
                                          BComponent<Vector3> vector,
                                          BComponent<Vector3> output) {
    for (int i = 0; i < scalar.Length; i++) {
      output.Data[i] = scalar.Data[i] * vector.Data[i];
    }
    return output;
  }
  public static BComponent<Vector3>
  Scale(float scalar, BComponent<Vector3> vector, BComponent<Vector3> output) {
    for (int i = 0; i < vector.Length; i++) {
      output.Data[i] = scalar * vector.Data[i];
    }
    return output;
  }
  public static BComponent<Vector3> Normalize(BComponent<Vector3> vector,
                                              BComponent<Vector3> output) {
    for (int i = 0; i < vector.Length; i++) {
      output.Data[i] = vector.Data[i].normalized;
    }
    return output;
  }
  public static BComponent<Vector3> WrapBetween(float maxX, float maxY,
                                                BComponent<Vector3> vector,
                                                BComponent<Vector3> output) {
    for (int i = 0; i < vector.Length; i++) {
      output.Data[i].x = Mathf.Repeat(vector.Data[i].x, maxX);
      output.Data[i].y = Mathf.Repeat(vector.Data[i].y, maxY);
    }
    return output;
  }
}
}
