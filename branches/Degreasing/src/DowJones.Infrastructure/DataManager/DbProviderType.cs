namespace DowJones.DataManager
{
    /// <summary>
    /// Specifies the database provider types supported by <see cref="DbManager"/>.  
    /// Use <see cref="DbProviderType.Other"/> for custom Ole Db providers not on this list.
    /// </summary>
    public enum DbProviderType
    {
        SqlClient,
        Oracle,
        OleDb,
        Odbc,
        Other,
    }
}
