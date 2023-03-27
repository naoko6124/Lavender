using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public int health;
    public float[] position;
    public int[] inventory;

    public SaveData(int health, float[] position, int[] inventory)
    {
        this.health = health;
        this.position = position;
        this.inventory = inventory;
    }

}
