using SQLite4Unity3d;
using UnityEngine;
using System.Linq;
using System;

#if !UNITY_EDITOR
using System.Collections;
using System.IO;
#endif
using System.Collections.Generic;

public class DataService {
    //Connection
    private SQLiteConnection _connection;
    public SQLiteConnection Connection
    {
        get
        {
            return _connection;
        }
    }
    //----------

    
    /// <summary>
    /// Constructor. Creates the database in the specified location, or uses an existing database in the specified location
    /// </summary>
    /// <param name="prDatabaseName"></param>
    public DataService(string prDatabaseName) {

#if UNITY_EDITOR
        var lcDbPath = string.Format(@"Assets/StreamingAssets/{0}", prDatabaseName);
#else
        // check if file exists in Application.persistentDataPath
        var filepath = string.Format("{0}/{1}", Application.persistentDataPath, DatabaseName);

        if (!File.Exists(filepath))
        {
            Debug.Log("Database not in Persistent path");
            // if it doesn't ->
            // open StreamingAssets directory and load the db ->

#if UNITY_ANDROID
            var loadDb = new WWW("jar:file://" + Application.dataPath + "!/assets/" + DatabaseName);  // this is the path to your StreamingAssets in android
            while (!loadDb.isDone) { }  // CAREFUL here, for safety reasons you shouldn't let this while loop unattended, place a timer and error check
            // then save to Application.persistentDataPath
            File.WriteAllBytes(filepath, loadDb.bytes);
#elif UNITY_IOS
                 var loadDb = Application.dataPath + "/Raw/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);
#elif UNITY_WP8
                var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);

#elif UNITY_WINRT
		var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
		// then save to Application.persistentDataPath
		File.Copy(loadDb, filepath);
		
#elif UNITY_STANDALONE_OSX
		var loadDb = Application.dataPath + "/Resources/Data/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
		// then save to Application.persistentDataPath
		File.Copy(loadDb, filepath);
#else
	var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
	// then save to Application.persistentDataPath
	File.Copy(loadDb, filepath);

#endif

            Debug.Log("Database written");
        }

        var dbPath = filepath;
#endif
        _connection = new SQLiteConnection(lcDbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
        Debug.Log("Final PATH: " + lcDbPath);
    }




