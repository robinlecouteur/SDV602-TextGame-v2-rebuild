using System.Collections.Generic;

/// <summary>
/// Contains a map of possible commands
/// </summary>
public class CommandMap
{

    private Dictionary<string, Command> _commands;
    public string Result = "";

    /// <summary>
    /// Constructor. Initialises the dictionary and adds the commands to it
    /// </summary>
    public CommandMap()
    {
        _commands = new Dictionary<string, Command>();
        _commands.Add("go", new GoCommand());
        _commands.Add("pick", new PickCommand());
        _commands.Add("show", new ShowCommand());
        _commands.Add("attack", new AttackCommand());
        _commands.Add("use", new UseCommand());
    }

    /// <summary>
    /// Handles running a command for the current token if it matches up to a command in the dictionary
    /// </summary>
    /// <returns name="lcResult"></returns>
    public bool RunCmd()
    {
        bool lcResult = false;
        Command lcCmd;
        string lcStrCommand = Command.Tokens[Command.CurrentToken];
        if (_commands.ContainsKey(lcStrCommand))
        {
            lcCmd = _commands[lcStrCommand];
            lcCmd.Do(this); 
            lcResult = true;
        }
        else
        {
            GameManager.DebugLog("I do not understand");
            lcResult = false;
        }
        return lcResult;
    }
}
