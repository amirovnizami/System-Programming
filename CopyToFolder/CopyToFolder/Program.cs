using System.IO;
namespace CopyToFolder
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter File Path : ");
            var filePath = Console.ReadLine();
            Console.Write("Enter Folder Path : ");
            var folderPath = Console.ReadLine();
            void CopyFile()
            {
                try
                {
                    if (File.Exists(filePath))
                    {
                        var content = File.ReadAllBytes(filePath);
                        string destinationFilePath = Path.Combine(folderPath, Path.GetFileName(filePath));

                        for (int i = content.Length; i >= 0; i--)
                        {
                            Console.WriteLine($"{i} / {content.Length}");
                            Thread.Sleep(0);
                            Console.Clear();

                        }
                        File.Copy(filePath, destinationFilePath, true);

                        Console.WriteLine("Kopyalama ugurla tamamlandi..");

                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }

            Thread thread = new Thread(CopyFile);
            thread.Start();
            
        }
    }
}
