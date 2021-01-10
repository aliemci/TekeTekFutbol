using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Com.MyCompany.MyGame
{
    
    public class CharacterMovement : MonoBehaviourPun
    {

        #region Public Fields

        public float speed = 1f;
        public float jumpPower = 10f;

        public bool isGrounded;

        #endregion


        #region Private Fields

        const float gravity = 9.8f;
        Rigidbody characterRigidbody;
        Vector3 movementVector = Vector3.zero;
        Vector3 jumpVector = Vector3.zero;

        #endregion


        #region MonoBehaviour Callbacks


        void Start()
        {
            characterRigidbody = this.GetComponent<Rigidbody>();
        }

        void Update()
        {
            if(!photonView.IsMine && PhotonNetwork.IsConnected)
                return;
            
            movementVector = Vector3.ClampMagnitude(new Vector3(Input.GetAxis("Vertical"), 0f, -Input.GetAxis("Horizontal")), 1f);

            characterRigidbody.MovePosition(this.transform.position + movementVector * speed * Time.deltaTime);

            isGrounded = IsGrounded();

            if(Input.GetKeyDown(KeyCode.Space))
            {
                if(isGrounded)
                {
                    characterRigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
                }
            }

            // jumpVector.y -= gravity * Time.deltaTime;

            // jumpVector = new Vector3(jumpVector.x, Mathf.Clamp(jumpVector.y, 0, Mathf.Infinity), jumpVector.z);

            // characterRigidbody.MovePosition(jumpVector * Time.deltaTime);

        }

        bool IsGrounded(){

            CapsuleCollider capsuleCollider = this.gameObject.GetComponent<CapsuleCollider>();

            RaycastHit hit;

            Debug.DrawLine(this.transform.position - (Vector3.up/2), this.transform.position - Vector3.up, Color.red, 1f);

            if(Physics.Raycast(this.transform.position - (Vector3.up/2), -Vector3.up, out hit, 1f))
            {
                Debug.DrawLine(this.transform.position - (Vector3.up/2), hit.point, Color.blue, 1f);
                if (hit.distance < 1f)
                {
                    return true;
                }
            }

            return false;
        }

        #endregion


    }




}

