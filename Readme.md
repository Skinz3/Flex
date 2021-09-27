<p align="center">
  <img src="Utils/logo.png" />
</p>

# Flex 

Flex is a object-relational mapping (ORM) product for the Microsoft .NET Core and SQL : it provides a framework for mapping an object-oriented domain model to a traditional SQL database (MySql,Sqlite)

# Exemple of usage

```csharp

  MySqlDatabase database = new MySqlDatabase("MyDatabase","localhost","root","");

  Table<User> table = database.GetTable<User>();

  long count = table.Count();

  IEnumerable<User> users = table.Select();


```