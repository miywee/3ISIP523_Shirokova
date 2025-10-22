
using System;
class Pr1
{
	static void Main(string[] args)
	{
		int oper;
		Console.WriteLine("Введите количество операций: ");
		oper = Convert.ToInt32(Console.ReadLine());
		int[] prices = new int[oper];
		string[] product = new string[oper];
		for (int i = 0; i < oper; i++)
		{
			Console.WriteLine("Введите товар или услугу и цену(через ;)");
			string str = Console.ReadLine();
			string[] els = str.Split(new char[] { ';' });
			prices[i] = Convert.ToInt32(els[1]);
			product[i] = els[0];
		}
		while (true)
		{
			Console.WriteLine("МЕНЮ");
			Console.WriteLine("1. Вывод данных");
			Console.WriteLine("2. Статистика");
			Console.WriteLine("3. Сортировка по цене");
			Console.WriteLine("4. Конвертация валюты");
			Console.WriteLine("5. Поиск по названию");
			Console.WriteLine("0. Выход");
			string choice = Console.ReadLine();

			switch (choice)
			{
				case "1":
					Show(product, prices, oper);
					break;
				case "2":
					Stat(prices, oper);
					break;
				case "3":
					Sort(prices, product, oper);
					break;
				case "4":
					ConvertV(product, prices, oper);
					break;
				case "5":
					Search(product, prices, oper);
					break;
				case "0":
					return;
				default:
					Console.WriteLine("Неверный выбор");
					break;
			}
		}
	}
	static void Show(string[] product, int[] prices, int oper)
	{
		for (int i = 0; i < oper; i++)
		{
			Console.WriteLine($"{product[i]} {prices[i]}");
		}
	}

	static void Stat(int[] prices, int oper)
	{
		int sum = 0, average, min = prices[0], max = prices[0];
		for (int i = 0; i < oper; i++)
		{
			sum += prices[i];
			if (min > prices[i]) min = prices[i];
			if (max < prices[i]) max = prices[i];
		}
		average = sum / oper;
		Console.WriteLine($"Сумма: {sum} руб.");
		Console.WriteLine($"Среднее: {average} руб.");
		Console.WriteLine($"Минимальная цена: {min} руб.");
		Console.WriteLine($"Максимальная цена: {max} руб.");

	}

	static void Sort(int[] prices, string[] product, int oper)
	{
		for (int i = 0; i < oper - 1; i++)
		{
			for (int j = 0; j < oper - 1 - i; j++)
			{
				if (prices[j] > prices[j + 1])
				{
					int tmp = prices[j];
					prices[j] = prices[j+1];
					prices[j+1] = tmp;

					string ttmp = product[j];
					product[j] = product[j+1];
					product[j+1] = ttmp;
				}
			}
		}
	}

	static void ConvertV(string[] products, int[] prices, int oper)
	{
		Console.WriteLine("1. Доллары");
		Console.WriteLine("2. Евро");
		Console.WriteLine("3. Юани");
		Console.WriteLine("4. Лари");
		string choicee = Console.ReadLine();

		double rate = 0;
		string currency = "";

		switch (choicee)
		{
			case "1":
				rate = 80.39;
				currency = "USD";
				break;
			case "2":
				rate = 93;
				currency = "EUR";
				break;
			case "3":
				rate = 11.3;
				currency = "CNY";
				break;
			case "4":
				rate = 29.67;
				currency = "GEL";
				break;
			default:
				Console.WriteLine("Неверный выбор");
				return;
		}

		for (int i = 0; i < oper; i++)
		{
			double converted = prices[i] / rate;
			Console.WriteLine($"{products[i]} - {converted} {currency}");
		}

	}

	static void Search(string[] product, int[] prices, int oper)
	{
		Console.WriteLine("Введите название для поиска: ");
		string pos = Console.ReadLine();
		bool found = false;
		for (int i = 0; i < oper; i++)
		{
			if (product[i].ToLower().Contains(pos.ToLower())) 
			{
				Console.WriteLine($"{product[i]} - {prices[i]} руб.");
				found = true;
			}
		}
		if (!found)
		{
			Console.WriteLine("Товары не найдены");
		}
	}
}




