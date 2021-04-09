using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;


namespace Com.MyCompany.MyGame
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
    
        #region Private Serializable Fields

        [SerializeField] private byte maxPlayerPerRoom = 2;
        [SerializeField] private GameObject controlPanel;
        [SerializeField] private Text progressLabel;


        #endregion


        #region Private Fields


        /// <summary> This client's version number. Users are separated from each other by gameVersion (which allows you to make breaking changes). </summary>
        string gameVersion = "1";

        bool isConnecting;


        #endregion



        #region MonoBehaviour CallBacks


        void Awake()
        {
            // #Critical
            // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
            PhotonNetwork.AutomaticallySyncScene = true;
         
            Application.targetFrameRate = 60;
        }


        void Start()
        {
            progressLabel.gameObject.SetActive(true);
            controlPanel.SetActive(true);
        }

        void Update()
        {
            progressLabel.text = PhotonNetwork.LevelLoadingProgress.ToString();

        }

        #endregion


        #region Public Methods


        /// <summary>
        /// Start the connection process.
        /// - If already connected, we attempt joining a random room
        /// - if not yet connected, Connect this application instance to Photon Cloud Network
        /// </summary>
        public void Connect()
        {
            progressLabel.gameObject.SetActive(true);
            controlPanel.SetActive(false);


            // we check if we are connected or not, we join if we are , else we initiate the connection to the server.
            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.JoinOrCreateRoom("Room 0", new RoomOptions(){IsOpen=true, IsVisible=true, MaxPlayers=maxPlayerPerRoom}, TypedLobby.Default);

                // #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
                // PhotonNetwork.CreateRoom("Room 0", new RoomOptions(){IsOpen=true, IsVisible=true, MaxPlayers=maxPlayerPerRoom});
            }
            else
            {
                // #Critical, we must first and foremost connect to Photon Online Server.
                isConnecting = PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = gameVersion;
            }
        }

        public void Set60FPS()
        {
            Application.targetFrameRate = 60;
        }

        #endregion


        #region MonoBehaviourPunCallbacks Callbacks

        public override void OnConnectedToMaster()
        {
            Debug.Log("Connected To Master!");

            if(isConnecting)
            {
                
                PhotonNetwork.JoinOrCreateRoom("Room 0", new RoomOptions(){IsOpen=true, IsVisible=true, MaxPlayers=maxPlayerPerRoom}, TypedLobby.Default);
                // PhotonNetwork.JoinRoom("Room 0");

                isConnecting = false;
            }

        }

        // public override void OnJoinRandomFailed(short returnCode, string message)
        // {
        //     Debug.Log("Random join failed!");
        //     PhotonNetwork.CreateRoom(null, new RoomOptions{MaxPlayers = maxPlayerPerRoom});
        // }

        public override void OnJoinedRoom()
        {
            Debug.Log("Joined Room!");
            
            if(PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                Debug.Log("We load the \"Room for 1\" ");

                PhotonNetwork.LoadLevel("Room for 1");
            }
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            progressLabel.gameObject.SetActive(false);
            controlPanel.SetActive(true);

            isConnecting = false;


            Debug.Log("Disconnected!");
        }

        #endregion


    }


}

