using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Com.MyCompany.MyGame
{
    
    [RequireComponent(typeof(CharacterController))]
    public class SimpleCharacterMovement : MonoBehaviourPun
    {

        #region Public Fields

        public float speed = 1f;
        public float jumpPower = 10f;

        #endregion


        #region Private Fields

        const float gravity = 9.8f;
        CharacterController characterController;
        Vector3 movementVector = Vector3.zero;
        Vector3 jumpVector = Vector3.zero;

        #endregion


        #region MonoBehaviour Callbacks


        void Start()
        {
            characterController = this.GetComponent<CharacterController>();

        }

        void Update()
        {
            if(!photonView.IsMine && PhotonNetwork.IsConnected)
                return;
            
            Vector3 movementVector = Vector3.ClampMagnitude(new Vector3(Input.GetAxis("Vertical"), 0f, -Input.GetAxis("Horizontal")), 1f);

            characterController.SimpleMove(movementVector * speed * Time.deltaTime);

            if(Input.GetKeyDown(KeyCode.Space))
            {
                if(characterController.isGrounded)
                {
                    jumpVector.y = jumpPower;
                }
            }

            jumpVector.y -= gravity * Time.deltaTime;

            characterController.Move(jumpVector * Time.deltaTime);

        }

        #endregion


    }




}

