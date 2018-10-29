using System.Linq;

/// <summary>
/// Class for the game model
/// </summary>
public static class GameModel {
    /// <summary>
    /// Constructor. Checks if the database is empty and inserts the defaults if it is empty. Otherwise, it doesn't modify the database
    /// </summary>
    static GameModel()
    {
        if (DBIsEmpty() == true)
        {
            _db.SetDefaults();
        }
        else
        {
            GameManager.DebugLog("Loading existing database: " + _dbFileName);
        }

    }



    #region Database stuff
    private static  string _dbFileName = "TextGame.db";
    private static DataService _db = new DataService(_dbFileName);

    public static DataService DB
    {
        get
        {
            return _db;
        }
    }


    /// <summary>
    /// Checks if the database is empty
    /// </summary>
    /// <returns></returns>
    private static bool DBIsEmpty()
    {
        try
        {
            int lcCount = _db.Connection.Table<Area>().Count<Area>();
            if (lcCount > 0)
            {
                return false;
            }
            else
                return true;
        }
        catch (SQLite4Unity3d.SQLiteException)
        {

            return true;
        }

    }
    #endregion


    #region Login and register + Player management
    private static string _currentPlayerName;

    public static bool LoggedIn = false;
    public static string CurrentPlayerName
    {
        get
        {
            return _currentPlayerName;
        }
        set
        {
            _currentPlayerName = value;
        }
    }
    public static Player CurrentPlayer
    {
        get
        {
            Player lcCurrentPlayer = _db.GetPlayer(_currentPlayerName);
            GameManager.DebugLog("The current player is: " + _currentPlayerName);


            return lcCurrentPlayer;
        }
        set
        {
            Player lcCurrentPlayer = value;
            _db.SetPlayer(lcCurrentPlayer);
        }

    }




    /// <summary>
    /// Takes a username and password, and registers the user, and logs them in
    /// </summary>
    /// <param name="prPlayerName"></param>
    /// <param name="prPassword"></param>
    /// <returns name="lcSuccess"></returns>
    public static string RegisterPlayer(string prPlayerName, string prPassword)
    {
        string lcResult = "Failed";

        if (prPlayerName == "" || prPassword == "")
        {
            lcResult = "Cannot have empty fields!";
        }// Test for null fields
        else if (DB.PlayerExists(prPlayerName) == true)
        {
            lcResult = "A user with this name already exists! \n Please choose another name";
        }// Check if player exists
        else if (DB.PlayerExists(prPlayerName) == false)
        {
            DB.NewPlayer(prPlayerName, prPassword);// Add the new player
            if (LogIn(prPlayerName, prPassword) == "Success")
            {
                lcResult = "Success";
            }// Attempt to log the new player in
            else
                lcResult = "Could not log in new player";
        }//Check if player doesn't exist
        else
        {
            lcResult = "Failed for unknown reason";
        }
            
        return lcResult;
    }


    /// <summary>
    /// Takes a username and password, and logs in that user if successful
    /// </summary>
    /// <param name="prPlayerName"></param>
    /// <param name="prPassword"></param>
    /// <returns name="lcSuccess"></returns>
    public static string LogIn(string prPlayerName, string prPassword)
    {
        string lcResult = "Failed";

        if (prPlayerName == "" || prPassword == "") 
        {
            lcResult = "Cannot have empty fields!";
        }// Check for null fields
        else if (DB.PlayerExists(prPlayerName) == false) 
        {
            lcResult = "Player does not exist";
        }//Check if player doesn't exist
        else if (DB.PlayerExists(prPlayerName) == true)
        {
            if (DB.PlayerPassComboExists(prPlayerName, prPassword) == true)
            {
                Player lcPlayer = _db.GetPlayer(prPlayerName, prPassword);

                _currentPlayerName = lcPlayer.PlayerName; // CurrentPlayer
                lcResult = "Success";
            }// Check if Password is correct
            else
            {
                lcResult = "Please enter a valid password";
            }  
        }// Check if player DOES exist
        else
            lcResult = "Failed for unknown reason";

        return lcResult;
    }
    #endregion

}
