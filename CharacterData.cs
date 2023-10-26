using UnityEngine;
using System.Collections;

[System.Serializable]
public class CharacterData
{
    public Floor currentFloor;
    public Point point;
    public float health;
    public float maxHealth;
    public float originalATK;
    public float atk;
    public float defense;
}
