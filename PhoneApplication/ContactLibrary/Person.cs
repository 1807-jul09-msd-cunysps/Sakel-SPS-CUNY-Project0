using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;

namespace ContactLibrary
{
    [DataContract] // serialize the directory class
    public class ContactDirectory
    {
        [DataMember] // data member needs to be serialize
        public List<Person> People { get; set; }

        private SqlConnection connection = null;
        private SqlCommand command = null;
        private SqlDataReader reader = null;
        private NLog.Logger logger = null;

        public ContactDirectory()
        {
            People = new List<Person>();
            connection = new SqlConnection("Data Source=pavel121096.database.windows.net;Initial Catalog=PhoneAPP;Persist Security Info=True;User ID=shossain112;Password=Smackdown1");
            connection.Open();
            command = new SqlCommand("", connection);
            logger = NLog.LogManager.GetCurrentClassLogger();
        }

        private readonly Dictionary<int, string> Mapping = new Dictionary<int, string>()
        { { 1, "FirstName" }, { 2, "LastName" }, { 3, "HouseNum" }, { 4, "Street" }, { 5, "City" },
            { 6, "State" }, { 7, "Country" }, { 8, "ZipCode" }, { 9, "CountryCode" }, { 10, "AreaCode" },
            { 11, "Number" }, { 0, "Id" } };

        public void AddPerson(Person p)
        {
            People.Add(p);
        }

        public void AddPersonToDB(Person p)
        {
            command.CommandText = $"insert into Person (Pid, FirstName, LastName) values ({p.Pid}, '{p.FirstName}', '{p.LastName}') " +
                 $"insert into Address (Pid, HouseNum, Street, City, State, Country, ZipCode) values " +
                 $"({p.Pid}, '{p.Address.HouseNum}', '{p.Address.Street}', '{p.Address.City}', '{p.Address.State}', '{p.Address.Country}', '{p.Address.ZipCode}') " +
                 $"insert into Phone (Pid, CountryCode, AreaCode, PhoneNumber) values " +
                 $"({p.Pid}, '{p.Phone.CountryCode}', '{p.Phone.AreaCode}', '{p.Phone.Number})";
            command.ExecuteNonQuery();
        }

        public Person ReadPerson(int Pid)
        {
            IEnumerable<Person> query =
                from person in People
                where person.Pid.Equals(Pid)
                select person;
            if (!query.Any())
            {
                Console.WriteLine($"Person with ID {Pid} does not existin the directory.");
                return null;
            }
            return query.Single();
        }

        public string ReadFromDB(int Pid)
        {
            command.CommandText = $"select per.Pid, FirstName, LastName, HouseNum, Street, City, State, Country, ZipCode, CountryCode, AreaCode, PhoneNumber " +
                $"from Person as per inner join Address as addr on per.Pid = addr.Pid inner join Phone as pho on per.Pid = pho.Pid " +
                $"where per.Pid = {Pid}";
            reader = command.ExecuteReader();
            reader.Read();
            string result = "";
            for (int i = 0; i < reader.FieldCount; ++i)
            {
                result += reader[i] + "\t";
            }
            reader.Close();
            return result;
        }

        public void DeletePerson(int Pid)
        {
            IEnumerable<Person> query =
                from person in People
                where person.Pid.Equals(Pid)
                select person;
            if (!query.Any())
            {
                Console.WriteLine($"Person with ID {Pid} does not exist in the directory.");
                return;
            }
            People.Remove(query.Single());
        }

        public void DeleteFromDB(int Pid)
        {
            command.CommandText = $"delete from Person where Pid = {Pid}";
            command.ExecuteNonQuery();
        }

