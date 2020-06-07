using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewController : MonoBehaviour
{

    public float mouseSensitivity = 100f;
    private Transform playerTransform;
    private Transform cameraTransform;
    private float yRotation = 0f;
    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GetComponent<Transform>();
        if (playerTransform == null) Debug.LogError("Object must not be null", playerTransform);
        cameraTransform = gameObject.transform.Find("LookCamera").transform;
        if (cameraTransform == null) Debug.LogError("Object must not be null", cameraTransform);
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float movementX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float movementY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        yRotation -= movementY;

        cameraTransform.localRotation = Quaternion.Euler(yRotation, 0, 0);
        playerTransform.Rotate(Vector3.up, movementX);
    }
}
