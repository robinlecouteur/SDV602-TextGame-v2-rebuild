using SQLite4Unity3d;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class AreaLogic
{
    //Tables --- --- --- --- --- --- --- --- --- --- --- ---
    [PrimaryKey, AutoIncrement] public int ID { get; set; }
    [NotNull] public string AreaName { get; set; }



    //--- --- --- --- --- --- --- --- --- --- --- --- --- --

    //Ignored by SQLite
    private Area _area;
    [Ignore] public Area Area
    {
        get
        {
            return _area;
        }

        set
        {
            _area = value;
        }
    }
    //----------------




    //Logic --- --- --- --- -- --- --- --- --- --- --- --- -
    public void Arrive()
    {
        if (_area.AreaName == "TownCenter") { TownCenterArriveLogic(); }
        if (_area.AreaName == "Tavern") { TavernArriveLogic(); }
        if (_area.AreaName == "FootOfMountain") { FootOfMountainArriveLogic(); }
    }



    public void Leave()
    {
        _area.TimesVisited += 1;
        if (_area.AreaName == "Tavern") { TavernLeaveLogic(); }
    }


    //--- --- --- --- --- --- --- --- --- --- --- --- --- --


    private void TownCenterArriveLogic()
    {

    }



    private void TavernArriveLogic()
    {
        if (_area.TimesVisited > 0)
        {
            _area.AreaText = _area.DictAreaText["ReturningText"];
        }
    }
    
    private void TavernLeaveLogic()
    {
        bool lcGoldcoinPickedUp = false;
        foreach (Item lcItem in _area.LstItems)
        {
            if (lcItem.ItemName == "goldcoin")
            {
                lcGoldcoinPickedUp = false;
                break;
            }
            else
            {
                lcGoldcoinPickedUp = true;
            }
        }
        if (lcGoldcoinPickedUp == true || _area.LstItems.Count == 0)
        {
            Boss lcBigbird = GameModel.DB.GetBoss("bigbird");
            lcBigbird.AreaName = "FootOfMountain";
            GameModel.DB.SetBoss(lcBigbird);
            
        }
    }



    private void FootOfMountainArriveLogic()
    {

        Boss lcBigbird = GameModel.DB.GetBoss("bigbird");
        if (lcBigbird.AreaName == "FootOfMountain")
        {
            _area.AreaText = _area.DictAreaText["BossText"];

        } 
    }
}

