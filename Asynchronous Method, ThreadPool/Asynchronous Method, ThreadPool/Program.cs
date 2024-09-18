using System.IO;
using System.Text;
namespace Asynchronous_Method__ThreadPool
{
    internal class Program
    {
        static void EncryptFile(string filePath,char key)
        {
            try
            {
                var content = File.ReadAllText(filePath);
                StringBuilder stringBuilder = new StringBuilder();

                for (int i = 0; i < content.Length; i++)
                {
                    stringBuilder.Append(content[i] ^ key);
                }
                Console.WriteLine(stringBuilder);
            }
            catch {
                throw;
            }
            
        }
        
        static void Main(string[] args)
        {
            Console.Write("Enter File Path : ");
            string filePath = Console.ReadLine();
            Console.Write("Enter Key : ");
            var key = char.Parse(Console.ReadLine());

            Thread th = new Thread(() => EncryptFile(filePath, key));
            th.Start();
        }
    }
}
