﻿using UnityEngine;

// Code initially based on code from here:
// https://answers.unity.com/questions/155907/basic-movement-walking-on-walls.html

[RequireComponent(typeof(Collider))]
public class AlienMovement : MonoBehaviour
{
    public float mosueSensitivity = 10;
    public float movementSpeed = 6;
    // Turn speed is in degrees per second.
    public float turnSpeed = 90; 
    // Smoothing speed.
    public float lerpSpeed = 10;
    public float gravity = 10;
    // Is the alien in contact with the ground.
    public bool isGrounded;
    // The initial vertical speed of a jump.
    public float jumpSpeed = 10;
    // The range at which to detect a wall to stick to.
    public float jumpRange = 10;

    // The normal of the current surface.
    private Vector3 surfaceNormal;
    // The characters normal.
    private Vector3 charNormal;
    // The distance of the Alien from the ground.
    private float distGround;
    // Flag for if the alien is currently jumping.
    private bool jumping;
    // Current vertical speed.
    private float verticalSpeed;
    private Collider charCollider;
    private Rigidbody charRigidbody;

    void Start()
    {
        // Gets the character collider and rigidbody.
        charCollider = this.GetComponent<Collider>();
        charRigidbody = this.GetComponent<Rigidbody>();
        // Initialises the charNormal to the world normal.
        charNormal = transform.up;
        // Gets the height from the centre of the collider to the ground.
        distGround = charCollider.bounds.extents.y - charCollider.bounds.center.y;
    }

    void FixedUpdate()
    {
        // Calculate and apply force of gravity to char.
        Vector3 gravForce = -gravity * charRigidbody.mass * charNormal;
        charRigidbody.AddForce(gravForce);
    }

    void Update()
    {   
        // Exits Update if the character is mid-jump.
        if (jumping)
        {
            return;
        }

        // When the jump key is pressed activate either a normal jump or a jump to a wall.
        if (Input.GetButtonDown("Jump"))
        {
            Ray ray;
            RaycastHit hit;

            // Creates a ray from the current position in the direction the char is facing.
            ray = new Ray(transform.position, transform.forward);

            // If there is a wall ahead then trigger JumpToWall script.
            if (Physics.Raycast(ray, out hit, jumpRange))
            {
                //JumpToWall(hit.point, hit.normal);
            }
            // If the player is on the ground then jump up.
            else if (isGrounded)
            {
                charRigidbody.velocity += jumpSpeed * charNormal;
            }
        }
        
        // Gets mouse x and y movement.
        float mouseX = Input.GetAxis("Mouse X") * mosueSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mosueSensitivity;

        // Rotates the player model in the x-axis.
        transform.Rotate(new Vector3(0.0f, mouseX, 0.0f));

        // Rotates the camera so it tracks the mouse.
        Camera camera = this.GetComponentInChildren<Camera>();

        camera.transform.Rotate(new Vector3(-mouseY, 0.0f, 0.0f));



    }
}
