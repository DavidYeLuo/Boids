using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Boids {
/// 2D Boids Manager
public class BoidsManager {
  public static BComponent<Vector3> GetZeroVectors(BComponent<Vector3> output) {
    for (int i = 0; i < output.Length; i++) {
      output.Data[i] = Vector3.zero;
    }
    return output;
  }
  /// We're using this as 2D. Meaning the z-axis is 0.0f
  /// StartRange and EndRange are inclusive numbers for rng
  /// NxN box
  public static BComponent<Vector3>
  GetRandVectors(float startRange, float endRange, BComponent<Vector3> output) {
    for (int i = 0; i < output.Length; i++) {
      float rx = Random.Range(startRange, endRange);
      float ry = Random.Range(startRange, endRange);
      output.Data[i] = new Vector3(rx, ry, 0.0f);
    }
    return output;
  }
  /// We're using this as 2D. Meaning the z-axis is 0.0f
  /// StartRange and EndRange are inclusive numbers for rng
  /// NxN box
  public static BComponent<Vector3> GetRandVectors(float startX, float endX,
                                                   float startY, float endY,
                                                   BComponent<Vector3> output) {
    for (int i = 0; i < output.Length; i++) {
      float rx = Random.Range(startX, endX);
      float ry = Random.Range(startY, endY);
      output.Data[i] = new Vector3(rx, ry, 0.0f);
    }
    return output;
  }

  public static BComponent<Quaternion>
  GetIdentityQuaternions(int numOfQuaternions, BComponent<Quaternion> output) {
    for (int i = 0; i < numOfQuaternions; i++) {
      output.Data[i] = Quaternion.identity;
    }
    return output;
  }
  /// We're using this as 2D. Meaning we're only rotating around the z-axis
  public static BComponent<Quaternion>
  GetRandQuaternions(int numOfQuaternions, BComponent<Quaternion> output) {
    const float MIN_ANGLE = 0.001f;
    const float MAX_ANGLE = 359.999f;
    for (int i = 0; i < numOfQuaternions; i++) {
      float rAngle = Random.Range(MIN_ANGLE, MAX_ANGLE);
      output.Data[i] = Quaternion.Euler(0.0f, 0.0f, rAngle);
    }
    return output;
  }
  /// We're using this as 2D
  public static BComponent<float>
  GenerateNewAngle(BComponent<Vector3> directions,
                   BComponent<Vector3> newDirections,
                   BComponent<float> output) {
    Assert.AreEqual(directions.Length, newDirections.Length,
                    "Direction and newDirections have different sizes.");
    Assert.AreEqual(directions.Length, output.Length,
                    "Direction and output have different sizes.");
    int length = directions.Length;
    for (int i = 0; i < length; i++) {
      float dot = Vector3.Dot(directions.Data[i], newDirections.Data[i]);
      Assert.AreEqual(true, !float.IsNaN(dot), "dot is NaN");
      output.Data[i] =
          dot / newDirections.Data[i]
                    .magnitude; // Assuming directions is already a unit vector
      output.Data[i] = Mathf.Clamp(output.Data[i], -1.0f, 1.0f);
      output.Data[i] = Mathf.Acos(output.Data[i]);
      output.Data[i] *= Mathf.Rad2Deg;
    }
    return output;
  }
  /// We're using this as 2D
  public static BComponent<float>
  GenerateNewAngle(Vector3 directions, BComponent<Vector3> newDirections,
                   BComponent<float> output) {
    Assert.AreEqual(newDirections.Length, output.Length,
                    "Direction and output have different sizes.");
    int length = newDirections.Length;
    for (int i = 0; i < length; i++) {
      float dot = Vector3.Dot(directions, newDirections.Data[i]);
      output.Data[i] =
          dot / newDirections.Data[i]
                    .magnitude; // Assuming directions is already a unit vector
      output.Data[i] = Mathf.Clamp(output.Data[i], -1.0f, 1.0f);
      output.Data[i] = Mathf.Acos(output.Data[i]);
      output.Data[i] *= Mathf.Rad2Deg;
    }
    return output;
  }
}
}
