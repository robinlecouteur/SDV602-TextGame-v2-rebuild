using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SQLite4Unity3d;

/// <summary>
/// Defines what a player looks like in the database, and handles logic of a player such as movement and other actions.
/// A player consists of login details as well as ingame player statistics.
/// </summary>
public class Player
{
    //Colums to be stored in the database
    [PrimaryKey, AutoIncrement] public int ID { get; set; }
    public string PlayerName { get; set; }
    public string Password { get; set; }

    public int HPUpperLimit { get; set; }
    public int HPCurrent { get; set; }
    public int AtkDamage { get; set; }
    public int XP { get; set; }
    public string CurrentAreaName { get; set; }
    //--- --- --- --- --- --- --- --- --- ---


    //Ignored by the database
    private Area _currentArea;
    private List<Item> _lstInventory;

    [Ignore] public Area CurrentArea
    {
        get
        {
            return _currentArea;
        }

        set
        {
            _currentArea = value;
        }
    }
    [Ignore] public List<Item> LstInventory
    {
        get
        {
            return _lstInventory;
        }

        set
        {
            _lstInventory = value;
        }
    }
    
    public string InventoryString
    {
        get
        {
            string lcInvString = "\n --------------------------------------------------------------- \n *** " + this.PlayerName + "'s INVENTORY ***" + "\n";
            {
                foreach (Item lcItem in this._lstInventory)
                {

                    lcInvString += "   " + lcItem.ItemName + "\n";
                }

            }
            return lcInvString;
        }
    }
    //--- --- --- --- --- --- --- --- --- ---


    //Behavior
    /// <summary>
    /// Takes a direction and changes the player's current area to the area in that direction relative to the current area
    /// </summary>
    /// <param name="prDirection"></param>
    /// <returns></returns>
    public bool Move(string prDirection)
    {
        if (IsDirectionValid(CurrentArea, prDirection) == true)
        {
            foreach (AreaDirection prAreaDir in CurrentArea.Directions)
            {
                if (prAreaDir.Direction == prDirection)
                {
                    _currentArea.Leave();
                    GameModel.DB.SetArea(CurrentArea);
                    CurrentAreaName = prAreaDir.ToAreaName;
                    _currentArea = GameModel.DB.GetArea(prAreaDir.ToAreaName);
                    _currentArea.Arrive();
                    GameModel.DB.SetPlayer(this);
                }
            }

            
            return true;
        }
        else
        {
            return false;
        }

    }

    /// <summary>
    /// Checks to see if the inputted direction mateches up with a direction of the current area
    /// </summary>
    /// <param name="prArea"></param>
    /// <param name="prDirection"></param>
    /// <returns name="lcIsValid"></returns>
    private bool IsDirectionValid(Area prArea, string prDirection)
    {
        bool lcIsValid = false;
        foreach (AreaDirection prAreaDir in prArea.Directions)
        {
            if (prAreaDir.Direction == prDirection)
            { lcIsValid = true; }
        }

        return lcIsValid;
    }

    /// <summary>
    /// Takes the specified object and places it in the users inventory, then removes it from the area where it came from
    /// </summary>
    /// <param name="prItem"></param>
    public void PickUpItem(Item prItem)
    {
        this.LstInventory.Add(prItem);
        this.CurrentArea.LstItems.RemoveAll(x => x.ID == prItem.ID);
        GameModel.DB.SetPlayer(this);
    }

    /// <summary>
    /// Use the specified item to heal(As of current) the player, then return the results
    /// </summary>
    /// <param name="prItem"></param>
    /// <returns></returns>
    public string Use(Item prItem)
    {
        if (prItem.HealAmount != 0)
        {
            int lcHPBefore = HPCurrent;
            HPCurrent += prItem.HealAmount;

            if (HPCurrent > HPUpperLimit)
            {
                HPCurrent = HPUpperLimit;
            }
            int lcAmountHealed = HPCurrent - lcHPBefore;
            LstInventory.Remove(prItem);
            GameModel.DB.SetPlayer(this);
            return "You used the" + prItem.ItemName + " and regained " + lcAmountHealed + " health!" ;
        }
        else
        {
            return "Could not use that item!";
        }

    }

    /// <summary>
    /// Damage the specified boss based on base damage + bonus from items
    /// </summary>
    /// <param name="prBoss"></param>
    public string Attack(Boss prBoss)
    {
        int lcAtkDamage = AtkDamage;
        foreach (Item lcItem in LstInventory)
        {
            lcAtkDamage += lcItem.AttackBonus;
        }
        prBoss.HPCurrent -= lcAtkDamage;
        if (prBoss.HPCurrent <= 0)
        {
            CurrentArea.AreaText = CurrentArea.DictAreaText["DefaultText"];
            return "You killed the boss! You have " + HPCurrent + " health remaining";
        }
        else
        {
            prBoss.Attack(this);
            if (HPCurrent <= 0)
            {
                return "* * * YOU DIED * * *";
            }
            else
            {
                return "You traded blows with " + prBoss.Name + ". \n You now have " + HPCurrent + " health remaining, and " + prBoss.Name + " has " + prBoss.HPCurrent + " health remaining";
            }
        }
    }
    //--- --- --- --- --- --- --- --- --- ---

}
