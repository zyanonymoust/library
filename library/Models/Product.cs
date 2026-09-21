namespace library.Models;

public abstract class Product
{
    private decimal _price;

    private int _stock;

    public int Id { get; set; }

    public string Title { get; set; } = "";

    public decimal Price
    {
        get
        {
            return _price;
        }
        set
        {
            if (value < 0)
            { 
                throw new ArgumentException("Price cannot be negative.");
            }
            _price = value;
        }
    }

    public int Stock
    {
        get
        {
            return _stock;
        }
        set
        {
            if (value < 0)
            {
                throw new ArgumentException("Stock cannot be negative.");
            }
            _stock = value;
        }
    }
    public abstract string GetDetails();
}