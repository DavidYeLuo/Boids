using System.Collections.Generic;
using UnityEngine;
using Boids;
using BOptimizer;

namespace Playground {
public class ECSPrototype : MonoBehaviour {
  [Header("Prefab")]
  public GameObject boidPrefab;

  [Header("Config")]
  public int numBoids;
  public float speed;
  public float detectionDistance = 5.0f;
  public float coheranceFactor = 4.0f;
  public float alignmentFactor = 3.0f;
  public float avoidanceFactor = 4.0f;
  [Space]
  /// Defaults to 1080p
  public float SCREEN_WIDTH = 19.20f;
  public float SCREEN_HEIGHT = 10.8f;

  private List<GameObject> boids;
  private BComponent<Vector3> positions;
  private BComponent<Vector3> velocities;
  private BComponent<float> angles;

  /// For calculating the trajectory
  private BComponent<Vector3> coherance;
  private BComponent<Vector3> alignment;
  private BComponent<Vector3> avoidance;
  private BComponent<Vector3>[
    ,
  ] partitionedPositions;
  private BComponent<Vector3>[
    ,
  ] partitionedVelocities;
  private BComponent<float>[
    ,
  ] partitionedAngles;
  private BComponent<Vector3>[
    ,
  ] partitionedCoherance;
  private BComponent<Vector3>[
    ,
  ] partitionedAlignment;
  private BComponent<Vector3>[
    ,
  ] partitionedAvoidance;

  /// Spatial Partitioner
  private SpatialPartitioner spatialPartitioner;

  [SerializeField]
  int numJumpFrame;
  private int countFrame;

  private void Awake() {
    boids = new List<GameObject>(numBoids);
    positions = new BComponent<Vector3>(new Vector3[numBoids], numBoids);
    velocities = new BComponent<Vector3>(new Vector3[numBoids], numBoids);
    angles = new BComponent<float>(new float[numBoids], numBoids);
    coherance = new BComponent<Vector3>(new Vector3[numBoids], numBoids);
    alignment = new BComponent<Vector3>(new Vector3[numBoids], numBoids);
    avoidance = new BComponent<Vector3>(new Vector3[numBoids], numBoids);
    for (int i = 0; i < numBoids; i++) {
      boids.Add(Instantiate(boidPrefab));
    }
    spatialPartitioner = new SpatialPartitioner(detectionDistance, SCREEN_WIDTH,
                                                SCREEN_HEIGHT, numBoids);
    partitionedPositions = spatialPartitioner.MallocSpace<Vector3>();
    partitionedVelocities = spatialPartitioner.MallocSpace<Vector3>();
    partitionedAngles = spatialPartitioner.MallocSpace<float>();
    partitionedCoherance = spatialPartitioner.MallocSpace<Vector3>();
    partitionedAlignment = spatialPartitioner.MallocSpace<Vector3>();
    partitionedAvoidance = spatialPartitioner.MallocSpace<Vector3>();

    /// We're using (0, SCREEN_WIDTH), (0, SCREEN_HEIGHT)
    /// because the Mathf.Repeat only works on positive numbers.
    /// We use Mathf.Repeat to wrap positions around the screen.
    BoidsManager.GetRandVectors(0.0f, SCREEN_WIDTH, 0.0f, SCREEN_HEIGHT,
                                positions);
    BoidsManager.GetRandVectors(-1.0f, 1.0f, velocities);
    BoidsMath.Normalize(velocities, velocities);
    BoidsMath.Scale(speed, velocities, velocities);
  }

  private void Update() {
    if (Input.GetKeyDown(KeyCode.Space)) {
      countFrame = 0;
    }
  }
  private void FixedUpdate() {
    spatialPartitioner.ResetIndex();
    spatialPartitioner.UpdateIndex(positions);

    spatialPartitioner.Resize<Vector3>(partitionedPositions);
    spatialPartitioner.Resize<Vector3>(partitionedVelocities);
    spatialPartitioner.Resize<float>(partitionedAngles);
    spatialPartitioner.Resize<Vector3>(partitionedAlignment);
    spatialPartitioner.Resize<Vector3>(partitionedAvoidance);
    spatialPartitioner.Resize<Vector3>(partitionedCoherance);

    spatialPartitioner.Partition(positions, partitionedPositions);
    spatialPartitioner.Partition(velocities, partitionedVelocities);

    for (int row = 0; row <= spatialPartitioner.MaxRowIndex; row++) {
      for (int col = 0; col <= spatialPartitioner.MaxColIndex; col++) {
        BoidsMath.GetAvoidanceVector(partitionedPositions[row, col],
                                     partitionedAvoidance[row, col]);
        BoidsMath.GetCoheranceVector(partitionedPositions[row, col],
                                     partitionedCoherance[row, col]);
        BoidsMath.GetAlignmentVector(partitionedVelocities[row, col],
                                     partitionedAlignment[row, col]);
        BoidsMath.Normalize(partitionedAvoidance[row, col],
                            partitionedAvoidance[row, col]);
        BoidsMath.Normalize(partitionedCoherance[row, col],
                            partitionedCoherance[row, col]);
        BoidsMath.Normalize(partitionedAlignment[row, col],
                            partitionedAlignment[row, col]);
        BoidsMath.Scale(avoidanceFactor, partitionedAvoidance[row, col],
                        partitionedAvoidance[row, col]);
        BoidsMath.Scale(coheranceFactor, partitionedCoherance[row, col],
                        partitionedCoherance[row, col]);
        BoidsMath.Scale(alignmentFactor, partitionedAlignment[row, col],
                        partitionedAlignment[row, col]);
        BoidsMath.Sum(partitionedVelocities[row, col],
                      partitionedCoherance[row, col],
                      partitionedVelocities[row, col]);
        BoidsMath.Sum(partitionedVelocities[row, col],
                      partitionedAlignment[row, col],
                      partitionedVelocities[row, col]);
        BoidsMath.Sum(partitionedVelocities[row, col],
                      partitionedAvoidance[row, col],
                      partitionedVelocities[row, col]);
        BoidsMath.Normalize(partitionedVelocities[row, col],
                            partitionedVelocities[row, col]);
      }
    }
    spatialPartitioner.Write(partitionedVelocities, velocities);

    /// scale the unit vector by speed
    BoidsMath.Scale(speed * Time.deltaTime, velocities, velocities);

    BoidsMath.Sum(positions, velocities, positions); // Updates position
    BoidsMath.WrapBetween(SCREEN_WIDTH, SCREEN_HEIGHT, positions, positions);

    for (int i = 0; i < numBoids; i++) {
      boids[i].transform.position = positions.Data[i];
      if (velocities.Data[i].x == 0.0f) {
        boids[i].transform.rotation = Quaternion.Euler(0.0f, 0.0f, 90.0f);
        continue;
      }
      float angle = Mathf.Atan2(velocities.Data[i].y, velocities.Data[i].x);
      boids[i].transform.rotation = Quaternion.Euler(
          0.0f, 0.0f,
          angle * Mathf.Rad2Deg - 90.0f); // Object's natural offset is on the
                                          // y-axis so we need to offset it.
      Debug.DrawLine(positions.Data[i],
                     positions.Data[i] + velocities.Data[i].normalized,
                     Color.green);
      Debug.DrawLine(positions.Data[i], positions.Data[i] + avoidance.Data[i],
                     Color.red);
      Debug.DrawLine(positions.Data[i], positions.Data[i] + coherance.Data[i],
                     Color.yellow);
      Debug.DrawLine(positions.Data[i], positions.Data[i] + alignment.Data[i],
                     Color.blue);
    }
  }
}
}
