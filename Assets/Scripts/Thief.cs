using UnityEngine;

public class Thief : MonoBehaviour 
{
    private Vector3 _targetPosition;

    private void Update()
    {
        transform.LookAt(_targetPosition);
    }
}