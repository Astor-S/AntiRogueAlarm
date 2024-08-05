using System.Collections.Generic;
using UnityEngine;

public class PathFollower : MonoBehaviour
{
    [SerializeField] private List<Vector3> _waypoints;
    [SerializeField] private float _speed = 2f;

    private int _currentWaypointIndex = 0;

    private void Update()
    {
        if (_currentWaypointIndex < _waypoints.Count)
        {
            Vector3 targetPosition = _waypoints[_currentWaypointIndex];
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, _speed * Time.deltaTime);

            if (transform.position == targetPosition)
            {
                _currentWaypointIndex++;
            }
        }
    }
}