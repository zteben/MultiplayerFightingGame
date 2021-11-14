using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Photon.MonoBehaviour
{
    public PhotonView photonView;
    public Rigidbody2D rb;
    public GameObject PlayerCamera;
    public SpriteRenderer sr;
    public float MoveSpeed;

    public GameObject EauObject;
    public Transform FirePos;

    public bool isGrounded;
    public Transform feetPos;
    public float checkRadius;
    public LayerMask whatIsGround;
    public float jumpForce;
    private float jumpTimeCounter;
    public float jumpTime;
    public bool isJumping;

    public bool DisableInput = false;

    private void Awake()
    {
        if (photonView.isMine && !DisableInput)
        {
            PlayerCamera.SetActive(true);
        }
            
    }

    private void Update()
    {
        if (photonView.isMine)
        {
            CheckInput();
        }
    }
    private void CheckInput()
    {
       
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);
        if (isGrounded == true && Input.GetKeyDown(KeyCode.Z))
        {
            isJumping = true;
            jumpTimeCounter = jumpTime;
            rb.velocity = Vector2.up * jumpForce;
        }
        if (Input.GetKey(KeyCode.Z) && isJumping == true)
        {
            if (jumpTimeCounter > 0)
            {
                rb.velocity = Vector2.up * jumpForce;
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }
        if (Input.GetKeyUp(KeyCode.Z))
        {
            isJumping = false;
        }
       
        var move = new Vector3(Input.GetAxisRaw("Horizontal"), 0);
        transform.position += move * MoveSpeed * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.X))
        {
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            photonView.RPC("FlipTrue", PhotonTargets.AllBuffered);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            photonView.RPC("FlipFalse", PhotonTargets.AllBuffered);
        }
    }


    private void Shoot()
    {
        if (sr.flipX == false)
        {
            GameObject obj = PhotonNetwork.Instantiate(EauObject.name, new Vector2(FirePos.transform.position.x, FirePos.transform.position.y), Quaternion.identity, 0);
        }

        if (sr.flipX == true)
        {
            GameObject obj = PhotonNetwork.Instantiate(EauObject.name, new Vector2(FirePos.transform.position.x, FirePos.transform.position.y), Quaternion.identity, 0);
            obj.GetComponent<PhotonView>().RPC("ChangeDir_left", PhotonTargets.AllBuffered);
        } 
        
    }


    [PunRPC] 
    private void FlipTrue()
    {
        sr.flipX = true;
    }
    [PunRPC]
    private void FlipFalse()
    {
        sr.flipX = false;
    }


}
