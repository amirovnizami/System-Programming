using Bogus;
using System.CodeDom.Compiler;
using System.Collections.Concurrent;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace CriticalSectionTask
{
    class User
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
    internal class Program
    {
        static object _sync = new object();
        static void Main(string[] args)
        {
            List<User> AllUser = new List<User>();

            void writeFile()
            {
                for (int i = 0; i < 5; i++)
                {
                    var faker = new Faker();

                    var users = new Faker<User>().RuleFor(u => u.Name, p => p.Person.FirstName)
                        .RuleFor(u => u.Surname, p => p.Person.LastName)
                        .RuleFor(u => u.DateOfBirth, p => p.Person.DateOfBirth)
                        .Generate(50);
                    var json = JsonSerializer.Serialize(users);
                    File.WriteAllText($"{i}.json", json);
                }
            }
            void writeListSingle()
            {
                for (int i = 0; i < 5; i++)
                {
                    using FileStream fs = new FileStream($"{i}.json", FileMode.Open);
                    var users = System.Text.Json.JsonSerializer.Deserialize<User[]>(fs);
                    for (int j = 0; j < users.Length; j++)
                    {
                        AllUser.Add(users[j]);
                    }
                }
                //foreach (var user in AllUser)
                //{
                //    Console.WriteLine(user.Name);
                //}

            }
            void writeListMulti()
            {
                for (int i = 0; i < 5; i++)
                {
                    int j = i;
                    ThreadPool.QueueUserWorkItem((o) =>
                    {
                        lock(_sync){
                            using FileStream fs = new FileStream($"{j}.json", FileMode.Open);
                            var users = System.Text.Json.JsonSerializer.Deserialize<User[]>(fs);
                            for (int j = 0; j < users.Length; j++)
                            {
                                AllUser.Add(users[j]);
                            }
                        }
                        
                    });

                }
                Thread.Sleep(2000);

                //foreach (var user in AllUser)
                //{
                //    Console.WriteLine(user.Name);
                //}
            }


            writeFile();
            Console.WriteLine("1.Single");
            Console.WriteLine("2.Multiple");
            var choice = Console.ReadLine();
            if (choice == "1")
            {
                ThreadPool.QueueUserWorkItem((o) => writeListSingle());

            }
            else if (choice == "2")
            {
                writeListMulti();
            }
            else
            {
                Console.WriteLine("Incorrect choice!");
            }

            Thread.Sleep(2000);

            foreach(User user in AllUser)
            {
                Console.WriteLine($"Name: {user.Name} , Surname :{user.Surname} , DateOfBirth :{user.DateOfBirth}");
            }
        }


    }
}
