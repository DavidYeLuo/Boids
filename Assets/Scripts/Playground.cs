using System.Collections.Generic;
using UnityEngine;
using Boids;

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

    BoidsManager.GetRandVectors(-SCREEN_WIDTH / 2, SCREEN_WIDTH / 2,
                                -SCREEN_HEIGHT / 2, SCREEN_HEIGHT / 2,
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
    /// Calculates direction
    BoidsMath.GetAvoidanceVector(positions, avoidance);
    BoidsMath.GetCoheranceVector(positions, coherance);
    BoidsMath.Sub(
        coherance, positions,
        coherance); // TODO: Incorporate this ooperation to coherance vector
    BoidsMath.GetAlignmentVector(velocities, alignment);
    BoidsMath.Normalize(avoidance, avoidance);
    BoidsMath.Normalize(coherance, coherance);
    BoidsMath.Normalize(alignment, alignment);
    BoidsMath.Scale(avoidanceFactor, avoidance, avoidance);
    BoidsMath.Scale(coheranceFactor, coherance, coherance);
    BoidsMath.Scale(alignmentFactor, alignment, alignment);
    BoidsMath.Sum(avoidance, coherance, velocities);
    BoidsMath.Sum(velocities, alignment, velocities);
    BoidsMath.Normalize(velocities, velocities);

    /// scale the unit vector by speed
    BoidsMath.Scale(speed * Time.deltaTime, velocities, velocities);

    BoidsMath.Sum(positions, velocities, positions); // Updates position

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
    // Debug.DrawLine(new Vector3(-SCREEN_WIDTH / 2, -SCREEN_HEIGHT / 2),
    //                new Vector3(SCREEN_WIDTH / 2, -SCREEN_HEIGHT / 2),
    //                Color.green);
    // Debug.DrawLine(new Vector3(-SCREEN_WIDTH / 2, -SCREEN_HEIGHT / 2),
    //                new Vector3(-SCREEN_WIDTH / 2, SCREEN_HEIGHT / 2),
    //                Color.green);
    // Debug.DrawLine(new Vector3(SCREEN_WIDTH / 2, SCREEN_HEIGHT / 2),
    //                new Vector3(SCREEN_WIDTH / 2, -SCREEN_HEIGHT / 2),
    //                Color.green);
    // Debug.DrawLine(new Vector3(SCREEN_WIDTH / 2, SCREEN_HEIGHT / 2),
    //                new Vector3(-SCREEN_WIDTH / 2, SCREEN_HEIGHT / 2),
    //                Color.green);
  }
}
}