    /// <summary>
    /// Creates all the tables in the database and inserts the initial game data into them
    /// </summary>
    public void SetDefaults()
    {
        #region Drop and create tables
        _connection.DropTable<Player>();
        _connection.DropTable<Area>();
        _connection.DropTable<AreaDirection>();
        _connection.DropTable<AreaItem>();
        _connection.DropTable<AreaText>();
        _connection.DropTable<AreaLogic>();
        _connection.DropTable<Item>();
        _connection.DropTable<InventoryItem>();
        _connection.DropTable<Boss>();

        _connection.CreateTable<Player>();
        _connection.CreateTable<Area>();
        _connection.CreateTable<AreaDirection>();
        _connection.CreateTable<AreaItem>();
        _connection.CreateTable<AreaText>();
        _connection.CreateTable<AreaLogic>();
        _connection.CreateTable<Item>();
        _connection.CreateTable<InventoryItem>();
        _connection.CreateTable<Boss>();
        #endregion

        #region Insert into tables
        _connection.InsertAll(new Player[]{
            new Player{
                PlayerName = "Robin",
                Password = "Pa$$w0rd",
                CurrentAreaName = "TownCenter",
                HPUpperLimit = 100,
                HPCurrent = 100,
                AtkDamage = 30,
                XP = 0,
            },
            new Player{
                PlayerName = "Player1",
                Password = "Pa$$w0rd",
                CurrentAreaName = "TownCenter",
                HPUpperLimit = 100,
                HPCurrent = 100,
                AtkDamage = 30,
                XP = 0,
            },

        });

        _connection.InsertAll(new AreaDirection[]{
            new AreaDirection{ FromAreaName = "TownCenter",     ToAreaName = "Tavern",          Direction = "south" },
            new AreaDirection{ FromAreaName = "Tavern",         ToAreaName = "TownCenter",      Direction = "north" },
            new AreaDirection{ FromAreaName = "Tavern",         ToAreaName = "FootOfMountain",  Direction = "south" },
            new AreaDirection{ FromAreaName = "FootOfMountain", ToAreaName = "Tavern",          Direction = "north" },
        });

        _connection.InsertAll(new AreaText[]{
            new AreaText{ AreaName = "TownCenter",      Label = "DefaultText",      Text = "You are in the Town Center" },
            new AreaText{ AreaName = "Tavern",          Label = "DefaultText",      Text = "You arrive at the Tavern" },
            new AreaText{ AreaName = "Tavern",          Label = "ReturningText",    Text = "You return to the Tavern" },
            new AreaText{ AreaName = "FootOfMountain",  Label = "DefaultText",      Text = "You are at the foot of the mountain" },
            new AreaText{ AreaName = "FootOfMountain",  Label = "BossText",         Text = "The aura grows stronger in waves, then out of a dark corner Bigbird aproaches you with an evil glint in his eye" }
        });

        _connection.InsertAll(new Item[]{
            new Item{ItemName = "goldcoin", Description = "A shiny gold coin",                  TextOnPickup = "You quickly pocket the coin of precious gold. \n Suddenly you feel a menacing aura from the south" },
            new Item{ItemName = "sword",    Description = "A blade of steel",                   TextOnPickup = "You grab the sword and admire the shiny steel" },
            new Item{ItemName = "shield",   Description = "A sturdy wooden shield",             TextOnPickup = "You pick up the shield as a defense against danger" },
            new Item{ItemName = "potion",   Description = "A single vial of healing potion",    TextOnPickup = "You put the vial of blue liquid in your pocket. Careful, there is only enough for one swig!", HealAmount = 30}
        });

        _connection.InsertAll(new AreaItem[]{
            new AreaItem{ItemName = "goldcoin", AreaName = "Tavern"},
            new AreaItem{ItemName = "sword",    AreaName = "Tavern"},
            new AreaItem{ItemName = "shield",   AreaName = "TownCenter"},
            new AreaItem{ItemName = "potion",   AreaName = "TownCenter"},
        });

        _connection.InsertAll(new AreaLogic[]{
            new AreaLogic{AreaName = "TownCenter"},
            new AreaLogic{AreaName = "Tavern"},
            new AreaLogic{AreaName = "FootOfMountain"}
        });

        _connection.InsertAll(new Area[]{
            new Area{ AreaName = "TownCenter",      Description = "The Town Center",            AreaText = "You are in the Town Center"},
            new Area{ AreaName = "Tavern",          Description = "The Tavern",                 AreaText = "You arrive at the Tavern"},
            new Area{ AreaName = "FootOfMountain",  Description = "The Foot of the Mountain",   AreaText = "You are at the foot of the mountain"}

        });

        _connection.InsertAll(new Boss[]{
            new Boss{ AtkDamage = 40, HPUpperLimit = 100, HPCurrent = 100, Name = "bigbird"}

        });

        #endregion
    }


    //Boss --- --- --- --- --- --- ---
    /// <summary>
    /// Gets a boss by name
    /// </summary>
    /// <param name="prBossName"></param>
    /// <returns></returns>
    public Boss GetBoss(string prBossName)
    {
        //Retrieve Boss
        Boss lcBoss = _connection.Table<Boss>().Where<Boss>(x => x.Name == prBossName).First<Boss>();

        return lcBoss;
    }

    /// <summary>
    /// Sets a boss
    /// </summary>
    /// <param name="prBoss"></param>
    public void SetBoss(Boss prBoss)
    {
        //Set Boss
        _connection.Update(prBoss);
    }  
    //--- --- --- --- --- --- --- ---



