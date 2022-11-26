using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{
    public float bpm = 60.0f;
    public Hook hook;
    public float transitionLength = 0.1f;
    public MeshRenderer cog;

    private int tick = 0;
    private AudioSource ticker;

    public AudioSource sound;
    private Material material;

    private void OnEnable()
    {
        tick = Mathf.FloorToInt(sound.time * (bpm / 60.0f));
        ticker = GetComponent<AudioSource>();
        material = cog.material;
    }

    void Update()
    {
        int newTick = Mathf.FloorToInt(sound.time * (bpm / 60.0f));

        if(tick != Mathf.FloorToInt((sound.time + transitionLength) * (bpm / 60.0f))) {
            material.mainTextureOffset = new Vector2(((tick % 3) * 2.0f + 1.0f) / 8.0f, 0.0f);
        }

        if (tick != newTick)
        {
            tick = newTick;
            material.mainTextureOffset = new Vector2(((tick % 3) * 2.0f) / 8.0f, 0.0f);

            if (ticker.isPlaying)
                ticker.Stop();
            ticker.Play();

            if(tick % 3 == 0)
            {
                hook.Fire();
            }
        }
    }
}
