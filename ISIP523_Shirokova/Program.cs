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
            Console.WriteLine("МЕНЮ:");
            Console.WriteLine("1. Ввести текст");
            Console.WriteLine("2. Количество слов");
            Console.WriteLine("3. Самое короткое слово");
            Console.WriteLine("4. Самое длинное слово");
            Console.WriteLine("5. Количество предложений");
            Console.WriteLine("6. Гласные и согласные");
            Console.WriteLine("7. Частота букв");
            Console.WriteLine("8. Полный анализ");
            Console.WriteLine("9. Количество анализов");
            Console.WriteLine("0. Выход");
            Console.Write("Выберите: ");
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
                    Console.WriteLine("Неверный выбор!");
                    break;
            }
        }
    }
    static void InputText()
    {
        Console.Write("Введите текст (минимум 100 символов): ");
        string text = Console.ReadLine();
        
        if (text.Length < 100)
        {
            Console.WriteLine("Ошибка, минимум 100 символов\n");
            return;
        }
        
        currentText = text;
        Console.WriteLine("Текст сохранен!\n");
    }
}