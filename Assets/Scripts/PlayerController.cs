using System;
using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEditor.Tilemaps;
using UnityEngine;

public enum Speeds { }
public enum GamePhase { Jump = 0, Fly = 1 }
public class PlayerController : MonoBehaviour
{

    [Header("JUMP PHASE VARIABLES")]
    public bool wasOnGround;
    public bool jumped;
    public float roadSpeed;
    public float jumpPower;
    public bool coyoteStarted;
    public float coyoteTime;
    public bool coyoteEnabled;
    public float distanceRequiredToJump = 0.6f;
    public float jumpGravityScale = 12.0461f;
    public GameObject particleEffect;
    public GameObject groundTouchGameObject;
    [Header("FLY PHASE VARIABLES")]
    public float flyGravityScale = 2.93f;
    public float flyYVelocityLimit = 9.93f;
    [Header("GENERAL VARIABLES")]
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] Rigidbody2D rigidbody;
    [SerializeField] private Transform spriteTransform;
    public static PlayerController Instance;
    public GamePhase gamePhase;
    public Action ControlInputs;

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            ChangeGameMode(gamePhase);
        }
    }
    void Update()
    {
        ControlInputs.Invoke();
        transform.position += Vector3.right * Time.deltaTime * roadSpeed;
        //move right
    }
    private void FlyPhaseControls()
    {
        rigidbody.gravityScale = (Input.GetMouseButton(0)) ? -flyGravityScale : flyGravityScale;
        transform.localEulerAngles = new Vector3(0, rigidbody.velocity.y, rigidbody.velocity.y * 2);
        LimitYVelocity(flyYVelocityLimit);
    }
    private void JumpPhaseControls()
    {
        if (isOnGround())
        {
            if (particleEffect.activeInHierarchy == false)
            {
                particleEffect.SetActive(true);
            }
            particleEffect.SetActive(true);

            Vector3 currentRotation = spriteTransform.localEulerAngles;
            currentRotation.z = Mathf.Round(currentRotation.z / 90f) * 90;
            spriteTransform.localEulerAngles = new Vector3(0, 0, currentRotation.z);
            if (Input.GetMouseButtonDown(0))
            {
                Jump();
            }
            else
            {
                wasOnGround = true;
            }
        }
        else
        {
            if (wasOnGround == true && jumped == false)
            {
                wasOnGround = false;
                jumped = false;
                StartCoroutine(CloseCoyoteAfterDelay(coyoteTime));
            }
            else if (Input.GetMouseButtonDown(0) && coyoteEnabled)
            {
                Jump();
            }
            spriteTransform.Rotate(Vector3.back, 452.4152186f * Time.deltaTime);
            LimitYVelocity(24.2f);
            particleEffect.SetActive(false);
        }
    }
    private void Jump()
    {
        PlayJumpSound();
        rigidbody.velocity = Vector3.zero;
        rigidbody.AddForce(Vector3.up * jumpPower, ForceMode2D.Impulse);
        coyoteEnabled = false;
        wasOnGround = false;
        jumped = true;
    }
    private void PlayJumpSound()
    {
        if (GameManager.Instance.playSound == true)
        {
            GameObject sound = new GameObject("sound");
            sound.AddComponent<AudioSource>();
            sound.GetComponent<AudioSource>().volume = 1;
            sound.GetComponent<AudioSource>().PlayOneShot(GameManager.Instance.jumpSound);
            Destroy(sound, GameManager.Instance.jumpSound.length);
        }
    }
    private bool isOnGround()
    {
        return Physics2D.OverlapBox(groundTouchGameObject.transform.position,new Vector2(1f,distanceRequiredToJump),0,groundLayerMask);

    }
    private IEnumerator CloseCoyoteAfterDelay(float coyoteTime)
    {
        coyoteEnabled = true;
        yield return new WaitForSeconds(coyoteTime);
        coyoteEnabled = false;
    }

    public void ChangeGameMode(GamePhase _gamePhase)
    {
        if (_gamePhase == GamePhase.Jump)
        {
            ControlInputs = JumpPhaseControls;
            rigidbody.gravityScale = jumpGravityScale;
        }
        if (_gamePhase == GamePhase.Fly)
        {
            ControlInputs = FlyPhaseControls;
            rigidbody.gravityScale = -flyGravityScale;

        }
        gamePhase = _gamePhase;
    }
    public void LimitYVelocity(float yLimit)
    {
        if (Mathf.Abs(rigidbody.velocity.y) >= yLimit)
        {
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, yLimit * Mathf.Sign(rigidbody.velocity.y));
        }
    }
}
