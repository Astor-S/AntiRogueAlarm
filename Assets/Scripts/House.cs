using System;
using UnityEngine;

public class House : MonoBehaviour
{
    public Action HouseBreakInDetected;
    public Action HouseBreakOutDetected;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Thief _))
        {
            HouseBreakInDetected?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Thief _))
        {
            HouseBreakOutDetected?.Invoke();
        }
    }
}