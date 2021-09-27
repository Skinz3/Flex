<p align="center">
  <img src="Utils/logo.png" />
</p>

# Flex 

Flex is a object-relational mapping (ORM) product for the Microsoft .NET Core and SQL : it provides a framework for mapping an object-oriented domain model to a traditional SQL database.

> Flex only supports MySql for now, i am actively working on the creation of a db factory to be able to support SQLite, Postgresql etc.
# Exemple of usage


* Schema Mapping

```csharp

  [Table("Users")]
  public class User : IEntity
  { 
      [Primary]
      [AutoIncrement]
      public int Id
      {
          get;
          set;
      }

      public string Name
      {
          get;
          set;
      }

      [Update]
      public string Ip
      {
          get;
          set;
      }
      /*
         Using Google Protobuf for blob serialization
      */
      [Blob]
      public Certificate Certificate
      {
          get;
          set;
      }

      /*
        This property is ignored in SQL Schema
      */
      [Transcient] 
      public bool Connected
      {
          get;
          set;
      }
  }
```

* Setup

```csharp

  MySqlDatabase database = new MySqlDatabase("MyDatabase","localhost","root","");
```

* Reading database

```csharp

  Table<User> table = database.GetTable<User>();

  long count = table.Count();

  IEnumerable<User> Users = table.Select(); 

  IEnumerable<User> users = table.Select(x=> x.Username == "John Doe").GroupBy(x => x.Ip); // Dynamic query builder

```

* Writting database

Both ```Update()``` ```Insert()``` and ```Delete()``` are extensions method for IEntity. You can also use ```table.Insert(T Entity)```

```csharp

  User user = new User();
  user.Name = "John Doe";
  user.Certificate = new Certificate();

  user.Insert(); // Extension method for IEntity, you can also use table.Insert(T entity)

  user.Ip = "127.0.0.1";

  user.Update();

  table.DeleteAll();

  database.Drop<User>();

```


