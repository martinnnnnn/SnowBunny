using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector3 cameraOffset;
    public Transform target;
    public float lerpSpeed;
    public float errorRate = 0.02f;

    bool follow = false;

    private void Start()
    {
        transform.position = target.position;
        transform.rotation = target.rotation;
        cameraOffset = Camera.main.transform.position - transform.position;
        Camera.main.depthTextureMode = DepthTextureMode.Depth;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == target.gameObject)
        {
            follow = true;
        }
    }

    void FixedUpdate()
    {
        if (follow)
        {
            if (Vector3.Distance(target.position, transform.position) < errorRate)
            {
                follow = false;
            }
            else
            {
                Camera.main.transform.position += (target.position + cameraOffset - Camera.main.transform.position) * lerpSpeed;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 1, 1, 0.3f);
        Gizmos.DrawSphere(transform.position, errorRate);
    }
}
