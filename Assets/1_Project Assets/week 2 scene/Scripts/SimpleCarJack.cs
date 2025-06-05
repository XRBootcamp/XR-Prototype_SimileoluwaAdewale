using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SimpleCarJack : MonoBehaviour
{
    [SerializeField] private Transform car;
    [SerializeField] private Transform lever;
    [SerializeField] private float liftPerDegree = 0.05f;
    [SerializeField] private Toggle jackStepToggle;  // UI toggle to mark step as done

    private float lastAngle;
    private bool toggleSet = false;

    void Start()
    {
        lastAngle = lever.localEulerAngles.y;
    }

    void Update()
    {
        float currentAngle = lever.localEulerAngles.y;
        float deltaAngle = Mathf.DeltaAngle(lastAngle, currentAngle);

        if (deltaAngle > 0)
        {
            float liftAmount = deltaAngle * liftPerDegree;
            car.position += new Vector3(0f, liftAmount, 0f);

            // Set toggle on first valid movement
            if (!toggleSet && jackStepToggle != null)
            {
                jackStepToggle.isOn = true;
                toggleSet = true;
            }
        }

        lastAngle = currentAngle;
    }
}