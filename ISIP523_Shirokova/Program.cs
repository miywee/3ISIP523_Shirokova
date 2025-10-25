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

    public override string ToString()
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
    
    
    
}