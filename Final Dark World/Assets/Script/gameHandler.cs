using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameHandler : MonoBehaviour
{

    public CameraFollow cameraFollow;
    public Transform playerTransform;
    private float zoom = 60f;
    void Start()
    {
        cameraFollow.Setup(() => playerTransform.position, () => zoom);
    }

   
}
