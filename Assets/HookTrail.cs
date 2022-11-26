using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookTrail : MonoBehaviour
{
    public Hook hook;
    private Material material;

    private void OnEnable()
    {
        material = GetComponent<MeshRenderer>().material;
    }

    void Update()
    {
        if (hook.Firing)
        {
            var from = transform.parent.position + hook.holdCenter;
            var to = hook.transform.position;
            var dir = (to - from);
            dir.z = 0.0f;
            dir = dir.normalized;

            float length = Vector2.Distance(from, to);

            transform.rotation = Quaternion.Euler(0.0f, 0.0f, Mathf.Atan2(dir.y, dir.x) * 180.0f / Mathf.PI + 90);
            transform.localScale = new Vector3(0.25f, length, 1.0f);
            transform.position = (from + to) * 0.5f;

            material.mainTextureScale = new Vector2(1.0f, length / 0.25f);
        }
        else
        {
            transform.localScale = Vector3.zero;
        }
    }
}
