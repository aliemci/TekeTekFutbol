using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BallPhysics : MonoBehaviourPun, IPunObservable
{

    [HideInInspector] public Rigidbody rb;
    public float ballSensitivity = 1;

    [HideInInspector] public Vector3 networkPosition;
    [HideInInspector] public Vector3 forceVector;


    void Awake()
    {
        rb = this.GetComponent<Rigidbody>();

    }

    void Start()
    {
        
    }

    void LateUpdate()
    {
        if (!photonView.IsMine)
        {
            rb.position = Vector3.Lerp(rb.position, networkPosition, Time.deltaTime);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            //We own this player: send the others our data
            stream.SendNext(rb.position);
            stream.SendNext(rb.velocity);
        }
        else
        {
            //Network player, receive data
            networkPosition = (Vector3)stream.ReceiveNext();
            rb.velocity = (Vector3)stream.ReceiveNext();

            float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));

            networkPosition += (rb.velocity * lag);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        GameObject player = other.gameObject;

        if(player.tag == "Player")
        {
            if (PhotonNetwork.IsMasterClient)
            {
                Vector3 ballDirection = this.transform.position - player.transform.position;
                ballDirection = ElementWiseMultiplication(ballDirection, new Vector3(1,0,1));
                forceVector = ballDirection * ballSensitivity;
                Debug.DrawLine(this.transform.position, this.transform.position + ballDirection * ballSensitivity, Color.red, Time.fixedDeltaTime);
                photonView.RPC("OnCollision", RpcTarget.All, forceVector);
                // rb.AddForce(forceVector, ForceMode.Impulse);
                
            }
        }

    }

    [PunRPC]
    public void OnCollision(Vector3 forceVector)
    {
        rb.AddForce(forceVector, ForceMode.Impulse);
        // networkPosition = position;
        // rb.velocity = speed;
    }

    public Vector3 ElementWiseMultiplication(Vector3 a, Vector3 b)
    {
        return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
    }
    
}
