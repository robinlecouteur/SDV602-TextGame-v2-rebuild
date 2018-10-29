using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SQLite4Unity3d;

/// <summary>
/// Class to define what an Item looks like in the database
/// </summary>
public class Item {
    [PrimaryKey, AutoIncrement] public int ID { get; set; }
    [NotNull] public string ItemName { get; set; }
    [NotNull] public string Description { get; set; }

    public int DefenseBonus { get; set; }
    public int AttackBonus { get; set; }
    public int HealAmount { get; set; } 
    public string TextOnPickup { get; set; }
}
