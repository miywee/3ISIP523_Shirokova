using System;

public enum Category
{
    Clothing,
    Food,
    Books
}

public class Product
{
    public string code;
    public string name;
    public double price;
    public int quantity;
    public bool inStock;
    public Category category;

    public Product(string code, string name, double price, int quantity, bool inStock, Category category)
    {
        this.code = code; 
        this.name = name;
        this.price = price;
        this.quantity = quantity;
        this.inStock = quantity > 0;
        this.category = category;
    }

    public string Show()
    {
        return code + " " + name + " " + price + " руб";
    }
}

public class Store
{
    private Product[] products = new Product[100];
    private int productCount = 0;
    private int nextProductId = 1;
}

