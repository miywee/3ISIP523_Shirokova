using System;
using System.Collections.Generic;
using System.Linq;
using ISIP523_Shirokova;
using ISIP523_Shirokova.Context;
using ISIP523_Shirokova.Entities;

namespace ISIP523_Shirokova
{
    class Program
    {
        private static User currentUser = null;
        static Dictionary<int, int> cart = new Dictionary<int, int>();

        static void Main(string[] args)
        {
            CheckData();
            
            while (true)
            {
                Console.Clear();
                
                if (currentUser == null)
                {
                    Console.WriteLine("МАРКЕТПЛЕЙС");
                    Console.WriteLine("1. Товары");
                    Console.WriteLine("2. Регистрация");
                    Console.WriteLine("3. Вход");
                    Console.WriteLine("4. Выход");
                    Console.Write("Выбор: ");
                    
                    switch (Console.ReadLine())
                    {
                        case "1": 
                            ShowProducts(); 
                            break;
                        case "2": 
                            Register(); 
                            break;
                        case "3": 
                            Login(); 
                            break;
                        case "4": 
                            return;
                    }
                }
                else
                {
                    Console.WriteLine($"Пользователь: {currentUser.Name}");
                    Console.WriteLine("1. Товары");
                    Console.WriteLine("2. Корзина");
                    Console.WriteLine("3. Добавить в корзину");
                    Console.WriteLine("4. Купить");
                    Console.WriteLine("5. Заказы");
                    Console.WriteLine("6. Выйти");
                    Console.Write("Выбор: ");
                    
                    switch (Console.ReadLine())
                    {
                        case "1": 
                            ShowProducts(); 
                            break;
                        case "2": 
                            ShowCart(); 
                            break;
                        case "3": 
                            AddToCart(); 
                            break;
                        case "4": 
                            Buy(); 
                            break;
                        case "5": 
                            ShowOrders(); 
                            break;
                        case "6": 
                            currentUser = null; 
                            cart.Clear(); 
                            break;
                    }
                }
            }
        }
        
        static void CheckData()
        {
            if (!Core.Context.Products.Any())
            {
                Core.Context.Products.AddRange(new List<Product>
                {
                    new Product { Name = "Телефон", Price = 50000, Stock = 10 },
                    new Product { Name = "Ноутбук", Price = 80000, Stock = 5 },
                    new Product { Name = "Наушники", Price = 10000, Stock = 20 }
                });
            }
            
            if (!Core.Context.Points.Any())
            {
                Core.Context.Points.AddRange(new List<Point>
                {
                    new Point { Name = "ПВЗ 1", Address = "Адрес 1" },
                    new Point { Name = "ПВЗ 2", Address = "Адрес 2" },
                    new Point { Name = "ПВЗ 3", Address = "Адрес 3" }
                });
            }
            
            Core.Context.SaveChanges();
        }
        
        static void ShowProducts()
        {
            Console.Clear();
            Console.WriteLine("ТОВАРЫ:");
            
            var products = Core.Context.Products.ToList();
            foreach (var p in products)
            {
                Console.WriteLine($"{p.Id}. {p.Name} - {p.Price} руб. (осталось: {p.Stock})");
            }
            
            Wait();
        }
        
        static void Register()
        {
            Console.Clear();
            Console.WriteLine("РЕГИСТРАЦИЯ");
            
            Console.Write("Имя: ");
            string name = Console.ReadLine();
            
            Console.Write("Email: ");
            string email = Console.ReadLine();
            
            Console.Write("Пароль: ");
            string pass1 = Console.ReadLine();
            
            Console.Write("Повтор: ");
            string pass2 = Console.ReadLine();
            
            if (pass1 != pass2)
            {
                Console.WriteLine("Пароли не совпадают!");
                Wait();
                return;
            }
            
            if (Core.Context.Users.Any(u => u.Email == email))
            {
                Console.WriteLine("Email уже используется!");
                Wait();
                return;
            }
            
            Core.Context.Users.Add(new User
            {
                Name = name,
                Email = email,
                Password = pass1
            });
            
            Core.Context.SaveChanges();
            Console.WriteLine("Успешно!");
            Wait();
        }
        
        static void Login()
        {
            Console.Clear();
            Console.WriteLine("ВХОД");
            
            Console.Write("Email: ");
            string email = Console.ReadLine();
            
            Console.Write("Пароль: ");
            string password = Console.ReadLine();
            
            var user = Core.Context.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
            
            if (user == null)
            {
                Console.WriteLine("Неверные данные!");
                Wait();
                return;
            }
            
            currentUser = user;

            cart.Clear();
            var cartItems = Core.Context.ProductCarts.Where(pc => pc.UserId == user.Id).ToList();
            foreach (var item in cartItems)
            {
                cart[item.ProductId] = item.Quantity;
            }
            
            Console.WriteLine($"Добро пожаловать, {user.Name}!");
            Wait();
        }
        
        static void ShowCart()
        {
            Console.Clear();
            Console.WriteLine("КОРЗИНА");
            
            if (cart.Count == 0)
            {
                Console.WriteLine("Пусто");
                Wait();
                return;
            }
            
            double total = 0;
            foreach (var item in cart)
            {
                var product = Core.Context.Products.FirstOrDefault(p => p.Id == item.Key);
                if (product != null)
                {
                    double sum = product.Price * item.Value;
                    total += sum;
                    Console.WriteLine($"{product.Name}: {item.Value} шт. = {sum} руб.");
                }
            }
            
            Console.WriteLine($"ИТОГО: {total} руб.");
            Wait();
        }
        
