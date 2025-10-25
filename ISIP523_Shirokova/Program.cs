using System;

public enum Category
{
    Clothing = 1,
    Food = 2,
    Books = 3
}

public class Product
{
    public string code;
    public string name;
    public double price;
    public int quantity;
    public bool inStock;
    public Category category;

    public Product(string code, string name, double price, int quantity, Category category)
    {
        this.code = code; 
        this.name = name;
        this.price = price;
        this.quantity = quantity;
        this.inStock = quantity > 0;
        this.category = category;
    }

    public string ToString()
    {
        return $"{code}, {name}, Цена: {price} руб, Количество: {quantity}, В наличии: {(inStock ? "Да" : "Нет")}, Категория: {category}";
    }
}

public class Store
{
    private Product[] products = new Product[100];
    private int productCount = 0;
    private int nextProductId = 1;

    public Store()
    {
        AddTestProduct("Футболка", 600, 25, Category.Clothing);
        AddTestProduct("Джинсы", 1200, 15, Category.Clothing);
        AddTestProduct("Яблоко", 30, 30, Category.Food);
        AddTestProduct("Молоко", 70, 20, Category.Food);
        AddTestProduct("Фэнтези", 450, 10, Category.Books);
    }
    private void AddTestProduct(string name, double price, int quantity, Category category)
    {
        string code = nextProductId.ToString();
        nextProductId++;
        products[productCount] = new Product(code, name, price, quantity, category);
        productCount++;
    }
    
    public void ShowAllProducts()
    {
        Console.WriteLine("\nВсе товары:");
        for (int i = 0; i < productCount; i++)
        {
            Console.WriteLine(products[i]);
        }
    }
    
    public void AddProduct(string name, double price, int quantity, Category category)
    {
        string code = nextProductId.ToString();
        nextProductId++;
        
        products[productCount] = new Product(code, name, price, quantity, category);
        productCount++;
        Console.WriteLine("Товар добавлен!");
    }
    
    public void RemoveProduct(string code)
    {
        for (int i = 0; i < productCount; i++)
        {
            if (products[i].code == code)
            {
                for (int j = i; j < productCount - 1; j++)
                {
                    products[j] = products[j + 1];
                }

                productCount--;
                Console.WriteLine("Товар удален!");
                return;
            }
        }

        Console.WriteLine("Товар не найден!");
    }
    
    public void OrderSupply(string code, int quantity)
    {
        for (int i = 0; i < productCount; i++)
        {
            if (products[i].code == code)
            {
                products[i].quantity += quantity;
                products[i].inStock = true;
                Console.WriteLine("Поставка добавлена!");
                return;
            }
        }
        Console.WriteLine("Товар не найден!");
    }
    
    public void SellProduct(string code, int quantity)
    {
        for (int i = 0; i < productCount; i++)
        {
            if (products[i].code == code)
            {
                if (products[i].quantity >= quantity)
                {
                    products[i].quantity -= quantity;
                    products[i].inStock = products[i].quantity > 0;
                    Console.WriteLine("Продажа завершена!");
                }
                else
                {
                    Console.WriteLine("Недостаточно товара!");
                }
                return;
            }
        }
        Console.WriteLine("Товар не найден!");
    }
    
    public void SearchByCode(string code)
    {
        for (int i = 0; i < productCount; i++)
        {
            if (products[i].code == code)
            {
                Console.WriteLine("Найден товар: " + products[i]);
                return;
            }
        }
        Console.WriteLine("Товар не найден!");
    }
    
    public void SearchByName(string name)
    {
        Console.WriteLine("\nРезультаты поиска:");
        bool found = false;
        for (int i = 0; i < productCount; i++)
        {
            if (products[i].name.ToLower().Contains(name.ToLower()))
            {
                Console.WriteLine(products[i]);
                found = true;
            }
        }
        if (!found) Console.WriteLine("Товары не найдены!");
    }

    public void SearchByCategory(Category category)
    {
        Console.WriteLine("\nТовары в категории " + category + ":");
        bool found = false;
        for (int i = 0; i < productCount; i++)
        {
            if (products[i].category == category)
            {
                Console.WriteLine(products[i]);
                found = true;
            }
        }
        if (!found) Console.WriteLine("Товары не найдены!");
    }
    
}