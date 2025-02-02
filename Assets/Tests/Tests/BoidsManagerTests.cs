using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Boids;

public class BoidsStarterTests {
  [Test]
  public void AngleGenerationTest() {
    float marginOfError = 0.01f;
    Vector3[] startAngles = { new Vector3(1.0f, 0.0f, 0.0f),
                              new Vector3(0.0f, 1.0f, 0.0f) };
    Vector3[] endAngles = { new Vector3(Mathf.PI / 4, Mathf.PI / 4, 0.0f),
                            new Vector3(Mathf.PI / 4, Mathf.PI / 4, 0.0f) };
    float[] expected = { 45.0f, 45.0f };

    BComponent<Vector3> startComponent =
        new BComponent<Vector3>(startAngles, 2);
    BComponent<Vector3> endComponent = new BComponent<Vector3>(endAngles, 2);
    BComponent<float> result = new BComponent<float>(new float[2], 2);

    BoidsManager.GenerateNewAngle(startComponent, endComponent, result);
    for (int i = 0; i < 2; i++) {
      float buffer = expected[i] - result.Data[i];
      Assert.Less(Mathf.Abs(buffer), marginOfError);
    }
  }
}
