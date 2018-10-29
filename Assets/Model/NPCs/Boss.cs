using SQLite4Unity3d;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/// <summary>
/// Class to define what characteristics a boss will have, and what it can do such as attack
/// </summary>
public class Boss
{
    [PrimaryKey, AutoIncrement] public int ID { get; set; }
    public string Name { get; set; }
    public string AreaName { get; set; }
    public int HPUpperLimit { get; set; }
    public int HPCurrent { get; set; }
    public int AtkDamage { get; set; }

    /// <summary>
    /// Attacks the specified player
    /// </summary>
    /// <param name="prPlayer"></param>
    public void Attack(Player prPlayer)
    {
        int lcAtkDamage = AtkDamage;
        foreach (Item lcItem in prPlayer.LstInventory)
        {
            lcAtkDamage -= lcItem.DefenseBonus;
        }
        prPlayer.HPCurrent -= lcAtkDamage;

        GameModel.DB.SetPlayer(prPlayer);
    }
}
