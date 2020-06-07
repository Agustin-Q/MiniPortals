using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{

    CharacterController characterController;
    public float speed = 1f;
    private float speedDivider = 1f;
    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        if (characterController == null) Debug.LogError("Objet must not be null", characterController);
    }

    // Update is called once per frame
    void Update()
    {
        float movementX = Input.GetAxis("Horizontal");
        float movementZ = Input.GetAxis("Vertical");

        Vector3 move = new Vector3();
        move = transform.right * movementX + transform.forward * movementZ;
        move.Normalize();
        move = move * (speed/speedDivider) * Time.deltaTime;
        characterController.Move(move);

    }
}
