using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class for the GameManager
/// </summary>
public class GameManager : MonoBehaviour {

    public static GameManager Instance;


    #region Canvases (Screens)
    /*     Displays are expressed as Canvases
     */

    public Canvas CurrentCnv;
    public Canvas CnvLoginScreen;
    public Canvas CnvGame;
    public Canvas CnvMap;
    public Canvas CnvInventory;
    public Canvas CnvTabs;

    public List<Canvas> Canvases = new List<Canvas>();
    #endregion


    #region Text inputs and outputs
    /* Password Fields
     */
    public InputField PlayerName;
    public InputField Password;
    public Text PasswordPrompt;

    /* For the Story display
     */
    public Text TextOutput;
    public InputField TextInput;


    /* For the Inventory display
     */
    public Text TextOutputInv;
    public InputField TextInputInv;

    #endregion


    #region Canvas Switching
    /// <summary>
     /// Takes a canvas and shows it. Has specific behavior depending on what canvas you switch to
     /// </summary>
     /// <param name="prCanvas"></param>
    public void SwitchToCanvas(Canvas prCanvas)
    {
        if (prCanvas == CnvInventory) // In case you switch to the inventory, update the inventory
        {
            UpdateInventory();
        }
        if (prCanvas == CnvLoginScreen) //If you switch to the login screen, logout the user and hide the tabs
        {
            GameModel.CurrentPlayerName = "";
            GameModel.LoggedIn = false;
            TextOutput.text = "";
            HideCanvas(CnvTabs);

        }
        ShowCanvas(prCanvas);
        CurrentCnv = prCanvas;
        DebugLog("The current canvas is: " + CurrentCnv.name);
    }


    /// <summary>
    /// Shows the specified canvas and hides all others
    /// </summary>
    /// <param name="prCanvas"></param>
    private void ShowCanvas(Canvas prCanvas)
    {
        prCanvas.gameObject.SetActive(true);
        foreach (Canvas prCnv in Canvases)
        {
            if (prCnv.name != prCanvas.name)
            { HideCanvas(prCnv); }
        }
    }


    /// <summary>
    /// Hides the specified canvas
    /// </summary>
    /// <param name="prCanvas"></param>
    private void HideCanvas(Canvas prCanvas)
    {
        prCanvas.gameObject.SetActive(false);
    }


    /// <summary>
    /// Adds the specified canvas to the list of canvases
    /// </summary>
    /// <param name="prCanvas"></param>
    private void AddCanvas(Canvas prCanvas)
    {
        Canvases.Add(prCanvas);
    }
    #endregion


    #region Password Processing
    /// <summary>
    /// Login player. This script gets attached to a Unity game object
    /// </summary>
    public void LogInPlayer()
    {

        LogInPlayer(PlayerName.text, Password.text);
    }

    /// <summary>
    /// Takes the username and password, and attempts to log the user in. On failure it displays a relevant error message on the screen to notify the user as to what was wrong
    /// </summary>
    /// <param name="prPlayerName"></param>
    /// <param name="prPassword"></param>
    public void LogInPlayer(string prPlayerName, string prPassword)
    {
        string lcResult = GameModel.LogIn(prPlayerName, prPassword);
        if ( lcResult == "Success")
        {
            PlayerName.text = ""; Password.text = ""; PasswordPrompt.text = "";
            CnvTabs.gameObject.SetActive(true);
            HideCanvas(CnvLoginScreen);
            SwitchToCanvas(CnvGame);
            PushOutput(GameModel.CurrentPlayer.CurrentArea.AreaText + GameModel.CurrentPlayer.CurrentArea.ItemsText + GameModel.CurrentPlayer.CurrentArea.DestinationText);
        }
        else
        {
            PasswordPrompt.text = lcResult;
        }
    }




    /// <summary>
    /// Register player. This script gets attached to a Unity game object
    /// </summary>
    public void RegisterPlayer()
    {
        RegisterPlayer(PlayerName.text, Password.text);
    }

    /// <summary>
    /// Takes the username and password, and attempts to register a new user and log them in. On failure it displays a relevant error message on the screen to notify the user as to what was wrong
    /// </summary>
    /// <param name="prPlayerName"></param>
    /// <param name="prPassword"></param>
    public void RegisterPlayer(string prPlayerName, string prPassword)
    {
        string lcResult = GameModel.RegisterPlayer(prPlayerName, prPassword);
        if (lcResult == "Success")
        {
            PlayerName.text = ""; Password.text = ""; PasswordPrompt.text = "";
            CnvTabs.gameObject.SetActive(true);
            HideCanvas(CnvLoginScreen);
            SwitchToCanvas(CnvGame);
            GameModel.CurrentPlayer.CurrentArea.Arrive();
            PushOutput(GameModel.CurrentPlayer.CurrentArea.AreaText + GameModel.CurrentPlayer.CurrentArea.ItemsText + GameModel.CurrentPlayer.CurrentArea.DestinationText);
        }
        else
        {
            PasswordPrompt.text = lcResult;
        }
    }
    #endregion


    #region Text IO
    /// <summary>
    /// Takes the user text input, runs it throught the command processor, and pushes the output to the screen
    /// </summary>
    /// <param name="prCmdInput"></param>
    public void SubmitInput(Text prCmdInput)
    {
        CommandProcessor lcCmdProcessor = new CommandProcessor(); //Instantiate a command processor

        string lcOutputText = lcCmdProcessor.Parse(prCmdInput.text); //Pass the Inputted command into the command processor to parse

        PushOutput(lcOutputText);//Push output text to the display

        ResetInput();//Reset Input field and input text
    }

    /// <summary>
    /// Takes the output text and puts it on the screen
    /// </summary>
    /// <param name="prOutputText"></param>
    private void PushOutput(string prOutputText)
    {
        if (CurrentCnv == CnvGame)
        {
            TextOutput.text += prOutputText;
        }
        else if (CurrentCnv == CnvInventory)
        {
            TextOutputInv.text += prOutputText;
        }
        
    }

    /// <summary>
    /// Empties the input field
    /// </summary>
    private void ResetInput()
    {
        TextInput.text = "";
        TextInput.ActivateInputField();

        TextInputInv.text = "";
        TextInput.ActivateInputField();
    }

    /// <summary>
    /// Updates the inventory text on screen
    /// </summary>
    public void UpdateInventory()
    {
        TextOutputInv.text = null;
        if (GameModel.CurrentPlayer.LstInventory != null)
        {
            TextOutputInv.text += GameModel.CurrentPlayer.InventoryString;
        }
        else
        {
            TextOutputInv.text = "Your inventory is empty!";
        }
    }
    #endregion


    #region Misc
    /// <summary>
    /// Pushes text to the debug log. 
    /// Allows parts of the model to push to debug without directly interfacing with unity
    /// </summary>
    /// <param name="prDebugText"></param>
    public static void DebugLog(string prDebugText)
    {
        Debug.Log(prDebugText);
    }

    /// <summary>
    /// Runs when created. Sets up the default view
    /// </summary>
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("I am the one");

            if (Canvases == null)

                AddCanvas(CnvLoginScreen);
            AddCanvas(CnvGame);
            AddCanvas(CnvMap);
            AddCanvas(CnvInventory);


            CnvLoginScreen.gameObject.SetActive(true);
            CnvTabs.gameObject.SetActive(false);
        }
    }

    public void ResetDB()
    {
        GameModel.DB.SetDefaults();
    }
    #endregion

}
