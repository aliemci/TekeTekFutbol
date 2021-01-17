using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BallPhysics : MonoBehaviourPun, IPunObservable
{
    public float ballSensitivity = 1;
    Vector3 latestPos;
    Quaternion latestRot;
    float currentTime = 0;
    double currentPacketTime = 0;
    double lastPacketTime = 0;
    Vector3 positionAtLastPacket = Vector3.zero;
    Quaternion rotationAtLastPacket = Quaternion.identity;



    void Update()
    {
        if (!photonView.IsMine)
        {
            //Lag compensation
            double timeToReachGoal = currentPacketTime - lastPacketTime;
            currentTime += Time.deltaTime;

            //Update remote player
            transform.position = Vector3.Lerp(positionAtLastPacket, latestPos, (float)(currentTime / timeToReachGoal));
            transform.rotation = Quaternion.Lerp(rotationAtLastPacket, latestRot, (float)(currentTime / timeToReachGoal));
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            //We own this player: send the others our data
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            //Network player, receive data
            latestPos = (Vector3)stream.ReceiveNext();
            latestRot = (Quaternion)stream.ReceiveNext();

            //Lag compensation
            currentTime = 0.0f;
            lastPacketTime = currentPacketTime;
            currentPacketTime = info.SentServerTime;
            positionAtLastPacket = transform.position;
            rotationAtLastPacket = transform.rotation;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        GameObject player = other.gameObject;


        if(player.tag == "Player")
        {
            
            Vector3 ballDirection = this.transform.position - player.transform.position;
            ballDirection = ElementWiseMultiplication(ballDirection, new Vector3(1,0,1));
            this.GetComponent<Rigidbody>().AddForce(ballDirection * ballSensitivity, ForceMode.Impulse);

            Debug.DrawLine(this.transform.position, this.transform.position + ballDirection * ballSensitivity, Color.red, Time.deltaTime);
        }   

    }

    Vector3 ElementWiseMultiplication(Vector3 a, Vector3 b)
    {
        return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
    }
}
