using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace Com.MyCompany.MyGame
{

    public class PlayerManager : MonoBehaviourPunCallbacks
    {
        #region Public Fields
        public static GameObject localPlayerInstance;
        [SerializeField] public GameObject playerUiPrefab;


        #endregion


        #region Private Fields



        #endregion

        #region Private Methods

        #if UNITY_5_4_OR_NEWER
        void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode loadingMode)
        {
            this.CalledOnLevelWasLoaded(scene.buildIndex);
        }
        #endif

        #endregion

        #region MonoBehaviour CallBacks


        void Awake()
        {
            if(photonView.IsMine)
                PlayerManager.localPlayerInstance = this.gameObject;
    
            DontDestroyOnLoad(this.gameObject);
    
        }


        void Start()
        {
            if(playerUiPrefab != null)
            {
                GameObject _uiGO = Instantiate(playerUiPrefab);
                _uiGO.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);

            }
            else
            {
                Debug.LogWarning("<Color=Red><a>Missing</a></Color> PlayerUiPrefab reference on player Prefab.", this);
            }

            #if UNITY_5_4_OR_NEWER
            // Unity 5.4 has a new scene management. register a method to call CalledOnLevelWasLoaded.
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
            #endif
        }


        void Update()
        {

            
            
        }


        #if !UNITY_5_4_OR_NEWER
        /// <summary>See CalledOnLevelWasLoaded. Outdated in Unity 5.4.</summary>
        void OnLevelWasLoaded(int level)
        {
            this.CalledOnLevelWasLoaded(level);
        }
        #endif


        void CalledOnLevelWasLoaded(int level)
        {
            // check if we are outside the Arena and if it's the case, spawn around the center of the arena in a safe zone
            if (!Physics.Raycast(transform.position, -Vector3.up, 5f))
            {
                transform.position = new Vector3(0f, 5f, 0f);
            }

            GameObject _uiGo = Instantiate(this.playerUiPrefab);
            _uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
        }

        #if UNITY_5_4_OR_NEWER
        public override void OnDisable()
        {
            // Always call the base to remove callbacks
            base.OnDisable ();
            UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
        }
        #endif

        #endregion

        #region MonoBehaviourPunCallbacks



        #endregion

        
    }
}
