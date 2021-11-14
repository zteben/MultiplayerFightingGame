using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
    public GameObject PlayerPrefab;
    public GameObject GameCanvas;
    public GameObject DeathCanvas;
    public GameObject SceneCamera;

    [HideInInspector] public GameObject LocalPlayer;
    public GameObject RespawnMenuL;
    public GameObject RespawnW;
    private float TimerAmount = 5f;
    private bool RunSpawnTimer = false;
    

    private void Awake()
    {
        Instance = this;
        GameCanvas.SetActive(true);
    }

    public void Update()
    {
        if (RunSpawnTimer)
        {
            StartRespawn();
        }
    }
    public void EnableRespawn()
    {
        TimerAmount = 5f;
        RunSpawnTimer = true;
        RespawnMenuL.SetActive(true);
    }
    

    public void WinResponse()
    {
        TimerAmount = 5f;
        RunSpawnTimer = true;
        RespawnW.SetActive(true);
    }
    public void StartRespawn()
    {
        TimerAmount -= Time.deltaTime;

        if (TimerAmount <= 0)
        {
            PhotonNetwork.LoadLevel("Menu");
            PhotonNetwork.LeaveRoom();
  //          LocalPlayer.GetComponent<PhotonView>().RPC("Respawn", PhotonTargets.AllBuffered);
  //          LocalPlayer.GetComponent<Health>().EnableInput();

//           RespawnMenu.SetActive(false);
 //           RunSpawnTimer = false;
        }
    }


    public void SpawnPlayer()
    {
        float randomValue = Random.Range(-1f, 1f);

        PhotonNetwork.Instantiate(PlayerPrefab.name, new Vector2(this.transform.position.x * randomValue, this.transform.position.y), Quaternion.identity, 0);
        GameCanvas.SetActive(false);
        SceneCamera.SetActive(false);
    }
}
