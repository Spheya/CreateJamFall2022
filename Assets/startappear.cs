using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startappear : MonoBehaviour
{
    // Start is called before the first frame update
    float t = 0.0f;
    public AudioSource src;
    bool played = false;
    Vector3 pos;

    void Start()
    {
        pos = transform.position;
        transform.position = Vector3.one * 10000.0f;
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;
        if(t > 0.25f && !played)
        {
            transform.position = pos;
            played = true;
            src.Play();
        }
    }
}
