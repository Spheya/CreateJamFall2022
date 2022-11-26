using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FlipAnimator;

public class CameraShake : MonoBehaviour
{
    private float intensity = 0.0f;
    public float fallOff = 1.0f;
    public float amplitude = 0.1f;
    public float frequency = 0.1f;

    public void Shake(float intensity)
    {
        this.intensity += intensity;
    }

    private void Update()
    {
        intensity -= fallOff * Time.deltaTime;
        if (intensity < 0.0f)
            intensity = 0.0f;

        float perlin1 = Mathf.PerlinNoise(0.873712f, Time.time * frequency) * amplitude * intensity;
        float perlin2 = Mathf.PerlinNoise(4.312432f, Time.time * frequency) * amplitude * intensity;

        transform.localPosition = new Vector3(perlin1, perlin2, transform.localPosition.z);
    }

}