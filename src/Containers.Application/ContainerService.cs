using Containers.Models;
using Microsoft.Data.SqlClient;

namespace Containers.Application;

public class ContainerService : IContainerService
{
    private readonly String connectionString;

    public ContainerService(String connectionString)
    {
        this.connectionString = connectionString;
    }

    public IEnumerable<Container> GetAllContainers()
    {
        List<Container> containers = [];
        
        
        string query = "SELECT * FROM Containers";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlCommand command = new SqlCommand(query, connection);
            
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            try
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var container = new Container()
                        {
                            id = reader.GetInt32(0),
                            Containertypeid = reader.GetInt32(1),
                            isHazardious = reader.GetBoolean(2),
                            Name = reader.GetString(3),
                        };
                        containers.Add(container);
                    }
                }
            }
            finally
            {
                reader.Close();
            }
        }
       return containers;
    }
    
    public bool Create(Container container)
    {
        const string insertString = "INSERT INTO Containers (Containertypeid,isHazardous,Name) VALUES (@Containertypeid,@IsHazardous,@Name)";
        int countRowsAdd = -1;

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlCommand command = new SqlCommand(insertString, connection);

            command.Parameters.AddWithValue("@Containertypeid", container.Containertypeid);
            command.Parameters.AddWithValue("@IsHazardous", container.isHazardious);
            command.Parameters.AddWithValue("@Name", container.Name);

            connection.Open();
            countRowsAdd = command.ExecuteNonQuery();
        }
        return countRowsAdd != -1;
    }
}
