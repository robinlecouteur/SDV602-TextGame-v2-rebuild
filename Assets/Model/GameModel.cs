using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class GameModel {
    private static PlayerManager _playerManager;
    private static StoryManager _storyManager;


    public static StoryManager Story
    {
        get { return _storyManager; }

    }

    public static PlayerManager PlayerManager
    {
        get { return _playerManager; }

    }


    /*
     * Views
     */
    public static List<Canvas> ViewRenders = new List<Canvas>();

    public static void AddCanvas(Canvas pCanvas)
    {
        ViewRenders.Add(pCanvas);
    }
    public static void ShowCanvas(Canvas pCanvas)
    {
        pCanvas.enabled = true;

        //var otherViews = 
        ViewRenders.Where(x =>
        {
            if (x.name != pCanvas.name)
            {
                x.enabled = false;
                return true;
            }
            else
                return false;
        }).ToList<Canvas>();
        //Debug.Log(otherViews.Count());
    }

    static GameModel()
    {
        /* 
         * When the GameModel is made
         * Make the Story(Manager)
         * Make the Player(Manager)
         */
        _storyManager = new StoryManager();
        _playerManager = new PlayerManager();
      
       
    }

}
