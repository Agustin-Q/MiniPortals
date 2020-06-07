using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{

    Rigidbody characterRB;
    public float speed = 1f;
    private float speedDivider = 1f;
    // Start is called before the first frame update
    void Start()
    {
        characterRB = GetComponent<Rigidbody>();
        if (characterRB == null) Debug.LogError("Objet must not be null", characterRB);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float movementX = Input.GetAxis("Horizontal");
        float movementZ = Input.GetAxis("Vertical");

        Vector3 move = new Vector3();
        move = transform.right * movementX + transform.forward * movementZ;
        move.Normalize();
        move = move * (speed/speedDivider);
        Debug.Log(move.ToString());
        HorizontalMove(move);

        if(Input.GetKeyDown(KeyCode.Space)){
            Debug.Log("Jump");
        }

    }

    private void HorizontalMove(Vector3 movement)
    {
        movement.y = characterRB.velocity.y;
        characterRB.velocity = movement;

    }
}
