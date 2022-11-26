using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class trophy : MonoBehaviour
{
    Vector3 pos;
    public CanvasGroup fade;
    bool animationPlaying = false;
    float t = 0.0f;
    public Transform player;

    // Start is called before the first frame update
    void Start()
    {
        pos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(player.position, transform.position) < 0.5f)
            animationPlaying = true;

        if(animationPlaying)
        {
            t += Time.deltaTime * 2.0f;

            if(t > 2.0f)
            {
                SceneManager.LoadScene(2);
            }
        }

        fade.alpha = t;

        transform.position = pos + Vector3.up * Mathf.Sin(Time.time * 2.0f) * 0.25f;
    }
}
