using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Trapdoor : MonoBehaviour
{
    PlayerActions playerActions;
    GameObject target;
    GameObject player;
    Animator fade;
    AudioSource effect;
    bool onDoor = false;
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
        fade = GameObject.Find("Fade").GetComponent<Animator>();
        target = gameObject.transform.GetChild(0).gameObject;
        player = GameObject.Find("Player");
        effect = GameObject.Find("DoorSoundEffect").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnInteract(InputAction.CallbackContext value)
    {
        if (onDoor)
        {
            effect.Play();
            StartCoroutine(DoorCoroutine());
            onDoor = false;
        }
    }

    IEnumerator DoorCoroutine()
    {
        fade.SetTrigger("Fade");
        yield return new WaitForSeconds(0.3f);
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
