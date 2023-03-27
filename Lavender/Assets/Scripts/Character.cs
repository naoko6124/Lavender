using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.DefaultInputActions;
using static UnityEngine.GraphicsBuffer;

public class Character : MonoBehaviour
{
    GameObject playerInfo;
    public PlayerActions playerActions;
    Animator animator;
    Animator fade;
    AudioSource effect;
    Vector2 move;

    void Awake()
    {
        playerActions = new PlayerActions();
    }
    void OnEnable()
    {
        playerActions.Enable();
        playerActions.Movement.InventoryOpen.performed += OnInventoryOpen;
    }
    void OnDisable()
    {
        playerActions.Disable();
    }
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        effect = GameObject.Find("WalkSoundEffect").GetComponent<AudioSource>();
        fade = GameObject.Find("Fade").GetComponent<Animator>();
        playerInfo = GameObject.Find("PlayerInfo");
    }

    // Update is called once per frame
    void Update()
    {
        move = playerActions.Movement.Walk.ReadValue<Vector2>();

        animator.SetBool("WalkingForward", (move.y > 0 || move.x != 0));
        animator.SetBool("WalkingBackward", move.y < 0);

        if (move.x == 0 && move.y == 0)
            effect.Stop();
        else
        {
            if (!effect.isPlaying) effect.Play();
        }

        transform.Translate(Vector3.forward * Time.deltaTime * move.y);
        transform.eulerAngles += new Vector3(0.0f, move.x * 90.0f * Time.deltaTime, 0.0f);
    }

    private void OnInventoryOpen(InputAction.CallbackContext context)
    {
        StartCoroutine(InventoryCoroutine());
    }
    IEnumerator InventoryCoroutine()
    {
        fade.SetTrigger("Fade");
        yield return new WaitForSeconds(0.4f);
        playerInfo.GetComponent<Canvas>().enabled = true;
        playerInfo.GetComponentInChildren<Inventory>().inventoryActions.Enable();
        playerActions.Disable();
    }
}
