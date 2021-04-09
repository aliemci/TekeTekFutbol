using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class CharacterMovement : MonoBehaviourPunCallbacks, IPunObservable
{

    #region Public Fields

    public float speed = 1f;
    public float jumpPower = 10f;
    public bool isGrounded;
    public float moveSpeed;

    public GameObject ball;

    #endregion


    #region Private Fields

    float currentTime = 0;
    double currentPacketTime = 0;
    double lastPacketTime = 0;
    const float gravity = 9.8f;
    Vector3 latestPosition;
    Vector3 positionAtLastPacket = Vector3.zero;
    Vector3 movementVector = Vector3.zero;
    Vector3 jumpVector = Vector3.zero;
    Rigidbody characterRigidbody;

    #endregion


    #region MonoBehaviour Callbacks

    void OnGUI()
    {
        GUILayout.Label("Ping: " + PhotonNetwork.GetPing());
        GUILayout.Label("FPS: " + (1/Time.deltaTime));
    }

    void Start()
    {
        characterRigidbody = this.GetComponent<Rigidbody>();
    }

    void Update()
    {
        if(!photonView.IsMine && PhotonNetwork.IsConnected)
        {
            double timeToReachGoal = currentPacketTime - lastPacketTime;
            currentTime += Time.deltaTime;

            transform.position = Vector3.Lerp(positionAtLastPacket, latestPosition, (float)(currentTime / timeToReachGoal));

            return;
        }

        movementVector = Vector3.ClampMagnitude(new Vector3(Input.GetAxis("Vertical"), 0f, -Input.GetAxis("Horizontal")), 1f);

        moveSpeed = Vector3.Magnitude(movementVector * speed * Time.deltaTime);
        
    }

    void FixedUpdate()
    {
        // movementVector = Vector3.ClampMagnitude(new Vector3(Input.GetAxis("Vertical"), 0f, -Input.GetAxis("Horizontal")), 1f);

        // print("deltatime:" + Time.smoothDeltaTime);

        // moveSpeed = Vector3.Magnitude(movementVector * speed * Time.deltaTime);

        characterRigidbody.MovePosition(this.transform.position + movementVector * speed * Time.deltaTime);


        isGrounded = IsGrounded();

        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(isGrounded)
            {
                characterRigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            }
        }


    }

    bool IsGrounded(){

        CapsuleCollider capsuleCollider = this.gameObject.GetComponent<CapsuleCollider>();

        RaycastHit hit;

        // Debug.DrawLine(this.transform.position - (Vector3.up/2), this.transform.position - Vector3.up, Color.red, 1f);

        if(Physics.Raycast(this.transform.position - (Vector3.up/2), -Vector3.up, out hit, 1f))
        {
            // Debug.DrawLine(this.transform.position - (Vector3.up/2), hit.point, Color.blue, 1f);
            if (hit.distance < 1f)
            {
                return true;
            }
        }

        return false;
    }

    #endregion


    #region IPunObservable

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(transform.position);
        }
        else
        {
            latestPosition = (Vector3)stream.ReceiveNext();

            currentTime = 0.0f;
            lastPacketTime = currentPacketTime;
            currentPacketTime = info.SentServerTime;
            positionAtLastPacket = transform.position;
        }
    }

    #endregion




}
