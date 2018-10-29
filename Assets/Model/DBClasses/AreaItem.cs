using SQLite4Unity3d;

/// <summary>
/// Class to define an AreaItem in the database. Acts as a join table between Area and Item since the database can't store a list of items inside an area
/// </summary>
public class AreaItem  {
    [PrimaryKey, AutoIncrement] public int ID { get; set; }
    [NotNull] public string AreaName { get; set; }
    [NotNull] public string ItemName { get; set; }
}
