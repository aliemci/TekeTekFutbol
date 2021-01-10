using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPhysics : MonoBehaviour
{
    public float ballSensitivity = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
