using System;
using System.Collections.Generic;
using System.Linq;
using ISIP523_Shirokova;
using ISIP523_Shirokova.Context;
using ISIP523_Shirokova.Entities;

namespace AutoServiceGame
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("автосервис");
            
            List<Detail> details = Core.Context.Details.ToList();
            List<Garage> garages = Core.Context.Garages.ToList();
            List<DetailGarage> garageDetails = Core.Context.GarageDetails.ToList();
            
            if (garages.Count == 0)
            {
                Console.WriteLine("Нет данных в базе.");
                Console.ReadKey();
                return;
            }
            
            var myGarage = garages[0];
            
            Dictionary<int, int> stock = new Dictionary<int, int>();
            
            foreach (var gd in garageDetails.Where(gd => gd.GarageID == myGarage.ID))
            {
                // Каждая запись в GarageDetails - это одна деталь
                // Чтобы хранить количество, считаем сколько раз встречается каждая деталь
                if (stock.ContainsKey(gd.DetailsID))
                {
                    stock[gd.DetailsID]++;
                }
                else
                {
                    stock[gd.DetailsID] = 1;
                }
            }
            
            Random rnd = new Random();
            bool running = true;
            
            while (running)
            {
                Console.Clear();
                Console.WriteLine($"💰 БЮДЖЕТ: {myGarage.Budget} руб.");
                Console.WriteLine($"🏠 Гараж: {myGarage.Name}");
                Console.WriteLine("\n📦 СКЛАД:");
                
                if (stock.Count == 0)
                {
                    Console.WriteLine("   Пусто!");
                }
                else
                {
                    foreach (var kvp in stock)
                    {
                        var detail = details.FirstOrDefault(d => d.ID == kvp.Key);
                        if (detail != null)
                        {
                            Console.WriteLine($"   {detail.Name}: {kvp.Value} шт.");
                        }
                    }
                }
                
                Console.WriteLine("\n1 - Новый клиент");
                Console.WriteLine("2 - Купить запчасти");
                Console.WriteLine("3 - Выйти");
                Console.Write("\nВыбери: ");
                
                string choice = Console.ReadLine();
                
                if (choice == "1")
                {
                    // Новый клиент
                    Console.Clear();
                    
                    if (details.Count == 0)
                    {
                        Console.WriteLine("Нет деталей в базе!");
                        Console.ReadKey();
                        continue;
                    }
                    
                    var brokenDetail = details[rnd.Next(details.Count)];
                    decimal repairCost = brokenDetail.Price * 1.5m;
                    
                    Console.WriteLine($"\n🚗 Клиент приехал!");
                    Console.WriteLine($"🔧 Сломано: {brokenDetail.Name}");
                    Console.WriteLine($"💰 Стоимость ремонта: {repairCost} руб.");
                    
                    // Проверяем есть ли деталь на складе
                    bool hasDetail = stock.ContainsKey(brokenDetail.ID) && stock[brokenDetail.ID] > 0;
                    
                    if (hasDetail)
                    {
                        Console.Write("\nРемонтировать? (да/нет): ");
                        string answer = Console.ReadLine();
                        
                        if (answer.ToLower() == "да")
                        {
                            // Убираем одну деталь со склада
                            stock[brokenDetail.ID]--;
                            if (stock[brokenDetail.ID] == 0)
                                stock.Remove(brokenDetail.ID);
                            
                            // Удаляем одну запись из базы (GarageDetails)
                            var toRemove = Core.Context.GarageDetails
                                .FirstOrDefault(gd => gd.GarageID == myGarage.ID && gd.DetailsID == brokenDetail.ID);
                            
                            if (toRemove != null)
                            {
                                Core.Context.GarageDetails.Remove(toRemove);
                            }
                            
                            // Добавляем деньги
                            var garageInDb = Core.Context.Garages.First(g => g.ID == myGarage.ID);
                            garageInDb.Budget += (int)repairCost;
                            
                            Core.Context.SaveChanges();
                            myGarage.Budget = garageInDb.Budget;
                            
                            Console.WriteLine($"\n✅ Ремонт выполнен! +{repairCost} руб.");
                        }
                        else
                        {
                            // Штраф за отказ
                            var garageInDb = Core.Context.Garages.First(g => g.ID == myGarage.ID);
                            int fine = (int)(repairCost * 0.3m);
                            garageInDb.Budget -= fine;
                            
                            Core.Context.SaveChanges();
                            myGarage.Budget = garageInDb.Budget;
                            
                            Console.WriteLine($"\n❌ Отказ. Штраф: {fine} руб.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("\n❌ Нет этой детали на складе!");
                        Console.Write("\n1 - Отказать (штраф 30%)\n2 - Попробовать поставить другую (штраф 200%)\nВыбери: ");
                        
                        string opt = Console.ReadLine();
                        
                        if (opt == "1")
                        {
                            var garageInDb = Core.Context.Garages.First(g => g.ID == myGarage.ID);
                            int fine = (int)(repairCost * 0.3m);
                            garageInDb.Budget -= fine;
                            
                            Core.Context.SaveChanges();
                            myGarage.Budget = garageInDb.Budget;
                            
                            Console.WriteLine($"\n❌ Отказ. Штраф: {fine} руб.");
                        }
                        else if (opt == "2")
                        {
                            if (stock.Count > 0)
                            {
                                // Берем первую доступную деталь
                                var firstKey = stock.Keys.First();
                                stock[firstKey]--;
                                if (stock[firstKey] == 0)
                                    stock.Remove(firstKey);
                                
                                // Удаляем из базы
                                var toRemove = Core.Context.GarageDetails
                                    .FirstOrDefault(gd => gd.GarageID == myGarage.ID && gd.DetailsID == firstKey);
                                
                                if (toRemove != null)
                                {
                                    Core.Context.GarageDetails.Remove(toRemove);
                                }
                                
                                var garageInDb = Core.Context.Garages.First(g => g.ID == myGarage.ID);
                                int penalty = (int)(repairCost * 2m);
                                garageInDb.Budget -= penalty;
                                
                                Core.Context.SaveChanges();
                                myGarage.Budget = garageInDb.Budget;
                                
                                var wrongDetail = details.First(d => d.ID == firstKey);
                                Console.WriteLine($"\n💥 Поставили {wrongDetail.Name} вместо {brokenDetail.Name}");
                                Console.WriteLine($"💸 Штраф: {penalty} руб.");
                            }
                            else
                            {
                                Console.WriteLine("\n💥 На складе нет деталей вообще!");
                            }
                        }
                    }
                    
                    Console.WriteLine("\nНажми любую клавишу...");
                    Console.ReadKey();
                }
                else if (choice == "2")
                {
                    // Магазин
                    Console.Clear();
                    Console.WriteLine("=== МАГАЗИН ЗАПЧАСТЕЙ ===\n");
                    
                    if (details.Count == 0)
                    {
                        Console.WriteLine("Нет деталей в продаже!");
                    }
                    else
                    {
                        for (int i = 0; i < details.Count; i++)
                        {
                            var detail = details[i];
                            int inStock = stock.ContainsKey(detail.ID) ? stock[detail.ID] : 0;
                            Console.WriteLine($"{i+1} - {detail.Name} ({detail.Price} руб.) | На складе: {inStock} шт.");
                        }
                        
                        Console.Write("\nВыбери деталь (номер): ");
                        if (int.TryParse(Console.ReadLine(), out int num) && num > 0 && num <= details.Count)
                        {
                            var selected = details[num - 1];
                            Console.Write($"Сколько {selected.Name} купить? ");
                            
                            if (int.TryParse(Console.ReadLine(), out int qty) && qty > 0)
                            {
                                int cost = selected.Price * qty;
                                
                                // Проверяем деньги
                                var garageInDb = Core.Context.Garages.First(g => g.ID == myGarage.ID);
                                
                                if (garageInDb.Budget >= cost)
                                {
                                    // Добавляем детали
                                    for (int i = 0; i < qty; i++)
                                    {
                                        var newItem = new GarageDetails
                                        {
                                            GarageID = myGarage.ID,
                                            DetailsID = selected.ID
                                        };
                                        Core.Context.GarageDetails.Add(newItem);
                                    }
                                    
                                    // Обновляем словарь
                                    if (stock.ContainsKey(selected.ID))
                                    {
                                        stock[selected.ID] += qty;
                                    }
                                    else
                                    {
                                        stock[selected.ID] = qty;
                                    }
                                    
                                    // Списываем деньги
                                    garageInDb.Budget -= cost;
                                    
                                    Core.Context.SaveChanges();
                                    myGarage.Budget = garageInDb.Budget;
                                    
                                    Console.WriteLine($"\n✅ Куплено {qty} шт. за {cost} руб.");
                                }
                                else
                                {
                                    Console.WriteLine($"\n❌ Не хватает денег! Нужно: {cost}, есть: {garageInDb.Budget}");
                                }
                            }
                        }
                    }
                    
                    Console.WriteLine("\nНажми любую клавишу...");
                    Console.ReadKey();
                }
                else if (choice == "3")
                {
                    running = false;
                }
            }
            
            Console.WriteLine("\n👋 Игра окончена!");
            Console.ReadKey();
        }
    }
}