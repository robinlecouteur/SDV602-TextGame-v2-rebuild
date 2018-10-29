
/// <summary>
/// Base class for a Command
/// </summary>
public class Command
{// : MonoBehaviour {

    public static string[] Tokens;
    public static int CurrentToken;


    /// <summary>
    /// Change the current token up one so the next command processes the next token
    /// </summary>
    protected static void Consume()
    {
        if ((CurrentToken + 1) < Tokens.Length)
            CurrentToken++;
    }

    /// <summary>
    /// Is called to run the command. Command subclasses override this with 
    /// </summary>
    /// <param name="aCmd"></param>
    public virtual void Do(CommandMap aCmd)
    {
        Consume();
    }


}




/// <summary>
/// Class for a Go Command. If successful it moves a player to a new area
/// </summary>
public class GoCommand : Command
{
    /// <summary>
    /// Override for the Do function
    /// </summary>
    /// <param name="prCmd"></param>
    public override void Do(CommandMap prCmd)
    {
        base.Do(prCmd); // consume
        string lcDirection = Command.Tokens[Command.CurrentToken];

        GameManager.DebugLog("Got a Go" + lcDirection);

        Area lcArea = GameModel.CurrentPlayer.CurrentArea; // Shortcut to the current area
        string lcCurrentCnvName = GameManager.Instance.CurrentCnv.name; // Shortcut to the canvas that the player is currently on
        if (lcCurrentCnvName == "CnvGame") // Check that the current canvas is the game canvas
        {
            if (GameModel.CurrentPlayer.Move(lcDirection) == true) // try to move
            {
                prCmd.Result = GameModel.CurrentPlayer.CurrentArea.AreaText + "\n";
                prCmd.Result += GameModel.CurrentPlayer.CurrentArea.ItemsText;
                prCmd.Result += GameModel.CurrentPlayer.CurrentArea.DestinationText;
            }
            else
            {
                prCmd.Result = "Not a valid direction!";
            }


            
        }
        else
            prCmd.Result = "Not able to go places when in " + lcCurrentCnvName;

    }
}

/// <summary>
/// Class for a pick command. If successful it adds an item to the users inventory and removes the item from the area it was in
/// </summary>
public class PickCommand : Command
{
    /// <summary>
    /// Override for the Do function
    /// </summary>
    /// <param name="prCmd"></param>
    public override void Do(CommandMap prCmd)
    {
        base.Do(prCmd); // consume
        GameManager.DebugLog("Got a Pick" + Command.Tokens[CurrentToken]);
        if (Command.Tokens[CurrentToken].ToLower() == "up")
            Command.Consume();

        Area lcArea = GameModel.CurrentPlayer.CurrentArea; // Shortcut to the current area
        string lcCurrentCnvName = GameManager.Instance.CurrentCnv.name; // Shortcut to the canvas that the player is currently on
        if (lcCurrentCnvName == "CnvGame") // Check that the current canvas is the game canvas
        {

            foreach (Item prItem in lcArea.LstItems)
            {
                if (Command.Tokens[CurrentToken] == prItem.ItemName)
                {
                    GameModel.CurrentPlayer.PickUpItem(prItem);
                    prCmd.Result = prItem.TextOnPickup;
                    break;
                }
                else
                {
                    prCmd.Result = "Could not pick up that item!";
                }
            }
        }
        else
            prCmd.Result = "Not able to go places when in " + lcCurrentCnvName;
    }
}

/// <summary>
/// Class for a show command. If successful it switches to the specified canvas
/// </summary>
public class ShowCommand : Command
{
    /// <summary>
    /// Override for the Do function
    /// </summary>
    /// <param name="prCmd"></param>
    public override void Do(CommandMap prCmd)
    {
        base.Do(prCmd); // consume

        string lcResult = "";

        GameManager.DebugLog("Got a Show" + Command.Tokens[Command.CurrentToken]);
        switch (Command.Tokens[Command.CurrentToken])
        {
            case "game":
                GameManager.Instance.SwitchToCanvas(GameManager.Instance.CnvGame);

                break;
            case "map":
                GameManager.Instance.SwitchToCanvas(GameManager.Instance.CnvMap);
                break;
            case "inventory":
                GameManager.Instance.SwitchToCanvas(GameManager.Instance.CnvInventory);
                break;
            default:
                lcResult = "Do not understand. Did you mean \"show game\", \"show map\", or \"show inventory\"?";
                break;
        }
        prCmd.Result = lcResult;
    }
}

/// <summary>
/// Class for a Attack command. If successful it attacks and damages the specified boss
/// </summary>
public class AttackCommand : Command
{
    /// <summary>
    /// Override for the Do function
    /// </summary>
    /// <param name="prCmd"></param>
    public override void Do(CommandMap prCmd)
    {
        base.Do(prCmd); // consume

        string lcResult = "";

        GameManager.DebugLog("Got an Attack" + Command.Tokens[Command.CurrentToken]);

        Area lcArea = GameModel.CurrentPlayer.CurrentArea; // Shortcut to the current area
        string lcCurrentCnvName = GameManager.Instance.CurrentCnv.name; // Shortcut to the canvas that the player is currently on
        if (lcCurrentCnvName == "CnvGame") // Check that the current canvas is the game canvas
        {
            if (lcArea.Boss != null)
            {
                if (lcArea.Boss.AreaName == lcArea.AreaName)
                {
                    lcResult = GameModel.CurrentPlayer.Attack(lcArea.Boss);
                    GameModel.DB.SetPlayer(GameModel.CurrentPlayer);
                    GameModel.DB.SetArea(lcArea);
                }
            }
            else
                lcResult = "No boss to attack";
        }
        else
            lcResult = "Not able to go places when in " + lcCurrentCnvName;


        prCmd.Result = lcResult;
    }
}

/// <summary>
/// Class for a use command. If successful it uses the effect of an item
/// </summary>
public class UseCommand : Command
{
    /// <summary>
    /// Override for the Do function
    /// </summary>
    /// <param name="prCmd"></param>
    public override void Do(CommandMap prCmd)
    {
        base.Do(prCmd); // consume


        GameManager.DebugLog("Got a Use" + Command.Tokens[Command.CurrentToken]);

        string lcCurrentCnvName = GameManager.Instance.CurrentCnv.name; // Shortcut to the canvas that the player is currently on
        if (lcCurrentCnvName == "CnvGame") // Check that the current canvas is the game canvas
        {

            foreach (Item prItem in GameModel.CurrentPlayer.LstInventory)
            {
                if (prItem.ItemName == Tokens[CurrentToken])
                {
                    prCmd.Result = GameModel.CurrentPlayer.Use(prItem);
                    GameModel.DB.SetPlayer(GameModel.CurrentPlayer);
                    
                    break;
                }
            }
        }
        else
            prCmd.Result = "Not able to go places when in " + lcCurrentCnvName;

    }
}

