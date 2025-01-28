using UnityEngine;
using UnityEngine.Assertions;

namespace Boids {
public static class BoidsMath {
  public static Vector3[] GetAvoidanceVector(Vector3[] objs) {
    Vector3[] sum = new Vector3[objs.Length];
    float dist = 0.0f;
    // Calculates the sum around its neighbor except itself
    for (int i = 0; i < objs.Length; i++) {
      sum[i] = Vector3.zero;
      for (int l = 0; l < i; l++) {
        dist = Vector3.Distance(objs[i], objs[l]);
        sum[i] += (objs[i] - objs[l]) / Mathf.Sqrt(dist);
      }
      for (int r = i + 1; r < objs.Length; r++) {
        dist = Vector3.Distance(objs[i], objs[r]);
        sum[i] += (objs[i] - objs[r]) / Mathf.Sqrt(dist);
      }
    }
    return sum;
  }

  public static Vector3[] GetCoheranceVector(Vector3[] objs) {
    Vector3[] res = new Vector3[objs.Length];
    for (int i = 0; i < objs.Length; i++) {
      Vector3 sum = Vector3.zero;
      for (int j = 0; j < objs.Length; j++) {
        sum += objs[j];
      }
      res[i] = sum / objs.Length;
    }
    return res;
  }

  public static Vector3[] GetAlignmentVector(Vector3[] objs) {
    Vector3[] res = new Vector3[objs.Length];
    for (int i = 0; i < objs.Length; i++) {
      Vector3 sum = Vector3.zero;
      for (int j = 0; j < objs.Length; j++) {
        sum += objs[j];
      }
      res[i] = sum / objs.Length;
    }
    return res;
  }

  public static Vector3[] Sum(Vector3[] left, Vector3[] right) {
    Assert.AreEqual(left.Length, right.Length);
    int count = left.Length;
    Vector3[] res = new Vector3[count];

    for (int i = 0; i < left.Length; i++) {
      res[i] = left[i] + right[i];
    }
    return res;
  }
}
}
