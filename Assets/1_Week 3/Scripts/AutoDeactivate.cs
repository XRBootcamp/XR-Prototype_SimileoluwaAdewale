using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDeactivate : MonoBehaviour
{
    [Tooltip("Time in seconds before this UI element deactivates.")]
    public float delay = 6.8f;

    private void OnEnable()
    {
        Invoke(nameof(Deactivate), delay);
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(Deactivate));
    }
}
