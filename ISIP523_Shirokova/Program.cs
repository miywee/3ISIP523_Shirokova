using System;

class TextAnalyzer
{
    static string currentText = "";
    static int analysisCount = 0;

    static void Main(string[] args)
    {
        Console.WriteLine("Анализ текста\n");
        while (true)
        {
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    break;
                case "2":
                    break;
                case "3":
                    break;
                case "4":
                    break;
                case "5":
                    break;
                case "6":
                    break;
                case "7":
                    break;
                case "8":
                    break;
                case "9":
                    break;
                case "0":
                    Console.WriteLine("Выход");
                    return;
                default:
                    Console.WriteLine("Неверный выбор!\n");
                    break;
            }
        }
    }
}