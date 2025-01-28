using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Boids;

public class CorrectnessTests {
  // Debugs On Fail
  private void DebugVector(Vector3 current, Vector3 expected,
                           Vector3 marginOfError) {
    Vector3 temp = expected - current;
    if (temp.x > marginOfError.x || temp.y > marginOfError.y ||
        temp.z > marginOfError.z)
      Debug.LogFormat("[{0}, {1}, {2}], [{3}, {4}, {5}]", current.x, current.y,
                      current.z, expected.x, expected.y, expected.z);
  }
  [Test]
  public void SimpleAvoidanceTest() {
    Vector3 marginOfError = new Vector3(0.01f, 0.01f, 0.01f);

    Vector3[] threeObj = { new Vector3(0.0f, 0.0f), new Vector3(1.0f, 0.0f) };
    Vector3[] expectedVecs = { new Vector3(-1.0f, 0.0f),
                               new Vector3(1.0f, 0.0f) };
    Vector3[] avoidanceVecs = BoidsMath.GetAvoidanceVector(threeObj);
    for (int i = 0; i < threeObj.Length; i++) {
      Vector3 buffer = expectedVecs[i] - avoidanceVecs[i];
      Assert.Less(Mathf.Abs(buffer.x), marginOfError.x);
      Assert.Less(Mathf.Abs(buffer.y), marginOfError.y);
      Assert.Less(Mathf.Abs(buffer.z), marginOfError.z);
    }
  }
  [Test]
  public void ComplexAvoidanceTest() {
    Vector3 marginOfError = new Vector3(0.01f, 0.01f, 0.01f);

    Vector3[] threeObj = { new Vector3(0.0f, 1.0f), new Vector3(0.0f, 0.0f),
                           new Vector3(1.0f, 0.0f) };
    Vector3[] expectedVecs = { new Vector3(-1.0f / Mathf.Pow(2.0f, 0.25f),
                                           1.0f / Mathf.Pow(2.0f, 0.25f) + 1),
                               new Vector3(-1.0f, -1.0f),
                               new Vector3(1.0f / Mathf.Pow(2.0f, 0.25f) + 1,
                                           -1.0f / Mathf.Pow(2.0f, 0.25f)) };
    Vector3[] avoidanceVecs = BoidsMath.GetAvoidanceVector(threeObj);
    for (int i = 0; i < threeObj.Length; i++) {
      Vector3 buffer = expectedVecs[i] - avoidanceVecs[i];
      Assert.Less(Mathf.Abs(buffer.x), marginOfError.x);
      Assert.Less(Mathf.Abs(buffer.y), marginOfError.y);
      Assert.Less(Mathf.Abs(buffer.z), marginOfError.z);
    }
  }
  [Test]
  public void SimpleCoheranceTest() {
    Vector3 marginOfError = new Vector3(0.01f, 0.01f, 0.01f);

    Vector3[] threeObj = { new Vector3(0.0f, 0.0f), new Vector3(1.0f, 0.0f) };
    Vector3[] avoidanceVecscoheranceVecs =
        BoidsMath.GetCoheranceVector(threeObj);

    Vector3[] expectedVecs = { new Vector3(0.5f, 0.0f),
                               new Vector3(0.5f, 0.0f) };

    for (int i = 0; i < threeObj.Length; i++) {
      Vector3 buffer = expectedVecs[i] - avoidanceVecscoheranceVecs[i];
      Assert.Less(Mathf.Abs(buffer.x), marginOfError.x);
      Assert.Less(Mathf.Abs(buffer.y), marginOfError.y);
      Assert.Less(Mathf.Abs(buffer.z), marginOfError.z);
    }
  }
  [Test]
  public void SimpleAlignmentTest() {
    Vector3 marginOfError = new Vector3(0.01f, 0.01f, 0.01f);

    Vector3[] threeObj = { new Vector3(0.0f, 0.0f), new Vector3(1.0f, 0.0f) };
    Vector3[] avoidanceVecs = BoidsMath.GetAlignmentVector(threeObj);
    Vector3[] expectedVecs = { new Vector3(0.5f, 0.0f),
                               new Vector3(0.5f, 0.0f) };

    for (int i = 0; i < threeObj.Length; i++) {
      Vector3 buffer = expectedVecs[i] - avoidanceVecs[i];
      Assert.Less(Mathf.Abs(buffer.x), marginOfError.x);
      Assert.Less(Mathf.Abs(buffer.y), marginOfError.y);
      Assert.Less(Mathf.Abs(buffer.z), marginOfError.z);
    }
  }
  [Test]
  public void SumVecsTest() {
    Vector3 marginOfError = new Vector3(0.01f, 0.01f, 0.01f);

    Vector3[] left = { new Vector3(1, 1, 1), new Vector3(2, 2, 2),
                       new Vector3(3, 3, 3) };
    Vector3[] right = { new Vector3(4, 4, 4), new Vector3(5, 5, 5),
                        new Vector3(5, 5, 5) };
    Vector3[] expected = { new Vector3(5, 5, 5), new Vector3(7, 7, 7),
                           new Vector3(8, 8, 8) };

    Vector3[] sum = BoidsMath.Sum(left, right);
    for (int i = 0; i < 3; i++) {
      Vector3 buffer = sum[i] - sum[i];
      Assert.Less(Mathf.Abs(buffer.x), marginOfError.x);
      Assert.Less(Mathf.Abs(buffer.y), marginOfError.y);
      Assert.Less(Mathf.Abs(buffer.z), marginOfError.z);
    }
  }
  [Test]
  public void SimpleAlignmentCoheranceAligmentTest() {
    Vector3 marginOfError = new Vector3(0.01f, 0.01f, 0.01f);

    Vector3[] threeObj = { new Vector3(0.0f, 0.0f), new Vector3(1.0f, 0.0f) };
    Vector3[] avoidanceVecs =
        BoidsMath.GetAvoidanceVector(threeObj); // [-1, 0], [1, 0]
    Vector3[] coheranceVecs =
        BoidsMath.GetCoheranceVector(threeObj); // [0.5, 0], [0.5, 0]
    Vector3[] alignmentVecs =
        BoidsMath.GetAlignmentVector(threeObj); // [0.5, 0], [0.5, 0]
    Vector3[] sum = BoidsMath.Sum(avoidanceVecs, coheranceVecs);
    sum = BoidsMath.Sum(sum, alignmentVecs);
    Vector3[] expectedVecs = { new Vector3(0.0f, 0.0f),
                               new Vector3(2.0f, 0.0f) };

    for (int i = 0; i < threeObj.Length; i++) {
      Vector3 buffer = expectedVecs[i] - sum[i];
      Assert.Less(Mathf.Abs(buffer.x), marginOfError.x);
      Assert.Less(Mathf.Abs(buffer.y), marginOfError.y);
      Assert.Less(Mathf.Abs(buffer.z), marginOfError.z);
    }
  }
}
