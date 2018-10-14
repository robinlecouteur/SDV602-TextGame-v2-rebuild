﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    #region Views aka Displays - perhaps
    /*     Displays are expressed as Canvases
     */
    public Canvas StartGameMenu;
    public Canvas Story;
    public Canvas Inventory;
    public Canvas Navigator;
    #endregion

    #region Text inputs and outputs
    /* Password Fields
     */
    public Text UserName;
    public Text Password;
   
    /* For the Story display
     */
    public Text StoryDisplay;

    /* For the Score display
     */
     public Text ScoreDisplay;
    #endregion

 
    #region Display Control
    /*
     * Display control
     * 
     */
    public void SwitchTo(Canvas pCanvas)
    {
        GameModel.ShowCanvas(pCanvas);
    }

    public void HideCanvas(Canvas pCanvas)
    {
        pCanvas.enabled = false;
    }

   public void Restart()
    {
        Navigator.enabled = false;
        SwitchTo(StartGameMenu);
    }
    #endregion

    #region Command Processing
    /* For the Command Input
     */
    /*
     * Command processing
     */
    public void Command( string pCmd)
    {

    }

    #endregion

    #region Password Processing
    /*
     * Password
     */
    public Text PasswordPrompt;

    /*
     * Check password
     */
    public void CheckPassword()
    {
        CheckPassword(UserName.text, Password.text);
    }
    public void CheckPassword(string pUserName, string pPassword)
    {
        if( GameModel.PlayerManager.LogIn(pUserName, pPassword))
        {
            Navigator.enabled = true;
        }
        else
        {
            if (GameModel.PlayerManager.PlayerExists(pUserName))
            {
                PasswordPrompt.text = "Please enter a valid password";
                Navigator.enabled = false;
            }
            else
            {
                GameModel.PlayerManager.RegisterPlayer(pUserName, pPassword);
                if (GameModel.PlayerManager.LoggedIn)
                {
                    Navigator.enabled = true;
                    PasswordPrompt.text = "Please enter a password";
                    SwitchTo(Story);

                }
                
            }
        }
    }

    #endregion


  /*
  * Runs when the component is started
  */
    void Start()
    {

        GameModel.AddCanvas(StartGameMenu);
        GameModel.AddCanvas(Story);
        GameModel.AddCanvas(Inventory);
        Navigator.enabled = false;

    }

    #region For the timer << this is a proposed count down timer shown in the ZUp text
    /*   For the timer - currently not implemented
    */
    public Text Zup;
    // private float timeLeft = 3.0f;

    /* Only using this here for the timer
     */
    private void Update()
    {

    }
    #endregion





}
