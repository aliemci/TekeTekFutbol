using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Universalholder : MonoBehaviour
{
    
    public static Universalholder instance;

    public GameObject ball;
    public GameObject[] players;


    public void Awake()
    {
        instance = this;
    }


}
