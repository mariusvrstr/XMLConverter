
namespace XMLConverter
{
    using System;

    public class Program
    {
        public static void Main(string[] args)
        {
            var model = new Model.Model();
            
            model.GenerateNewFile();

            Console.ReadKey();

        }




    }
}
