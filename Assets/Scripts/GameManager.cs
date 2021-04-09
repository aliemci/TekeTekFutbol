using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

namespace Com.MyCompany.MyGame
{
    public class GameManager : MonoBehaviourPunCallbacks
    {

        #region Public Fields

        public static GameManager instance;
        public GameObject playerPrefab;
        public List<Transform> spawnPoint;

        #endregion


       #region Photon Callbacks


        public override void OnPlayerEnteredRoom(Player other)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                LoadArena();
            }
        }


        public override void OnPlayerLeftRoom(Player other)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                LoadArena();
            }
        }


        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(0);
        }


        #endregion
       

       #region Public Methods


        public void Start()
        {
            instance = this;

            int index = PhotonNetwork.LocalPlayer.ActorNumber - 1;

            if(PlayerManager.localPlayerInstance == null)
            {
                PhotonNetwork.Instantiate(this.playerPrefab.name, spawnPoint[index].position, Quaternion.identity, 0);
            }

            if(PhotonNetwork.IsMasterClient)
                PhotonNetwork.Instantiate("Ball", Vector3.zero, Quaternion.identity);

        }

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.LoadLevel("Launcher");
        }


        #endregion


        #region Private Methods


        void LoadArena()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }

            PhotonNetwork.LoadLevel("Room for " + 1);
        }


        #endregion
        
    }
    
}
