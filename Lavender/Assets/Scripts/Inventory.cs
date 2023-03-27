using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.InputSystem.DefaultInputActions;

[Serializable]
public class InventoryItem
{
    public string name;
    public Sprite sprite;
}

public class Inventory : MonoBehaviour
{
    public delegate void UseDelegate();
    public UseDelegate useMethod;

    GameObject playerInfo;

    Animator fade;
    private PlayerActions playerActions;

    public PlayerActions inventoryActions;
    PlayerActions contextActions;

    public InventoryItem[] available;

    public int health = 3;
    public Sprite lifefull;
    public Sprite lifehalf;
    public Sprite lifelow;
    Image life;
    public InventoryItem[] items = new InventoryItem[8];
    private int occupied = 0;

    private GameObject[] slots = new GameObject[8];
    private Image[] slotImage = new Image[8];
    private GameObject selector;
    private Image showImage;
    private TextMeshProUGUI showText;
    int current = 0;
    Vector2 movement;

    private GameObject contextMenu;
    private GameObject contextSelector;
    int contextOption = 0;

    public bool AddItem(string name)
    {
        if (occupied == 7) return false;
        int index = Array.FindIndex(available, row => row.name == name);
        if (index == -1) return false;
        items[occupied] = available[index];
        occupied++;
        return true;
    }
    public bool KeyItem(string name)
    {
        int index = Array.FindIndex(items, row => {
            if (row != null)
                return row.name == name;
            else
                return false;
            });
        if (index == -1) return false;
        items[index] = null;
        for (int i = index + 1; i < 8; i++)
        {
            items[i - 1] = items[i];
        }
        occupied--;
        return true;
    }
    private void RemoveItem(int index)
    {
        items[index] = null;
        for (int i = index + 1; i < 8; i++)
        {
            items[i - 1] = items[i];
        }
        occupied--;
    }

    private void Awake()
    {
        inventoryActions = new PlayerActions();
        contextActions = new PlayerActions();
    }

    private void OnEnable()
    {
        inventoryActions.Inventory.Move.performed += OnInventoryMove;
        inventoryActions.Inventory.Click.performed += OnInventoryClick;
        inventoryActions.Inventory.Exit.performed += OnInventoryExit;
        contextActions.InventoryContext.Back.performed += OnInventoryContextBack;
        contextActions.InventoryContext.Select.performed += OnInventoryContextSelect;
        contextActions.InventoryContext.Move.performed += OnInventoryContextMove;
    }

    // Start is called before the first frame update
    void Start()
    {
        life = gameObject.transform.GetChild(0).GetChild(0).GetComponentInChildren<Image>();
        playerInfo = transform.parent.gameObject;
        playerActions = GameObject.Find("Player").GetComponent<Character>().playerActions;
        fade = GameObject.Find("Fade").GetComponent<Animator>();
        for (int i = 0; i < 8; i++)
        {
            slots[i] = gameObject.transform.GetChild(i + 1).gameObject;
            slotImage[i] = slots[i].transform.GetChild(0).GetComponent<Image>();
        }
        selector = gameObject.transform.GetChild(9).gameObject;
        GameObject show = gameObject.transform.GetChild(10).gameObject;
        showImage = show.gameObject.transform.GetChild(0).GetComponent<Image>();
        showText = show.gameObject.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();
        contextMenu = gameObject.transform.GetChild(11).gameObject;
        contextSelector = contextMenu.transform.GetChild(2).gameObject;
    }

    private void OnInventoryContextBack(InputAction.CallbackContext context)
    {
        inventoryActions.Enable();
        contextActions.Disable();
        contextMenu.GetComponent<Animator>().SetTrigger("Hide");
    }
    private void OnInventoryContextSelect(InputAction.CallbackContext context)
    {
        switch (contextOption)
        {
            case 0:
                switch (items[current].name)
                {
                    case "Painkiller":
                        if (health < 3)
                        {
                            health++;
                            RemoveItem(current);
                        }    
                        break;
                }
                break;
            case 1:
                RemoveItem(current);
                break;
        }
        inventoryActions.Enable();
        contextActions.Disable();
        contextMenu.GetComponent<Animator>().SetTrigger("Hide");
    }
    private void OnInventoryContextMove(InputAction.CallbackContext context)
    {
        float v = context.ReadValue<float>();
        if (v > 0)
            if (contextOption > 0)
                contextOption--;
        if (v < 0)
            if (contextOption < 1)
                contextOption++;
    }

    private void OnInventoryClick(InputAction.CallbackContext context)
    {
        if (items[current] == null)
            return;
        contextActions.Enable();
        inventoryActions.Disable();
        contextMenu.GetComponent<Animator>().SetTrigger("Show");
        contextMenu.transform.position = selector.transform.position + new Vector3(-40.0f, 0.0f, 0.0f);
        contextOption = 0;
    }
    private void OnInventoryExit(InputAction.CallbackContext context)
    {
        StartCoroutine(InventoryCoroutine());
    }
    IEnumerator InventoryCoroutine()
    {
        fade.SetTrigger("Fade");
        yield return new WaitForSeconds(0.4f);
        inventoryActions.Disable();
        playerActions.Enable();
        playerInfo.GetComponent<Canvas>().enabled = false;
    }
    private void OnInventoryMove(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();

        if (movement.x != 0 && movement.y != 0)
            return;

        if (movement.x > 0)
        {
            if (current % 2 == 0)
            {
                current += 1;
                selector.transform.position = slots[current].transform.position;
            }
        }
        else if (movement.x < 0)
        {
            if (current % 2 == 1)
            {
                current -= 1;
                selector.transform.position = slots[current].transform.position;
            }
        }
        else if (movement.y < 0)
        {
            if (current + 2 < slots.Length)
            {
                current += 2;
                selector.transform.position = slots[current].transform.position;
            }
        }
        else if (movement.y > 0)
        {
            if (current - 2 >= 0)
            {
                current -= 2;
                selector.transform.position = slots[current].transform.position;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < 8; i++)
        {
            if (items[i] != null && items[i].name != "")
            {
                slotImage[i].sprite = items[i].sprite;
                slotImage[i].color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            }
            else
            {
                slotImage[i].sprite = null;
                slotImage[i].color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
            }
        }
        if (items[current] != null)
        {
            showImage.sprite = slotImage[current].sprite;
            showImage.color = slotImage[current].color;
            showText.text = items[current].name;
        }
        else
        {
            showImage.sprite = null;
            showImage.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
            showText.text = "";
        }

        contextSelector.transform.position = contextMenu.transform.GetChild(contextOption).transform.position;
        switch (health)
        {
            case 1:
                life.sprite = lifelow;
                break;
            case 2:
                life.sprite = lifehalf;
                break;
            case 3:
                life.sprite = lifefull;
                break;
        }
    }

}
