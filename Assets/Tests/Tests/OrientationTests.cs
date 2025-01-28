using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Boids;

public class OrientationTests {
  // A Test behaves as an ordinary method
  [Test]
  public void OrientationSimplePasses() {
    // Use the Assert class to test conditions
    GameObject boids = new GameObject();
    boids.transform.rotation = Quaternion.identity;
    boids.transform.Rotate(0, 0, 30);

    float dot = Vector3.Dot(boids.transform.right,
                            new Vector3(Mathf.Cos(30 * Mathf.Deg2Rad),
                                        Mathf.Sin(30 * Mathf.Deg2Rad), 0.0f));
    Assert.Greater(dot + 0.0001f, 0.0f);
    Assert.Less(Mathf.Abs(1 - dot), 0.00001f);
  }
  [Test]
  public void OrientationWithUnknownStart() {
    // Use the Assert class to test conditions
    GameObject boids = new GameObject();
    boids.transform.rotation = Quaternion.Euler(0, 0, 40);
    boids.transform.Rotate(0, 0, 30);

    float dot =
        Quaternion.Dot(Quaternion.Euler(0, 0, 70), boids.transform.rotation);
    Assert.Less(Mathf.Abs(1 - dot), 0.001);
  }
}
