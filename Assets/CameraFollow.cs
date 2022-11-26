using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float strength = 1.0f;

    private void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(player.position.x, player.position.y, transform.position.z), Time.deltaTime * strength);
    }
}
