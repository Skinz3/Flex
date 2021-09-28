<p align="center">
  <img src="Utils/logo.png" />
</p>

# Flex 

Flex is a lightweight object-relational mapping (ORM) product for the Microsoft .NET Core and SQL : it provides a framework for mapping an object-oriented domain model to a traditional SQL database.

# Exemple of usage

#### Schema Mapping

```csharp

  [Table("Users")]
  public class User : IEntity // <--- Interface allows extensions methods for entities
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

      [Blob] // <--- Fast serialization using Google Protobuf 
      public Certificate Certificate 
      {
          get;
          set;
      }

      [Transcient]  // <--- This property is ignored in SQL Schema 
      public bool Connected
      {
          get;
          set;
      }
  }
```

 #### Interopability

```csharp

  Database database = new Database(new MySqlProvider("myDb","localhost","root",""));
  // or
  Database database =  new Database(new SQLiteProvider("file.sqlite"));

```

 #### Reading database

```csharp

  Table<User> table = database.GetTable<User>();

  long count = table.Count();

  IEnumerable<User> Users = table.Select();

  IEnumerable<User> users = table.Select(x=> x.Username == "John Doe").GroupBy(x => x.Ip); // <--- Dynamic query builder

```

 #### Writting database

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

 #### Transactional

```csharp

database.BeginTransaction(); // <--- create query cache

database.Insert(new User() { Name : "John" });
database.Insert(new User() { Name : "Ethan" });
database.Insert(new User() { Name : "William" });

database.EndTransaction(); // <--- execute queries

```

 #### Database Copy (from one SGBD to another)

```csharp

  Database mySqlDb = new MySqlDatabase("MyDatabase","localhost","root","");
  Database sqlLiteDb = new SQLiteDatabase("database.sqlite");

  sqlLiteDb.Copy(mySqlDb);

```

# Package Dependencies

 | Name        | Version           |
| ------------- |:-------------:|
| MySql.Data      | 8.0.11 | 
| protobuf-net | 3.0.101 |
| System.Data.SQLite | 1.0.115 | 