        static void AddToCart()
        {
            ShowProducts();
            
            Console.Write("\nID товара: ");
            if (!int.TryParse(Console.ReadLine(), out int id)) return;
            
            var product = Core.Context.Products.FirstOrDefault(p => p.Id == id);
            if (product == null) return;
            
            Console.Write("Количество: ");
            if (!int.TryParse(Console.ReadLine(), out int qty) || qty < 1) return;
            
            if (product.Stock < qty)
            {
                Console.WriteLine($"Недостаточно! В наличии: {product.Stock}");
                Wait();
                return;
            }
 
            if (cart.ContainsKey(id))
                cart[id] += qty;
            else
                cart[id] = qty;
            
            var cartItem = Core.Context.ProductCarts
                .FirstOrDefault(pc => pc.UserId == currentUser.Id && pc.ProductId == id);
            
            if (cartItem != null)
                cartItem.Quantity = cart[id];
            else
                Core.Context.ProductCarts.Add(new ProductCart
                {
                    UserId = currentUser.Id,
                    ProductId = id,
                    Quantity = qty
                });
            
            Core.Context.SaveChanges();
            
            Console.WriteLine("Добавлено!");
            Wait();
        }
        
        static void Buy()
        {
            Console.Clear();
            Console.WriteLine("ЧТО КУПИТЬ?");
            Console.WriteLine("1. Один товар");
            Console.WriteLine("2. Все из корзины");
            Console.Write("Выбор: ");
            
            if (Console.ReadLine() == "1")
                BuySingle();
            else
                BuyCart();
        }
        
        static void BuySingle()
        {
            ShowProducts();
            
            Console.Write("\nID товара: ");
            if (!int.TryParse(Console.ReadLine(), out int id)) return;
            
            var product = Core.Context.Products.FirstOrDefault(p => p.Id == id);
            if (product == null) return;
            
            Console.Write("Количество: ");
            if (!int.TryParse(Console.ReadLine(), out int qty) || qty < 1) return;
            
            if (product.Stock < qty)
            {
                Console.WriteLine($"Недостаточно! В наличии: {product.Stock}");
                Wait();
                return;
            }
            
            CreateOrder(new Dictionary<int, int> { { id, qty } });
        }
        
        static void BuyCart()
        {
            if (cart.Count == 0)
            {
                Console.WriteLine("Корзина пуста!");
                Wait();
                return;
            }
            
            CreateOrder(cart);
        }
        
        static void CreateOrder(Dictionary<int, int> items)
        {
            var points = Core.Context.Points.ToList();
            Console.WriteLine("\nПВЗ:");
            foreach (var p in points)
            {
                Console.WriteLine($"{p.Id}. {p.Name}");
            }
            
            Console.Write("ID ПВЗ: ");
            if (!int.TryParse(Console.ReadLine(), out int pointId)) return;
            
            double total = 0;
            foreach (var item in items)
            {
                var product = Core.Context.Products.FirstOrDefault(p => p.Id == item.Key);
                if (product != null)
                    total += product.Price * item.Value;
            }
            
            Console.WriteLine($"\nСумма: {total} руб.");
            Console.Write("Оформить? (да/нет): ");
            
            if (Console.ReadLine().ToLower() != "да") return;
            
            var order = new Order
            {
                UserId = currentUser.Id,
                Date = DateOnly.FromDateTime(DateTime.Now),
                Amount = total,
                PointId = pointId
            };
            
            Core.Context.Orders.Add(order);
            Core.Context.SaveChanges();
            
            foreach (var item in items)
            {
                var product = Core.Context.Products.FirstOrDefault(p => p.Id == item.Key);
                if (product != null)
                {
                    product.Stock -= item.Value;
                    Core.Context.OrdersProducts.Add(new OrdersProduct
                    {
                        OrderId = order.Id,
                        ProductId = product.Id,
                        Count = item.Value
                    });
                }
            }
            
            if (items == cart)
            {
                var userCart = Core.Context.ProductCarts.Where(pc => pc.UserId == currentUser.Id).ToList();
                Core.Context.ProductCarts.RemoveRange(userCart);
                cart.Clear();
            }
            
            Core.Context.SaveChanges();
            
            Console.WriteLine($"Заказ №{order.Id} оформлен!");
            Wait();
        }
        
        static void ShowOrders()
        {
            Console.Clear();
            Console.WriteLine("ВАШИ ЗАКАЗЫ:");
            
            var orders = Core.Context.Orders
                .Where(o => o.UserId == currentUser.Id)
                .OrderByDescending(o => o.Date)
                .ToList();
            
            if (orders.Count == 0)
            {
                Console.WriteLine("Нет заказов");
                Wait();
                return;
            }
            
            foreach (var order in orders)
            {
                Console.WriteLine($"\nЗаказ #{order.Id} от {order.Date}");
                Console.WriteLine($"Сумма: {order.Amount} руб.");
                
                var items = Core.Context.OrdersProducts.Where(op => op.OrderId == order.Id).ToList();
                foreach (var item in items)
                {
                    var product = Core.Context.Products.FirstOrDefault(p => p.Id == item.ProductId);
                    if (product != null)
                        Console.WriteLine($"  {product.Name}: {item.Count} шт.");
                }
            }
            
            Wait();
        }
        
        static void Wait()
        {
            Console.WriteLine("\nНажмите Enter...");
            Console.ReadLine();
        }
    }
}