    //Item --- --- --- --- --- --- ---
    /// <summary>
    /// Takes a string for an item name, then returns an item from the database with that name
    /// </summary>
    /// <param name="prItemName"></param>
    /// <returns name="lcItem"></returns>
    public Item GetItem(string prItemName)
    {
        //Retrieve item
        Item lcItem = _connection.Table<Item>().Where<Item>(x => x.ItemName == prItemName).First<Item>();

        return lcItem;
    }
    //--- --- --- --- --- --- --- ---



    //Area --- --- --- --- --- --- ---
    /// <summary>
    /// Takes a string for an area name, then returns an area from the database with that name
    /// </summary>
    /// <param name="prAreaName"></param>
    /// <returns name="lcArea"></returns>
    public Area GetArea(string prAreaName)
    {
        //Retrieve area
        Area lcArea = _connection.Table<Area>().Where<Area>(x => x.AreaName == prAreaName).First<Area>();

        //Retrieve directions 
        lcArea.Directions = _connection.Table<AreaDirection>().Where<AreaDirection>(x => x.FromAreaName == prAreaName).ToList<AreaDirection>();

        //Retrieve items
        lcArea.LstItems = new List<Item>();
        foreach (AreaItem lcAreaItem in _connection.Table<AreaItem>().Where<AreaItem>(x => x.AreaName == prAreaName).ToList<AreaItem>())
        {
            lcArea.LstItems.Add(GetItem(lcAreaItem.ItemName));
        }

        //Retrieve area text
        lcArea.DictAreaText = new Dictionary<string, string>();
        foreach (AreaText lcAreaText in _connection.Table<AreaText>().Where<AreaText>(x => x.AreaName == prAreaName).ToList<AreaText>())
        {
            lcArea.DictAreaText.Add(lcAreaText.Label, lcAreaText.Text);
        }

        //Retrieve area logic
        lcArea.AreaLogic = _connection.Table<AreaLogic>().Where<AreaLogic>(x => x.AreaName == prAreaName).First();
        lcArea.AreaLogic.Area = lcArea;

        //Retrieve area boss
        foreach (Boss lcBoss in _connection.Table<Boss>().Where<Boss>(x => x.AreaName == prAreaName).ToList())
        {
            lcArea.Boss = lcBoss;
        }
        

        return lcArea;
    }

    /// <summary>
    /// Takes an Area and updates it in the database
    /// </summary>
    /// <param name="prArea"></param>
    public void SetArea(Area prArea)
    {
        //Set area
        _connection.Update(prArea);

        //Set items - Deletes all items related to this area, and just inserts items back that still exist in the area.
        foreach (AreaItem lcAreaItem in _connection.Table<AreaItem>().Where<AreaItem>(x => x.AreaName == prArea.AreaName).ToList<AreaItem>())
        {
            _connection.Delete<AreaItem>(lcAreaItem.ID);
        }//Delete all the items related to the area

        List<AreaItem> lcAreaItems = new List<AreaItem>();

        foreach (Item lcItem in prArea.LstItems)
        {
            AreaItem lcAreaItemToAdd = new AreaItem() { AreaName = prArea.AreaName, ItemName = lcItem.ItemName };
            lcAreaItems.Add(lcAreaItemToAdd);
        }

        //Set area boss
        if (prArea.Boss != null)
            _connection.Update(prArea.Boss);

        
        _connection.InsertAll(lcAreaItems);
    }
    //--- --- --- --- --- --- --- ---



    //Player
    /// <summary>
    /// Takes a string for an player name, then returns an player from the database with that name
    /// </summary>
    /// <param name="prPlayerName"></param>
    /// <returns name="lcPlayer"></returns>
    public Player GetPlayer(string prPlayerName)
    {
        //Retrieve the player
        Player lcPlayer;
        lcPlayer = _connection.Table<Player>().
            Where<Player>(x => x.PlayerName == prPlayerName).First<Player>();

        //Retrieve the player's inventory
        lcPlayer.LstInventory = new List<Item>();
        foreach (InventoryItem lcInventoryItem in _connection.Table<InventoryItem>().Where<InventoryItem>(x => x.PlayerName == prPlayerName).ToList<InventoryItem>())
        {
            lcPlayer.LstInventory.Add(GetItem(lcInventoryItem.ItemName));
        }

        //Retrieve the player's current area
        lcPlayer.CurrentArea = GetArea(lcPlayer.CurrentAreaName);
            
        return lcPlayer;
    }