        public void UpdatePerson(int Pid, int attr, string val)
        {
            IEnumerable<Person> query =
                from person in People
                where person.Pid.Equals(Pid)
                select person;
            if (!query.Any())
            {
                Console.WriteLine($"Person with ID {Pid} does not exist in the directory.");
                return;
            }
            Person p = query.Single();
            switch (attr)
            {
                case int n when n <= 2:
                    p.GetType().GetProperty(Mapping[attr]).SetValue(p, val);
                    break;
                case int n when n <= 8:
                    p.Address.GetType().GetProperty(Mapping[attr]).SetValue(p.Address, val);
                    break;
                case int n when n <= 11:
                    p.Phone.GetType().GetProperty(Mapping[attr]).SetValue(p.Phone, val);
                    break;
                case int n when n < 0 || n > 11:
                    Console.WriteLine("No such attribute.");
                    break;
            }
        }

        public void UpdateInDB(int Pid, int attr, string val)
        {
            switch (attr)
            {
                case int n when n <= 2:
                    command.CommandText = $"update Person {Mapping[attr]} = {val} where Pid = {Pid}";
                    break;
                case int n when n <= 8:
                    command.CommandText = $"update Address {Mapping[attr]} = {val} where Pid = {Pid}";
                    break;
                case int n when n <= 11:
                    command.CommandText = $"update Phone {Mapping[attr]} = {val} where Pid = {Pid}";
                    break;
                case int n when n < 0 || n > 11:
                    Console.WriteLine("No such attribute.");
                    break;
            }
            command.ExecuteNonQuery();
        }

        public List<Person> SearchPerson(int attr, string val)
        {

            IEnumerable<Person> search;
            switch (attr)
            {
                case 1:
                    search =
                        from person in People
                        where person.FirstName.Equals(val)
                        select person;
                    break;
                case 2:
                    search =
                        from person in People
                        where person.LastName.Equals(val)
                        select person;
                    break;
                case 3:
                    search =
                        from person in People
                        where person.Address.ZipCode.Equals(val)
                        select person;
                    break;
                case 4:
                    search =
                        from person in People
                        where person.Address.City.Equals(val)
                        select person;
                    break;
                case 5:
                    search =
                        from person in People
                        let phoneNumber = person.Phone.CountryCode +
                            person.Phone.AreaCode +
                            person.Phone.Number
                        where phoneNumber.Equals(val)
                        select person;
                    break;
                default:
                    Console.WriteLine("Error, attribute does not exist");
                    return null;
            }
            return search.ToList();
        }

        public string SearchInDB(int attr, string val)
        {
            command.CommandText = "select per.Pid, FirstName, LastName, " +
                "HouseNum, Street, City, State, Country, ZipCode, " +
                "CountryCode, AreaCode, PhoneNumber" +
                "from Person as per inner join Address as addr on per.Pid = addr.Pid " +
                "inner join Phone as pho on per.Pid = pho.Pid ";
            switch (attr)
            {
                case 1:
                case 2:
                    command.CommandText += $"where {Mapping[attr]} = '{val}'";
                    break;
                case 3:
                    attr = 8;
                    command.CommandText += $"where {Mapping[attr]} = '{val}'";
                    break;
                case 4:
                    attr = 5;
                    command.CommandText += $"where {Mapping[attr]} = '{val}'";
                    break;
                case 5:
                    command.CommandText += $"where concat(CountryCode, AreaCode, PhoneNumber) = '{val}'";
                    break;
                default:
                    Console.WriteLine("Error, attribute does not exist");
                    return null;
            }
            reader = command.ExecuteReader();
            string result = "";
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; ++i)
                {
                    result += reader[i] + "\t";
                }
                result += "\n";
            }
            reader.Close();
            return result.TrimEnd('\t', '\n');
        }
    }

    public class Person
    {
        public Person()
        {
            /// Initialize the dependant objects
            Address = new Address();
            Phone = new Phone();
        }
        public long Pid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Address Address { get; set; }
        public Phone Phone { get; set; }

        public override string ToString()
        {
            return Pid + " " + FirstName + " " + LastName + " " +
                Address.ToString() + " " + Phone.ToString();
        }
    }
}
