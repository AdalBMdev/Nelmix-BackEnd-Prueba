using System.Data.SqlClient;

/// <summary>
/// Clase que proporciona la cadena de conexión a la base de datos SQL Server desde la configuración de la aplicación.
/// </summary>
public class Connection
{
    private string cadenaSQL = string.Empty;

    /// <summary>
    /// Constructor de la clase que inicializa la cadena de conexión desde la configuración de la aplicación.
    /// </summary>
    public Connection()
    {
        var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();

        cadenaSQL = builder.GetSection("ConnectionStrings:Conex").Value;

    }

    /// <summary>
    /// Obtiene la cadena de conexión a la base de datos SQL Server.
    /// </summary>
    /// <returns>La cadena de conexión a la base de datos.</returns>
    public string getCadenaSQL()
    {
        return cadenaSQL;
    }
}
