using SQLite4Unity3d;
/// <summary>
/// Class to define what an AreaText looks like in the database
/// </summary>
public class AreaText
{
    [PrimaryKey, AutoIncrement] public int ID { get; set; }
    [NotNull] public string Label { get; set; } //Is it default text, second visit text etc
    [NotNull] public string AreaName { get; set; }
    [NotNull] public string Text { get; set; } 

}

