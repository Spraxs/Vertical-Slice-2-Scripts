using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField]
    private Vector2 minimumBoundary = new Vector2(-3.04f, -1.96f);
    [SerializeField]
    private Vector2 maximumBoundary = new Vector2(0.87f, 0.96f);
    private GameObject playerObject;

    // Start is called before the first frame update
    void Start()
    {
        playerObject = FindObjectOfType<PlayerDeath>().gameObject;
    }

    void FixedUpdate()
    {

        if (playerObject == null) return;
        transform.position = new Vector3
        (
            Mathf.Clamp(playerObject.transform.position.x, minimumBoundary.x, maximumBoundary.x),
            Mathf.Clamp(playerObject.transform.position.y, minimumBoundary.y, maximumBoundary.y),
            transform.position.z
        );
    }
}
