using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarGameManager : MonoBehaviour

{
    [SerializeField] private GameObject Tire;
    [SerializeField] private GameObject car;
    [SerializeField] private float vibrationDuration = 1f;
    [SerializeField] private float vibrationMagnitude = 0.02f;
    [SerializeField] private AudioSource carSound;

    private Vector3 originalPos;
    private float timer = 0f;
    private bool isVibrating = false;

    void Update()
    {
        if (isVibrating)
        {
            carSound.Play();
            timer += Time.deltaTime;

            if (timer < vibrationDuration)
            {
                Vector3 offset = Random.insideUnitSphere * vibrationMagnitude;
                car.transform.localPosition = originalPos + offset;
            }
            else
            {
                car.transform.localPosition = originalPos;
                isVibrating = false;
                timer = 0f;
            }
        }
    }

    public void Carvibrate()
    {
        if (!isVibrating)
        {
            originalPos = car.transform.localPosition;
            isVibrating = true;
        }
    }

    public void StopVibration()
    {
        isVibrating = false;
        timer = 0f;
        car.transform.localPosition = originalPos;
        carSound.Stop();
    }
}
