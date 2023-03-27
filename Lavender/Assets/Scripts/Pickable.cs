using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Pickable : MonoBehaviour
{
    GameObject playerInfo;
    public string itemName;
    PlayerActions playerActions;
    GameObject player;
    GameObject message;
    bool onItem = false;
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
        player = GameObject.Find("Player");
        message = GameObject.Find("Message");
        playerInfo = GameObject.Find("PlayerInfo");
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnInteract(InputAction.CallbackContext value)
    {
        if (onItem)
        {
            float angle = Vector3.Angle(player.transform.forward, transform.position - player.transform.position);
            if (angle < 90.0f)
            {
                message.GetComponent<Message>().ShowMessage("You find a " + itemName + "!");
                playerInfo.GetComponentInChildren<Inventory>().AddItem(itemName);
                onItem = false;
                gameObject.SetActive(false);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            onItem = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            onItem = false;
    }
}
