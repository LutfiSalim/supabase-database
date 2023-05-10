using SupabaseNpg;

class Program
{
    static async Task Main(string[] args)
    {
        string connectionString = string.Empty;
        using (var configReader = new ConfigReader("configuration.json"))
        connectionString = await configReader.ReadAsync();

        var client = new SupabaseDatabaseClient(connectionString);

        while (true)
        {
            Console.Write("Enter a command (create, delete, get, list, update, exit): ");
            var command = Console.ReadLine();

            switch (command)
            {
                case "create":
                    Console.Write("Enter name: ");
                    var name = Console.ReadLine();
                    Console.Write("Enter email: ");
                    var email = Console.ReadLine();
                    Console.Write("Enter password: ");
                    var password = Console.ReadLine();

                    var result = await client.CreateData(name!, email!, password!);

                    Console.WriteLine(result);
                    break;

                case "delete":
                    Console.Write("Enter the UserID to delete: ");
                    var userIdStringDelete = Console.ReadLine();
                    var rowsAffected = await client.DeleteData(userIdStringDelete);
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine($"{rowsAffected} row(s) deleted.");
                    }
                    else
                    {
                        Console.WriteLine("wrong UserID");
                    }
                    break;

                case "get":
                    Console.Write("Enter the UserID to retrieve: ");
                    var userIdStringGet = Console.ReadLine();
                    var dataGet = await client.GetData(userIdStringGet);
                    if (dataGet != null || dataGet.Count > 0)
                    {
                        foreach (var row in dataGet)
                        {
                            Console.WriteLine($"User_id: {row["user_id"]} Name: {row["name"]} Email:{row["email"]}  Password:{row["password"]}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No rows found.");
                    }
                    break;

                case "list":
                    var data = await client.ListData();
                    if (data != null || data.Count > 0)
                    {
                        foreach (var row in data)
                        {
                            Console.WriteLine($"User_id: {row["user_id"]} Name: {row["name"]} Email:{row["email"]}  Password:{row["password"]}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No rows found.");
                    }
                    break;

                case "update":
                    Console.Write("Enter the UserID to update: ");
                    var userIdStringUpdate = Console.ReadLine();
                    Console.Write("Enter new name: ");
                    var nameupdate = Console.ReadLine();
                    Console.Write("Enter new email: ");
                    var emailupdate = Console.ReadLine();
                    Console.Write("Enter new password: ");
                    var passwordupdate = Console.ReadLine();
                    var rowsupdated = client.UpdateData(userIdStringUpdate!, nameupdate!, emailupdate!, passwordupdate!);
                    if (rowsupdated.Status != TaskStatus.Faulted)
                    {
                        Console.WriteLine($"Id : {userIdStringUpdate} updated.");
                    }
                    else
                    {
                        Console.WriteLine("Wrong ID: Table not updated");
                    }
                    break;

                case "exit":
                    return;

                default:
                    Console.WriteLine("Invalid command.");
                    break;
            }
        }
    }

}