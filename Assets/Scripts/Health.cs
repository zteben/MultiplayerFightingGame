using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : Photon.MonoBehaviour
{
    public float HealthAmount;

    public Image FillImage;

    public Player plMove;
    public Rigidbody2D rb;
    public BoxCollider2D bc;
    public SpriteRenderer sr;
    public GameObject PlayerCanvas;

    private void Awake()
    {
        if (photonView.isMine)
        {
            GameManager.Instance.LocalPlayer = this.gameObject;
        }
    }

    [PunRPC] public void ReduceHealth(float amount)
    {
        ModifyHealth(amount);
    }

    
    private void CheckHealth()
    {
        FillImage.fillAmount = HealthAmount / 100f;
        if (photonView.isMine && HealthAmount <= 0)
        {
            if (!photonView.isMine && HealthAmount > 0)
            {
                GameManager.Instance.WinResponse();
            }
            GameManager.Instance.EnableRespawn();
            plMove.DisableInput = true;
            this.GetComponent<PhotonView>().RPC("Dead", PhotonTargets.AllBuffered);
     
        }
        



    }


    public void EnableInput()
    {
        plMove.DisableInput = false;
    }

    [PunRPC] 
    private void Dead()
    {
        rb.gravityScale = 0;
        bc.enabled = false;
        sr.enabled = false;
        PlayerCanvas.SetActive(false);
    }

    [PunRPC]
    private void Respawn()
    {
        rb.gravityScale = 1;
        bc.enabled = true;
        sr.enabled = true;
        PlayerCanvas.SetActive(true);
    }
    
    private void ModifyHealth(float amount)
    {
        if (photonView.isMine)
        {
            HealthAmount -= amount;
            FillImage.fillAmount -= amount/100f;
        }
        else
        {
            HealthAmount -= amount;
            FillImage.fillAmount -= amount/100f;
            if (HealthAmount <= 0)
            {
                GameManager.Instance.WinResponse();
            }

        }

        CheckHealth();

    }

}
