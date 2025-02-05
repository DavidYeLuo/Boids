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

    Vector3[] samples = { new Vector3(0.0f, 0.0f), new Vector3(1.0f, 0.0f) };
    Vector3[] expected = { new Vector3(-1.0f, 0.0f), new Vector3(1.0f, 0.0f) };
    BComponent<Vector3> sampleComponent = new BComponent<Vector3>(samples, 2);
    BComponent<Vector3> resultComponent =
        new BComponent<Vector3>(new Vector3[2], 2);

    BoidsMath.GetAvoidanceVector(sampleComponent, resultComponent);
    for (int i = 0; i < samples.Length; i++) {
      Vector3 buffer = expected[i] - resultComponent.Data[i];
      Assert.Less(Mathf.Abs(buffer.x), marginOfError.x);
      Assert.Less(Mathf.Abs(buffer.y), marginOfError.y);
      Assert.Less(Mathf.Abs(buffer.z), marginOfError.z);
    }
  }
  [Test]
  public void ComplexAvoidanceTest() {
    Vector3 marginOfError = new Vector3(0.01f, 0.01f, 0.01f);

    Vector3[] samples = { new Vector3(0.0f, 1.0f), new Vector3(0.0f, 0.0f),
                          new Vector3(1.0f, 0.0f) };
    Vector3[] expected = { new Vector3(-1.0f / Mathf.Pow(2.0f, 0.25f),
                                       1.0f / Mathf.Pow(2.0f, 0.25f) + 1),
                           new Vector3(-1.0f, -1.0f),
                           new Vector3(1.0f / Mathf.Pow(2.0f, 0.25f) + 1,
                                       -1.0f / Mathf.Pow(2.0f, 0.25f)) };
    BComponent<Vector3> sampleComponent = new BComponent<Vector3>(samples, 3);
    BComponent<Vector3> resultComponent =
        new BComponent<Vector3>(new Vector3[3], 3);
    BoidsMath.GetAvoidanceVector(sampleComponent, resultComponent);
    for (int i = 0; i < samples.Length; i++) {
      Vector3 buffer = expected[i] - resultComponent.Data[i];
      Assert.Less(Mathf.Abs(buffer.x), marginOfError.x);
      Assert.Less(Mathf.Abs(buffer.y), marginOfError.y);
      Assert.Less(Mathf.Abs(buffer.z), marginOfError.z);
    }
  }
  [Test]
  public void SimpleCoheranceTest() {
    Vector3 marginOfError = new Vector3(0.01f, 0.01f, 0.01f);

    Vector3[] samples = { new Vector3(0.0f, 0.0f), new Vector3(1.0f, 0.0f) };
    Vector3[] expectedVecs = { new Vector3(0.5f, 0.0f),
                               new Vector3(-0.5f, 0.0f) };

    BComponent<Vector3> sampleComponent = new BComponent<Vector3>(samples, 2);
    BComponent<Vector3> resultComponent =
        new BComponent<Vector3>(new Vector3[2], 2);
    BoidsMath.GetCoheranceVector(sampleComponent, resultComponent);

    for (int i = 0; i < samples.Length; i++) {
      Vector3 buffer = expectedVecs[i] - resultComponent.Data[i];
      Assert.Less(Mathf.Abs(buffer.x), marginOfError.x);
      Assert.Less(Mathf.Abs(buffer.y), marginOfError.y);
      Assert.Less(Mathf.Abs(buffer.z), marginOfError.z);
    }
  }
  [Test]
  public void SimpleAlignmentTest() {
    Vector3 marginOfError = new Vector3(0.01f, 0.01f, 0.01f);

    Vector3[] samples = { new Vector3(0.0f, 0.0f), new Vector3(1.0f, 0.0f) };
    Vector3[] expectedVecs = { new Vector3(0.5f, 0.0f),
                               new Vector3(0.5f, 0.0f) };

    BComponent<Vector3> sampleComponent = new BComponent<Vector3>(samples, 2);
    BComponent<Vector3> resultComponent =
        new BComponent<Vector3>(new Vector3[2], 2);
    BoidsMath.GetAlignmentVector(sampleComponent, resultComponent);

    for (int i = 0; i < samples.Length; i++) {
      Vector3 buffer = expectedVecs[i] - resultComponent.Data[i];
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

    BComponent<Vector3> leftComponent = new BComponent<Vector3>(left, 3);
    BComponent<Vector3> rigthComponent = new BComponent<Vector3>(right, 3);
    BComponent<Vector3> resultComponent =
        new BComponent<Vector3>(new Vector3[3], 3);
    BoidsMath.Sum(leftComponent, rigthComponent, resultComponent);
    for (int i = 0; i < 3; i++) {
      Vector3 buffer = resultComponent.Data[i] - resultComponent.Data[i];
      Assert.Less(Mathf.Abs(buffer.x), marginOfError.x);
      Assert.Less(Mathf.Abs(buffer.y), marginOfError.y);
      Assert.Less(Mathf.Abs(buffer.z), marginOfError.z);
    }
  }
  [Test]
  public void SimpleAlignmentCoheranceAligmentTest() {
    Vector3 marginOfError = new Vector3(0.01f, 0.01f, 0.01f);

    Vector3[] samples = { new Vector3(0.0f, 0.0f), new Vector3(1.0f, 0.0f) };
    Vector3[] expected = { new Vector3(0.0f, 0.0f), new Vector3(1.0f, 0.0f) };
    BComponent<Vector3> sampleComponent = new BComponent<Vector3>(samples, 2);
    BComponent<Vector3> avoidanceComponent =
        new BComponent<Vector3>(new Vector3[2], 2);
    BComponent<Vector3> coheranceComponent =
        new BComponent<Vector3>(new Vector3[2], 2);
    BComponent<Vector3> alignmentComponent =
        new BComponent<Vector3>(new Vector3[2], 2);
    BComponent<Vector3> resultComponent =
        new BComponent<Vector3>(new Vector3[2], 2);
    BoidsMath.GetAvoidanceVector(sampleComponent,
                                 avoidanceComponent); // [-1, 0], [1, 0]
    BoidsMath.GetCoheranceVector(sampleComponent,
                                 coheranceComponent); // [0.5, 0], [-0.5, 0]
    BoidsMath.GetAlignmentVector(sampleComponent,
                                 alignmentComponent); // [0.5, 0], [0.5, 0]
    BoidsMath.Sum(avoidanceComponent, coheranceComponent, resultComponent);
    BoidsMath.Sum(resultComponent, alignmentComponent, resultComponent);

    for (int i = 0; i < samples.Length; i++) {
      Vector3 buffer = expected[i] - resultComponent.Data[i];
      Assert.Less(Mathf.Abs(buffer.x), marginOfError.x);
      Assert.Less(Mathf.Abs(buffer.y), marginOfError.y);
      Assert.Less(Mathf.Abs(buffer.z), marginOfError.z);
    }
  }
  [Test]
  public void ScaleVectorTest() {
    Vector3 marginOfError = new Vector3(0.01f, 0.01f, 0.01f);
    float[] scalar = { 0.0f, 1.0f, 8.0f, -4.0f, 1.0f };
    Vector3[] vector = { new Vector3(100.0f, 100.0f, 100.0f),
                         new Vector3(123.0f, 432.0f, 213.0f),
                         new Vector3(1.0f, 2.0f, 3.0f),
                         new Vector3(3.0f, 2.0f, 1.0f),
                         new Vector3(0.0f, 0.0f, 0.0f) };
    Vector3[] expected = { new Vector3(0.0f, 0.0f, 0.0f),
                           new Vector3(123.0f, 432.0f, 213.0f),
                           new Vector3(8.0f, 16.0f, 24.0f),
                           new Vector3(-12.0f, -8.0f, -4.0f),
                           new Vector3(0.0f, 0.0f, 0.0f) };
    BComponent<float> scalarComponent = new BComponent<float>(scalar, 5);
    BComponent<Vector3> vectorComponent = new BComponent<Vector3>(vector, 5);
    BComponent<Vector3> resultComponent =
        new BComponent<Vector3>(new Vector3[5], 5);
    BoidsMath.Scale(scalarComponent, vectorComponent, resultComponent);

    for (int i = 0; i < 5; i++) {
      Vector3 buffer = expected[i] - resultComponent.Data[i];
      Assert.Less(Mathf.Abs(buffer.x), marginOfError.x);
      Assert.Less(Mathf.Abs(buffer.y), marginOfError.y);
      Assert.Less(Mathf.Abs(buffer.z), marginOfError.z);
    }
  }
  [Test]
  public void NormalizeVectorTest() {
    Vector3 marginOfError = new Vector3(0.01f, 0.01f, 0.01f);
    Vector3[] vector = { new Vector3(1.0f, 0.0f, 0.0f),
                         new Vector3(0.5f, 0.5f, 0.0f) };
    Vector3[] expected = { new Vector3(1.0f, 0.0f, 0.0f),
                           (new Vector3(0.5f, 0.5f, 0.0f)).normalized };
    BComponent<Vector3> vectorComponent = new BComponent<Vector3>(vector, 2);
    BComponent<Vector3> resultComponent =
        new BComponent<Vector3>(new Vector3[2], 2);
    BoidsMath.Normalize(vectorComponent, resultComponent);

    for (int i = 0; i < 2; i++) {
      Vector3 buffer = expected[i] - resultComponent.Data[i];
      Assert.Less(Mathf.Abs(buffer.x), marginOfError.x);
      Assert.Less(Mathf.Abs(buffer.y), marginOfError.y);
      Assert.Less(Mathf.Abs(buffer.z), marginOfError.z);
    }
  }
  [Test]
  public void WrapPosition() {
    Vector3 marginOfError = new Vector3(0.01f, 0.01f, 0.01f);
    Vector3[] vector = { new Vector3(12.0f, 0.0f, 0.0f),
                         new Vector3(0.0f, 12.0f, 0.0f) };
    Vector3[] expected = { new Vector3(2.0f, 0.0f, 0.0f),
                           new Vector3(0.0f, 2.0f, 0.0f) };
    BComponent<Vector3> vectorComponent = new BComponent<Vector3>(vector, 2);
    BComponent<Vector3> resultComponent =
        new BComponent<Vector3>(new Vector3[2], 2);
    BoidsMath.WrapBetween(10.0f, 10.0f, vectorComponent, resultComponent);

    for (int i = 0; i < 2; i++) {
      Vector3 buffer = expected[i] - resultComponent.Data[i];
      Assert.Less(Mathf.Abs(buffer.x), marginOfError.x);
      Assert.Less(Mathf.Abs(buffer.y), marginOfError.y);
    }
  }
}
