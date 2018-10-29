using SQLite4Unity3d;

/// <summary>
/// Class to define an InventoryItem in the database. Acts as a join table between Player and Item since the database can't store a list of items inside a player
/// </summary>
public class InventoryItem {
    [PrimaryKey, AutoIncrement] public int ID { get; set; }
    [NotNull] public string PlayerName { get; set; }
    [NotNull] public string ItemName { get; set; }
}
