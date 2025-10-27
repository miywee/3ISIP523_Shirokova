using System;

class TextAnalyzer
{
    static string currentText = "";
    static int analysisCount = 0;

    static void Main(string[] args)
    {
        Console.WriteLine("Анализ текста");
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
                    InputText();
                    break;
                case "2":
                    WordCount();
                    break;
                case "3":
                    ShortestWord();
                    break;
                case "4":
                    LongestWord();
                    break;
                case "5":
                    SentenceCount();
                    break;
                case "6":
                    Letters();
                    break;
                case "7":
                    Stat();
                    break;
                case "8":
                    Analysis();
                    break;
                case "9":
                    Count();
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
        Console.Write("Введите текст (минимум 100 символов): \n");
        Console.WriteLine("Для завершения ввода введите пустую строку:");
        string text = "";
        string line;

        while ((line = Console.ReadLine()) != "")
        {
            text += line + " ";
        }
        
        if (text.Length < 100)
        {
            Console.WriteLine("Ошибка, минимум 100 символов");
            return;
        }
        
        currentText = text;
        Console.WriteLine("Текст сохранен!");
    }
    
    static void WordCount()
    {
        string[] words = currentText.Split(' ');
        int count = 0;
    
        for (int i = 0; i < words.Length; i++)
        {
            if (words[i] != "")
            {
                count++;
            }
        }
    
        Console.WriteLine($"Слов в тексте: {count}");
    }

    static void ShortestWord()
    {
        string[] words = currentText.Split(' ',',', '.', '!', '?', ';', ':');
        string shortest = "";
        bool found = false;
        for (int i = 0; i < words.Length; i++)
        {
            if (words[i].Length > 0)
            {
                if (!found || words[i].Length < shortest.Length)
                {
                    shortest = words[i];
                    found = true;
                }
            }
        }
        Console.WriteLine($"Самое короткое слово: '{shortest}' ({shortest.Length} букв)");
    }

    static void LongestWord()
    {
        string[] words = currentText.Split(' ',',', '.', '!', '?', ';', ':');
        string longest = "";
        for (int i = 0; i < words.Length; i++)
        {
            if (words[i] != "" && words[i].Length > longest.Length)
            {
                longest = words[i];
            }
        }
        Console.WriteLine($"Самое длинное слово: '{longest}' ({longest.Length} букв)");
    }
    
    static void SentenceCount()
    {
        int count = 0;
        for (int i = 0; i < currentText.Length; i++)
        {
            char c = currentText[i];
            if (c == '.' || c == '!' || c == '?')
            {
                count++;
            }
        }
        if (count == 0 && currentText != "") count = 1;
        Console.WriteLine($"Предложений в тексте: {count}");
    }
    
    static void Letters()
    {
        int vowels = 0;
        int consonants = 0;
        string vowelLetters = "аеёиоуыэюя";
        
        for (int i = 0; i < currentText.Length; i++)
        {
            char c = char.ToLower(currentText[i]);
            if (char.IsLetter(c))
            {
                if (vowelLetters.Contains(c.ToString()))
                {
                    vowels++;
                }
                else
                {
                    consonants++;
                }
            }
        }
        Console.WriteLine($"Гласные: {vowels}, Согласные: {consonants}");
    }
    
    static void Stat()
    {
        int[] counts = new int[33];
        int totalLetters = 0;
        for (int i = 0; i < currentText.Length; i++)
        {
            char c = char.ToLower(currentText[i]);
        
            if (c >= 'а' && c <= 'я')
            {
                counts[c - 'а']++;
                totalLetters++;
            }
        }
    
        Console.WriteLine("Частота букв:");
    
        for (int top = 0; top < 5; top++)  
        {
            int maxCount = 0;
            char maxChar = ' ';
        
            for (int i = 0; i < counts.Length; i++)
            {
                if (counts[i] > maxCount)
                {
                    maxCount = counts[i];
                    maxChar = (char)('а' + i);
                }
            }
            if (maxCount > 0)
            {
                double percent = (double)maxCount / totalLetters * 100;
                Console.WriteLine($"{percent:F1}%");
            
                for (int i = 0; i < counts.Length; i++)
                {
                    if (counts[i] == maxCount)
                    {
                        counts[i] = 0;
                        break;
                    }
                }
            }
        }
        Console.WriteLine();
    }

    private static void Analysis()
    {
        Console.WriteLine("ПОЛНЫЙ АНАЛИЗ");
        
        WordCount();
        ShortestWord();
        LongestWord();
        SentenceCount();
        Letters();
        Stat();
        analysisCount++;
        Console.WriteLine("Анализ завершен!");
    }
    static void Count()
    {
        Console.WriteLine($"Выполнено анализов: {analysisCount}");
    }
    
    
}