using UnityEngine;

/// <summary>
/// Class responsible for handling patrol points waiting time.
/// </summary>
public class EnemyPatrolPoint : MonoBehaviour
{
    [SerializeField] private float waitingTime;
    public float WaitingTime => waitingTime;
}
