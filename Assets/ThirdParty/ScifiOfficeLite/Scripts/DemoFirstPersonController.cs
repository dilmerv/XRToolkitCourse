using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScifiOffice {
    public class DemoFirstPersonController : MonoBehaviour {

        Rigidbody rb;
        CapsuleCollider col;
        bool isCrouching;

        public Transform playerBody;

        public enum ControlType { android, keyboard, keyboardMouse };
        public ControlType controlType;

        [Header("Movement")]
        public float speed = 3f;
        public float accelerationRate = 12f, crouchFactor = 0.5f, decelerationFactor = 1f;
        public float mouseSensitivity = 50f;

        float xRot = 0f;
        float horizontalMovement;
        float verticalMovement;

        [Header("HUD")]
        public GameObject canvas;      


        private void Start() {
            rb = playerBody.GetComponent<Rigidbody>();
            col = playerBody.GetComponent<CapsuleCollider>();
            
            if(controlType == ControlType.keyboardMouse)
                Cursor.lockState = CursorLockMode.Locked;
        }

        // Update is called once per frame
        void Update() {
            
            Walk();
            Look();

            //E to switch keyboard control type between keyboardMouse and keyboard
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (controlType == ControlType.keyboardMouse)
                {
                    controlType = ControlType.keyboard;
                    xRot = 0f;
                }
                else
                {
                    controlType = ControlType.keyboardMouse;
                }
            }
            else if (controlType == ControlType.android)
            {
                //Show mobile controls
                canvas.SetActive(true);
            }
            else
            {
                //Do not show mobile controls when using keyboard controls
                Crouch();
                canvas.SetActive(false);
            }



        }

        public void Look() {
            float mouseX = 0;
            float mouseY = 0;

            switch(controlType) {
                case ControlType.android:
                    mouseX = horizontalMovement * Time.deltaTime * mouseSensitivity;
                    break;

                case ControlType.keyboard:
                    //Get changes to look left and right only. Player cannot look up and down.
                    mouseX = Input.GetAxis("Horizontal") * mouseSensitivity * Time.deltaTime;
                    mouseY = 0;
                    break;

                default:
                case ControlType.keyboardMouse:
                    //Use mouse to control where to look. Can look in all directions.
                    mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
                    mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
                    break;
            }

            //rotate playerBody
            xRot -= mouseY;
            xRot = Mathf.Clamp(xRot, -90f, 90f);

            transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);
            playerBody.Rotate(Vector3.up * mouseX);
        }

        void Walk() {
            Vector3 displacement;
            float maxSpeed = speed, maxAcc = accelerationRate;

            // Lower the limits if we are crouching.
            if (isCrouching) {
                maxSpeed *= crouchFactor;
                maxAcc *= crouchFactor;
            }

            //Find displacement based on controlType.
            switch(controlType) {
                case ControlType.android:
                    //Move forward and back only. Horizontal turns.
                    displacement = playerBody.transform.forward * verticalMovement;
                    break;

                case ControlType.keyboard:
                    //Only can move forward and back
                    displacement = playerBody.transform.forward * Input.GetAxis("Vertical");
                    break;

                case ControlType.keyboardMouse:
                default:
                    //Move in 4 directions, this is the default control
                    displacement = playerBody.transform.forward * Input.GetAxis("Vertical") + playerBody.transform.right * Input.GetAxis("Horizontal");
                    break;
            }

            float len = displacement.magnitude;
            if(len > 0) {
                rb.velocity += displacement / len * Time.deltaTime * maxAcc;

                // Clamp velocity to the maximum speed.
                if(rb.velocity.magnitude > maxSpeed) {
                    rb.velocity = rb.velocity.normalized * speed;
                }
            } else {
                // If no buttons are pressed, decelerate.
                len = rb.velocity.magnitude;
                float decelRate = accelerationRate * decelerationFactor * Time.deltaTime;
                if(len < decelRate) rb.velocity = Vector3.zero;
                else {
                    rb.velocity -= rb.velocity.normalized * decelRate;
                }
            }
        }

        void Crouch() {
            //Crouch when the couch key is being pressed
            if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.LeftShift)) {
                col.height = .5f;
                isCrouching = true; 
            } else {
                //Otherwise, player stop crouching
                col.height = 2;
                //if (Input.GetKey(KeyCode.LeftShift)) {
                //    isCrouching = true;
                //    return;
                //}
                isCrouching = false;
            }
        }

        //crouching for android build
        public void MobileCrouch()
        {
            //If player is currently crouching, stop crouching and vice versa
            if(isCrouching)
            {
                col.height = 2;
                isCrouching = false;
            }
            else
            {
                col.height = .5f;
                isCrouching = true;
            }
        }

        //setting movement for android build
        public void MobileWalk(int direction)
        {
            
            if(direction * direction == 1)
            {
                //Moving left and right
                horizontalMovement = direction;
            }
            else if(direction == 3)
            {
                //When none of the button is pressed, stop moving
                horizontalMovement = 0;
                verticalMovement = 0;
            }
            else
            {
                //Moving forward and back
                verticalMovement = direction - 1;
            }
            
            
        }


    }
}
