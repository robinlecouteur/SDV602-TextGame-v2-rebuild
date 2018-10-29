using System;

/// <summary>
/// Class for the command processor. Directly recieves a command from the user, tokenises it, runs the command, then returns the result
/// </summary>
public class CommandProcessor
{
    /// <summary>
    /// Parses the user inputted command
    /// </summary>
    /// <param name="prCmdStr"></param>
    /// <returns name="lcResult"></returns>
    public String Parse(String prCmdStr)
    {
        String lcResult = "Do not understand"; // Default result

        prCmdStr = prCmdStr.ToLower();
        Command.Tokens = prCmdStr.Split(' '); // Tokenise the command
        Command.CurrentToken = 0;

        CommandMap lcMap = new CommandMap();
        if (lcMap.RunCmd())
        {
            lcResult = lcMap.Result;
            lcResult = "\n ---------------------------------------------------------------  \n" + lcResult; // Format the output text 
        }
        else
            lcResult = "\n" + lcResult;
       
        return lcResult;

    }
}
