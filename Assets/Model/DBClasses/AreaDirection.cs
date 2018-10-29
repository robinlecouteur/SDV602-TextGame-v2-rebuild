using SQLite4Unity3d;

/// <summary>
/// Class to define the AreaDirection table in the database. Stores what areas are in what directions of an area
/// </summary>
public class AreaDirection{
    [PrimaryKey, AutoIncrement] public int ID { get; set; }
    [NotNull] public string FromAreaName { get; set; }
    [NotNull] public string ToAreaName { get; set; }
    [NotNull] public string Direction { get; set; }

}