    /// <summary>
    /// Takes strings for a player's name and password, then returns an player from the database with that name and password
    /// </summary>
    /// <param name="prPlayerName"></param>
    /// <param name="prPassword"></param>
    /// <returns></returns>
    public Player GetPlayer(string prPlayerName, string prPassword)
    {
        Player lcPlayer;
        //Retrieve the player
        lcPlayer = _connection.Table<Player>().Where<Player>(
                            x => x.PlayerName == prPlayerName && x.Password == prPassword
                       ).First<Player>();

        //Retrieve the player's inventory
        lcPlayer.LstInventory = new List<Item>();
        foreach (InventoryItem lcInventoryItem in _connection.Table<InventoryItem>().Where<InventoryItem>(x => x.PlayerName == prPlayerName).ToList<InventoryItem>())
        {
            lcPlayer.LstInventory.Add(GetItem(lcInventoryItem.ItemName));
        }

        //Retrieve the player's current area
        lcPlayer.CurrentArea = GetArea(lcPlayer.CurrentAreaName);

        return lcPlayer;
    }

    /// <summary>
    /// Takes a Player and updates it in the database
    /// </summary>
    /// <param name="prArea"></param>
    public void SetPlayer(Player prPlayer)
    {
        //Set the player's current area
        prPlayer.CurrentAreaName = prPlayer.CurrentArea.AreaName;
        SetArea(prPlayer.CurrentArea);

        //Set the player
        _connection.Update(prPlayer);

        //Set the player's inventory
        foreach (InventoryItem lcInventoryItem in _connection.Table<InventoryItem>().Where<InventoryItem>(x => x.PlayerName == prPlayer.PlayerName).ToList<InventoryItem>())
        {
            _connection.Delete<InventoryItem>(lcInventoryItem.ID);
        }//Delete all the items related to the player
        List<InventoryItem> lcInventoryItems = new List<InventoryItem>();
        foreach (Item lcItem in prPlayer.LstInventory)
        {
            InventoryItem lcInventoryItemToAdd = new InventoryItem() { PlayerName = prPlayer.PlayerName, ItemName = lcItem.ItemName };
            lcInventoryItems.Add(lcInventoryItemToAdd);
        }
        _connection.InsertAll(lcInventoryItems);


        //Set the player's 
    }


    /// <summary>
    /// Checks if the specified player exists in the database
    /// </summary>
    /// <param name="prPlayerName"></param>
    /// <returns></returns>
    public bool PlayerExists(string prPlayerName)
    {
        return (_connection.Table<Player>().Where<Player>(
                          x => x.PlayerName == prPlayerName
                     ).ToList<Player>().Count > 0);
    }

    /// <summary>
    /// Checks if the username/password combo exists
    /// </summary>
    /// <param name="prPlayerName"></param>
    /// <param name="prPassword"></param>
    /// <returns></returns>
    public bool PlayerPassComboExists(string prPlayerName, string prPassword)
    {
        return (_connection.Table<Player>().Where<Player>(x => 
                        x.PlayerName == prPlayerName &&
                        x.Password == prPassword
                     ).ToList<Player>().Count > 0);
    }

    /// <summary>
    /// Creates a player with the specified username/password and sets the default player data
    /// </summary>
    /// <param name="prPlayerName"></param>
    /// <param name="prPassword"></param>
    public void NewPlayer(string prPlayerName, string prPassword)
    {
        Player newPlayer = new Player()
        {
            PlayerName = prPlayerName,
            Password = prPassword,
            CurrentAreaName = "TownCenter",
            HPUpperLimit = 100,
            HPCurrent = 100,
            AtkDamage = 30,
            XP = 0,
        };

        _connection.Insert(newPlayer);
    }
    //--- --- --- --- --- --- --- ---
}
