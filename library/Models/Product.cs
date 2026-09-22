namespace library.Models;

public abstract class Product
{
    private decimal _price;
    private int _stock;
    private decimal _discountPercentage;

    public int Id { get; internal set; }

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
                throw new ArgumentException(
                    "Price cannot be negative."
                );
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
                throw new ArgumentException(
                    "Stock cannot be negative."
                );
            }

            _stock = value;
        }
    }

    public decimal DiscountPercentage
    {
        get
        {
            return _discountPercentage;
        }
        set
        {
            if (value < 0 || value > 100)
            {
                throw new ArgumentException(
                    "Discount percentage must be between 0 and 100."
                );
            }

            _discountPercentage = value;
        }
    }

    public abstract string GetDetails();
}