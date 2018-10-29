using System.Collections.Generic;
using SQLite4Unity3d;

/// <summary>
/// Class to define what an Area contains and can do
/// </summary>
public class Area {

    //Colums to be stored in the database
    [PrimaryKey, AutoIncrement] public int ID { get; set; }
    [NotNull] public string AreaName { get; set; }
    [NotNull] public string Description { get; set; }
    public string AreaText { get; set; }
    public int TimesVisited { get; set; }
    //--- --- --- --- --- --- --- --- --- ---




    //Ignored by the database
    private Dictionary<string, string> _dictAreaText;
    private List<AreaDirection> _lstDirections;
    private List<Item> _lstItems;
    private AreaLogic _areaLogic = new AreaLogic();
    private Boss _boss;
    [Ignore] public List<Item> LstItems
    {
        get
        {
            return _lstItems;
        }

        set
        {
            _lstItems = value;
        }
    }
    [Ignore] public Dictionary<string, string> DictAreaText // <string LABEL, string TEXT>
    {
        get
        {
            return _dictAreaText;
        }

        set
        {
            _dictAreaText = value;
        }
    }
    [Ignore] public List<AreaDirection> Directions
    {
        get
        {
            return _lstDirections;
        }

        set
        {
            _lstDirections = value;
        }
    }
    [Ignore] public AreaLogic AreaLogic
    {
        get
        {
            return _areaLogic;
        }

        set
        {
            _areaLogic = value;
        }
    }
    [Ignore] public string DestinationText
    {
        get
        {
            string lcDestText = "\n \n This is where you can go from here: \n";
            foreach (AreaDirection lcDirection in _lstDirections)
            {
                lcDestText += "   " + lcDirection.Direction + "  " + lcDirection.ToAreaName + "\n";
            }

            return lcDestText;
        }
    }
    [Ignore] public string ItemsText
    {
        get
        {
            int lcCounter = 0;
            string lcItemsText = "";
            foreach (Item lcItem in _lstItems)
            {
                lcCounter += 1;
            }
            if (lcCounter > 0)
            {
                lcItemsText = "You see the following items on the ground:" + "\n";
                {
                    foreach (Item lcItem in _lstItems)
                    {
                        lcItemsText += "   " + lcItem.ItemName + "\n";
                    }

                }
            }
            return lcItemsText;
        }

    }
    [Ignore] public Boss Boss
    {
        get
        {
            return _boss;
        }

        set
        {
            _boss = value;
        }
    }

    public void Arrive()
    {
        AreaLogic.Arrive();
    }

    public void Leave()
    {
        AreaLogic.Leave();
    }
    //--- --- --- --- --- --- --- --- --- ---
}
