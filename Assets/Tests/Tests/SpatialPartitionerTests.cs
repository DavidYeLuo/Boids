using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using BOptimizer;
using Boids;

public class SpatialPartitionerTests {
  [Test]
  public void InitializationTest() {
    float spaceUnit = 1.0f;
    float screenWidth = 19.2f;
    float screenHeight = 10.8f;
    int maxEntity = 10;
    SpatialPartitioner<Vector3> sp = new SpatialPartitioner<Vector3>(
        spaceUnit, screenWidth, screenHeight, maxEntity);
    Assert.AreEqual(spaceUnit, sp.SpaceUnit,
                    "Space unit isn't initialized accordingly");
    Assert.AreEqual(screenWidth, sp.ScreenWidth,
                    "Screen Width isn't initialized properly");
    Assert.AreEqual(screenHeight, sp.ScreenHeight,
                    "Screen Height isn't initialized properly");
    Assert.AreEqual(maxEntity, sp.SpaceMaxEntity,
                    "Space entity isn't initialized properly");
  }
  [Test]
  public void UpdateIndexTest() {
    float divisionUnit = 1.0f;
    float screenWidth = 19.2f;
    float screenHeight = 10.8f;
    int maxEntity = 10;
    Vector3[] vec = { new Vector3(0.0f, 0.0f, 0.0f),
                      new Vector3(1.0f, 1.0f, 0.0f),
                      new Vector3(2.0f, 2.0f, 0.0f),
                      new Vector3(2.5f, 2.5f, 0.0f),
                      new Vector3(3.5f, 3.5f, 0.0f) };
    BComponent<Vector3> sample = new BComponent<Vector3>(vec, 5);
    SpatialPartitioner<Vector3> sp = new SpatialPartitioner<Vector3>(
        divisionUnit, screenWidth, screenHeight, maxEntity);

    BComponent<int>[
      ,
    ] indeces = sp.UpdateIndex(sample);
    Assert.AreEqual(0, indeces[0, 0].Data[0]);
    Assert.AreEqual(1, indeces[1, 1].Data[0]);
    Assert.AreEqual(2, indeces[2, 2].Data[0]);
    Assert.AreEqual(3, indeces[2, 2].Data[1]);
    Assert.AreEqual(4, indeces[3, 3].Data[0]);
  }
  [Test]
  public void PartitionDimensionTest() {
    float divisionUnit = 1.0f;
    float screenWidth = 19.2f;
    float screenHeight = 10.8f;
    int maxEntity = 10;
    Vector3[] vec = { new Vector3(0.0f, 0.0f, 0.0f),
                      new Vector3(1.0f, 1.0f, 0.0f),
                      new Vector3(2.0f, 2.0f, 0.0f),
                      new Vector3(2.5f, 2.5f, 0.0f),
                      new Vector3(3.5f, 3.5f, 0.0f) };
    BComponent<Vector3> sample = new BComponent<Vector3>(vec, 3);
    SpatialPartitioner<Vector3> sp = new SpatialPartitioner<Vector3>(
        divisionUnit, screenWidth, screenHeight, maxEntity);

    BComponent<Vector3>[
      ,
    ] ret = sp.MallocSpace<Vector3>();

    sp.UpdateIndex(sample);
    ret = sp.Partition(sample, ret);
    // Dimension Test
    // Inclusive range
    // [0, 19]
    // [0, 11]
    Assert.AreEqual(20, ret.GetLength(0), "Row doesn't match as expected");
    Assert.AreEqual(11, ret.GetLength(1), "Column doesn't match as expected");
  }
  [Test]
  public void PartitionSpaceTest() {
    float divisionUnit = 1.0f;
    float screenWidth = 19.2f;
    float screenHeight = 10.8f;
    int maxEntity = 10;
    Vector3[] vec = { new Vector3(0.0f, 0.0f, 0.0f),
                      new Vector3(1.0f, 1.0f, 0.0f),
                      new Vector3(2.0f, 2.0f, 0.0f),
                      new Vector3(2.5f, 2.5f, 0.0f),
                      new Vector3(3.5f, 3.5f, 0.0f) };
    BComponent<Vector3> sample = new BComponent<Vector3>(vec, 5);
    SpatialPartitioner<Vector3> sp = new SpatialPartitioner<Vector3>(
        divisionUnit, screenWidth, screenHeight, maxEntity);

    BComponent<Vector3>[
      ,
    ] ret = sp.MallocSpace<Vector3>();
    sp.UpdateIndex(sample);
    ret = sp.Partition(sample, ret);

    for (int row = 0; row < 20; row++) {
      for (int col = 0; col < 11; col++) {
        if (row == 0 && col == 0 || row == 1 && col == 1 ||
            row == 2 && col == 2 || row == 3 && col == 3)
          continue;
        Assert.AreEqual(0, ret[row, col].Length);
      }
    }
    Assert.AreEqual(1, ret[0, 0].Length);
    Assert.AreEqual(1, ret[1, 1].Length);
    Assert.AreEqual(2, ret[2, 2].Length);
    Assert.AreEqual(1, ret[3, 3].Length);
  }
  [Test]
  public void PartitionValueTest() {
    float divisionUnit = 1.0f;
    float screenWidth = 19.2f;
    float screenHeight = 10.8f;
    int maxEntity = 10;
    Vector3[] vec = { new Vector3(0.0f, 0.0f, 0.0f),
                      new Vector3(1.0f, 1.0f, 0.0f),
                      new Vector3(2.0f, 2.0f, 0.0f),
                      new Vector3(2.5f, 2.5f, 0.0f),
                      new Vector3(3.5f, 3.5f, 0.0f) };
    BComponent<Vector3> sample = new BComponent<Vector3>(vec, 5);
    SpatialPartitioner<Vector3> sp = new SpatialPartitioner<Vector3>(
        divisionUnit, screenWidth, screenHeight, maxEntity);

    BComponent<Vector3>[
      ,
    ] ret = sp.MallocSpace<Vector3>();
    sp.UpdateIndex(sample);
    ret = sp.Partition(sample, ret);
    Debug.Log(ret[0, 0].Data[0]);
    Debug.Log(ret[1, 1].Data[0]);
    Debug.Log(ret[2, 2].Data[0]);
    Debug.Log(ret[2, 2].Data[1]);
    Debug.Log(ret[3, 3].Data[0]);

    Assert.AreEqual(new Vector3(0.0f, 0.0f, 0.0f), ret[0, 0].Data[0]);
    Assert.AreEqual(new Vector3(1.0f, 1.0f, 0.0f), ret[1, 1].Data[0]);
    Assert.AreEqual(new Vector3(2.0f, 2.0f, 0.0f), ret[2, 2].Data[0]);
    Assert.AreEqual(new Vector3(2.5f, 2.5f, 0.0f), ret[2, 2].Data[1]);
    Assert.AreEqual(new Vector3(3.5f, 3.5f, 0.0f), ret[3, 3].Data[0]);
  }
  [Test]
  public void WriteToTest() {
    float divisionUnit = 1.0f;
    float screenWidth = 19.2f;
    float screenHeight = 10.8f;
    int maxEntity = 10;
    Vector3[] vec = { new Vector3(0.0f, 0.0f, 0.0f),
                      new Vector3(1.0f, 1.0f, 0.0f),
                      new Vector3(2.0f, 2.0f, 0.0f),
                      new Vector3(2.5f, 2.5f, 0.0f),
                      new Vector3(3.5f, 3.5f, 0.0f) };
    BComponent<Vector3> sample = new BComponent<Vector3>(vec, 5);
    SpatialPartitioner<Vector3> sp = new SpatialPartitioner<Vector3>(
        divisionUnit, screenWidth, screenHeight, maxEntity);

    BComponent<Vector3>[
      ,
    ] ret = sp.MallocSpace<Vector3>();
    sp.UpdateIndex(sample);
    ret = sp.Partition(sample, ret);

    Vector3 mutation = new Vector3(0.3f, 0.23f, 0.0f);
    ret[0, 0].Data[0] = mutation;
    sp.Write(ret, sample);
    Assert.AreEqual(mutation, sample.Data[0]);
  }
}
