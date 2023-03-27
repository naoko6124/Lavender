using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Door : MonoBehaviour
{
    GameObject sibling;
    public string keyItem = "None";
    public string lockedMessage = "This door is locked!";
    Animator fade;
    Inventory playerInfo;
    GameObject message;
    PlayerActions playerActions;
    GameObject target;
    GameObject player;
    AudioSource effectFail;
    AudioSource effectSuccess;
    bool onDoor = false;
    public bool locked = false;
    void Awake()
    {
        playerActions = new PlayerActions();
    }
    void OnEnable()
    {
        playerActions.Enable();
        playerActions.Movement.Interact.performed += OnInteract;
    }
    void OnDisable()
    {
        playerActions.Disable();
    }
    // Start is called before the first frame update
    void Start()
    {
        int siblingIndex = transform.GetSiblingIndex() == 1 ? 0 : 1;
        sibling = transform.parent.GetChild(siblingIndex).gameObject;
        fade = GameObject.Find("Fade").GetComponent<Animator>();
        player = GameObject.Find("Player");
        target = gameObject.transform.GetChild(0).gameObject;
        message = GameObject.Find("Message");
        playerInfo = GameObject.Find("PlayerInfo").GetComponentInChildren<Inventory>();
        playerInfo.KeyItem("Key");
        effectFail = GameObject.Find("LockedDoorSoundEffect").GetComponent<AudioSource>();
        effectSuccess = GameObject.Find("DoorSoundEffect").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnInteract(InputAction.CallbackContext value)
    {
        if (onDoor)
        {
            float angle = Vector3.Angle(player.transform.forward, target.transform.position - player.transform.position);
            if (angle < 90.0f)
            {
                if (locked)
                {
                    if (keyItem == "Side" || playerInfo.KeyItem(keyItem))
                    {
                        effectSuccess.Play();
                        StartCoroutine(DoorCoroutine());
                        locked = false;
                        sibling.GetComponent<Door>().locked = false;
                        message.GetComponent<Message>().ShowMessage("You unlocked the door!");
                    }
                    else
                    {
                        effectFail.Play();
                        message.GetComponent<Message>().ShowMessage(lockedMessage);
                    }
                }
                else
                {
                    effectSuccess.Play();
                    StartCoroutine(DoorCoroutine());
                }
            }
        }
    }

    IEnumerator DoorCoroutine()
    {
        fade.SetTrigger("Fade");
        yield return new WaitForSeconds(0.4f);
        player.transform.position = target.transform.position;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            onDoor = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            onDoor = false;
    }
}
