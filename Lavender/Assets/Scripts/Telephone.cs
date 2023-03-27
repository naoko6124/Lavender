using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Telephone : MonoBehaviour
{
    GameObject playerInfo;
    PlayerActions playerActions;
    GameObject player;
    GameObject message;
    bool onTelephone = false;
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

        Inventory inv = playerInfo.GetComponentInChildren<Inventory>();
        SaveData sd = SaveSystem.Load(0);
        player.transform.position = new Vector3(sd.position[0], sd.position[1], sd.position[2]);
        for (int i = 0; i < sd.inventory.Length; i++)
        {
            if (sd.inventory[i] != -1)
                inv.items[i] = inv.available[sd.inventory[i]];
            else
                inv.items[i] = null;
        }
        inv.health = sd.health;
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnInteract(InputAction.CallbackContext value)
    {
        if (onTelephone)
        {
            float angle = Vector3.Angle(player.transform.forward, transform.position - player.transform.position);
            if (angle < 90.0f)
            {
                message.GetComponent<Message>().ShowMessage("Game saved!");
                Inventory inv = playerInfo.GetComponentInChildren<Inventory>();
                int[] items = new int[inv.items.Length];
                for (int i = 0; i < inv.items.Length; i++)
                {
                    if (inv.items[i] != null)
                    {
                        int index = Array.FindIndex(inv.available, row => { return row.name == inv.items[i].name; });
                        items[i] = index;
                    }
                    else
                        items[i] = -1;
                }
                float[] position = new float[3];
                position[0] = player.transform.position.x;
                position[1] = player.transform.position.y;
                position[2] = player.transform.position.z;
                SaveData sd = new SaveData(inv.health, position, items);
                SaveSystem.Save(sd, 0);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            onTelephone = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            onTelephone = false;
    }
}
