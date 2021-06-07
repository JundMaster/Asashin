using UnityEngine;

/// <summary>
/// Class responsible for handling patrol points waiting time.
/// </summary>
public class EnemyPatrolPoint : MonoBehaviour
{
    [Range(0.01f, 10f)][SerializeField] private float waitingTime;
    public float WaitingTime => waitingTime;
}
