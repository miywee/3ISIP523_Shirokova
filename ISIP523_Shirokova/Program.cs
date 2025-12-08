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
        static Garage myGarage;
        static List<Detail> allDetails;
        static Dictionary<int, int> stock = new Dictionary<int, int>();
        static Random rnd = new Random();
        
        static List<DeliveryOrder> deliveryQueue = new List<DeliveryOrder>();
        
        static int customersCounter = 0;

        static void Main(string[] args)
        {
            Console.WriteLine("АВТОСЕРВИС");
            LoadData();

            if (myGarage == null)
            {
                Console.WriteLine("Нет данных в базе.");
                Console.ReadKey();
                return;
            }
            
            LoadStock();
            Game();

            Console.WriteLine("\nИгра окончена!");
            Console.ReadKey();
        }
        
        class DeliveryOrder
        {
            public int DetailId { get; set; }
            public int Quantity { get; set; }
            public int DeliveryIn { get; set; } 
        }
        
        static void LoadData()
        {
            allDetails = Core.Context.Details.ToList();
            var garages = Core.Context.Garages.ToList();
            
            if (garages.Count > 0)
                myGarage = garages[0];
        }
        
        static void LoadStock()
        {
            var garageDetails = Core.Context.GarageDetails.ToList();
            
            foreach (var item in garageDetails)
            {
                if (item.GarageId == myGarage.Id)
                {
                    if (stock.ContainsKey(item.DetailsId))
                        stock[item.DetailsId]++;
                    else
                        stock[item.DetailsId] = 1;
                }
            }
        }
        
        static void Game()
        {
            bool playing = true;
            while (playing)
            {
                ShowMenu();
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": 
                        NewCustomer(); 
                        customersCounter++;
                        CheckDeliveries();
                        break;
                    case "2": 
                        BuyDetails(); 
                        break;
                    case "3": 
                        playing = false; 
                        break;
                }
            }
        }
        
        static void CheckDeliveries()
        {
            for (int i = deliveryQueue.Count - 1; i >= 0; i--)
            {
                deliveryQueue[i].DeliveryIn--;
                
                if (deliveryQueue[i].DeliveryIn <= 0)
                {
                    int detailId = deliveryQueue[i].DetailId;
                    int quantity = deliveryQueue[i].Quantity;
                    
                    if (stock.ContainsKey(detailId))
                        stock[detailId] += quantity;
                    else
                        stock[detailId] = quantity;
                    
                    for (int j = 0; j < quantity; j++)
                    {
                        Core.Context.GarageDetails.Add(new GarageDetail
                        {
                            GarageId = myGarage.Id,
                            DetailsId = detailId
                        });
                    }
                    
                    var detail = allDetails.First(d => d.Id == detailId);
                    Console.WriteLine($"\n[доставка] Получено {quantity} шт. '{detail.Name}'");

                    deliveryQueue.RemoveAt(i);
                }
            }
            
            Core.Context.SaveChanges();
        }
        
        static void ShowMenu()
        {
            Console.Clear();
            Console.WriteLine($"АВТОСЕРВИС");
            Console.WriteLine($"Бюджет: {myGarage.Budget} руб.");
            Console.WriteLine($"Гараж: {myGarage.Name}");
            
            Console.WriteLine("\nСКЛАД");
            ShowStock();
            
            if (deliveryQueue.Count > 0)
            {
                Console.WriteLine("\nОЖИДАЮТСЯ ДОСТАВКИ");
                foreach (var order in deliveryQueue)
                {
                    var detail = allDetails.First(d => d.Id == order.DetailId);
                    Console.WriteLine($"{detail.Name}: {order.Quantity} шт. (через {order.DeliveryIn} клиента(ов))");
                }
            }
            
            Console.WriteLine($"\nКлиентов с последней доставки: {customersCounter}");
            
            Console.WriteLine("\nМЕНЮ");
            Console.WriteLine("1 - Новый клиент");
            Console.WriteLine("2 - Купить запчасти");
            Console.WriteLine("3 - Выйти");
            Console.Write("\nВыберите действие: ");
        }
        
        static void ShowStock()
        {
            if (stock.Count == 0)
            {
                Console.WriteLine("Пусто!");
                return;
            }

            foreach (var item in stock)
            {
                var detail = allDetails.FirstOrDefault(d => d.Id == item.Key);
                if (detail != null)
                    Console.WriteLine($"{detail.Name}: {item.Value} шт.");
            }
        }
        
        static void NewCustomer()
        {
            Console.Clear();

            if (allDetails.Count == 0)
            {
                Console.WriteLine("Нет деталей в базе!");
                WaitForKey();
                return;
            }
            
            var brokenDetail = allDetails[rnd.Next(allDetails.Count)];
            decimal repairCost = brokenDetail.Price * 1.5m;

            Console.WriteLine("НОВЫЙ КЛИЕНТ");
            Console.WriteLine($"Сломано: {brokenDetail.Name}");
            Console.WriteLine($"Стоимость ремонта: {repairCost} руб.");
            
            bool hasDetail = stock.ContainsKey(brokenDetail.Id) && stock[brokenDetail.Id] > 0;

            if (hasDetail)
            {
                HandleRepair(brokenDetail, repairCost);
            }
            else
            {
                HandleNoDetail(brokenDetail, repairCost);
            }

            WaitForKey();
        }
        
        static void HandleRepair(Detail brokenDetail, decimal repairCost)
        {
            Console.Write("\nРемонтировать? (у/т): ");
            string answer = Console.ReadLine();

            if (answer.ToLower() == "у")
            {
                UseDetail(brokenDetail.Id);
                
                UpdateBudget((int)repairCost, true);
                Console.WriteLine($"\nРемонт выполнен! +{repairCost} руб.");
            }
            else
            {
                int fine = (int)(repairCost * 0.3m);
                UpdateBudget(fine, false);
                Console.WriteLine($"\nОтказ. Штраф: {fine} руб.");
            }
        }
        
        static void HandleNoDetail(Detail brokenDetail, decimal repairCost)
        {
            Console.WriteLine("\nНужной детали нет на складе!");
            Console.WriteLine("1 - Отказать (штраф 30%)");
            Console.WriteLine("2 - Поставить другую (штраф 200%)");
            Console.Write("Выберите: ");

            string choice = Console.ReadLine();

            if (choice == "1")
            {
                int fine = (int)(repairCost * 0.3m);
                UpdateBudget(fine, false);
                Console.WriteLine($"\nОтказ. Штраф: {fine} руб.");
            }
            else if (choice == "2")
            {
                if (stock.Count > 0)
                {
                    int wrongDetailId = stock.Keys.First();
                    var wrongDetail = allDetails.First(d => d.Id == wrongDetailId);
                    
                    UseDetail(wrongDetailId);
                    
                    int penalty = (int)(repairCost * 2m);
                    UpdateBudget(penalty, false);
                    
                    Console.WriteLine($"\nПоставили {wrongDetail.Name} вместо {brokenDetail.Name}");
                    Console.WriteLine($"Штраф: {penalty} руб.");
                }
                else
                {
                    Console.WriteLine("\nНа складе совсем нет деталей!");
                }
            }
        }

        static void BuyDetails()
        {
            Console.Clear();
            Console.WriteLine("МАГАЗИН");
            Console.WriteLine("Детали придут через 2 клиента!\n");

            if (allDetails.Count == 0)
            {
                Console.WriteLine("Нет деталей в продаже!");
                WaitForKey();
                return;
            }
            
            ShowCatalog();

            Console.Write("\nВыберите деталь (номер): ");
            if (!int.TryParse(Console.ReadLine(), out int detailNum))
            {
                Console.WriteLine("Введите число");
                WaitForKey();
                return;
            }

            var selectedDetail = allDetails[detailNum - 1];

            Console.Write($"Сколько '{selectedDetail.Name}' купить? ");
            if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity <= 0)
            {
                Console.WriteLine("Неправильное количество!");
                WaitForKey();
                return;
            }
            
            int totalCost = selectedDetail.Price * quantity;
            
            if (myGarage.Budget < totalCost)
            {
                Console.WriteLine($"\nНе хватает денег! Нужно: {totalCost} руб., есть: {myGarage.Budget} руб.");
                WaitForKey();
                return;
            }
            
            BuyDetail(selectedDetail, quantity, totalCost);
            WaitForKey();
        }
        
        static void ShowCatalog()
        {
            for (int i = 0; i < allDetails.Count; i++)
            {
                var detail = allDetails[i];
                int inStock = stock.ContainsKey(detail.Id) ? stock[detail.Id] : 0;
                Console.WriteLine($"{i + 1}. {detail.Name} - {detail.Price} руб. (на складе: {inStock} шт.)");
            }
        }
        
        static void BuyDetail(Detail detail, int quantity, int totalCost)
        {
            UpdateBudget(totalCost, false);
            
            var order = new DeliveryOrder
            {
                DetailId = detail.Id,
                Quantity = quantity,
                DeliveryIn = 2  
            };
            
            deliveryQueue.Add(order);
            
            Console.WriteLine($"\nЗаказано {quantity} шт. '{detail.Name}' за {totalCost} руб.");
            Console.WriteLine($"Детали придут через 2 клиента!");
        }
        
        static void UseDetail(int detailId)
        {
            stock[detailId]--;
            if (stock[detailId] == 0)
                stock.Remove(detailId);
            
            var itemToRemove = Core.Context.GarageDetails
                .FirstOrDefault(gd => gd.GarageId == myGarage.Id && gd.DetailsId == detailId);
            
            if (itemToRemove != null)
                Core.Context.GarageDetails.Remove(itemToRemove);

            Core.Context.SaveChanges();
        }
        
        static void UpdateBudget(int amount, bool isIncome)
        {
            var garageInDb = Core.Context.Garages.First(g => g.Id == myGarage.Id);
            
            if (isIncome)
                garageInDb.Budget += amount;
            else
                garageInDb.Budget -= amount; 

            Core.Context.SaveChanges();
            myGarage.Budget = garageInDb.Budget;
        }
        
        static void WaitForKey()
        {
            Console.WriteLine("\nНажмите любую клавишу...");
            Console.ReadKey();
        }
    }
}