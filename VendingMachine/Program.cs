using System;
using System.Collections.Generic;

public class VendingMachine
{
    private Dictionary<string, decimal> acceptedCoins = new()
    {
        { "nickel", 0.05m },
        { "dime", 0.10m },
        { "quarter", 0.25m }
    };

    private Dictionary<string, decimal> productPrices = new()
    {
        { "cola", 1.00m },
        { "chips", 0.50m },
        { "candy", 0.65m }
    };

    private decimal currentAmount = 0.0m;
    private string lastMessage = "INSERT COIN";
    private List<string> coinReturn = new();

    public void Start()
    {
        while (true)
        {

            Console.WriteLine($"Display: {GetDisplay()}");

            Console.WriteLine("Available Products:");
            foreach (var item in productPrices)
            {
                Console.WriteLine($"- {item.Key} : {item.Value:C}");
            }

            Console.WriteLine("Accepted Coins:");
            foreach (var coin in acceptedCoins)
            {
                Console.WriteLine($"- {coin.Key} : {coin.Value:C}");
            }

            Console.WriteLine("\nEnter coin name or product name: ");
            string input = Console.ReadLine()?.Trim().ToLower();

            if (input == "exit")
                break;

            if (acceptedCoins.ContainsKey(input) || input == "penny")
            {
                InsertCoin(input);
            }
            else if (productPrices.ContainsKey(input))
            {
                SelectProduct(input);
            }
            else
            {
                Console.WriteLine("Invalid input.");
            }
        }
    }


    private void InsertCoin(string coin)
    {
        if (acceptedCoins.TryGetValue(coin, out decimal value))
        {
            currentAmount += value;
            lastMessage = $"{currentAmount:C}";
        }
        else
        {
            coinReturn.Add(coin);
            Console.WriteLine($"Rejected coin returned: {coin}");
        }
    }

    private void SelectProduct(string product)
    {
        decimal price = productPrices[product];
        if (currentAmount >= price)
        {
            currentAmount -= price;
            lastMessage = "THANK YOU";
        }
        else
        {
            lastMessage = $"PRICE {price:C}";
        }
    }

    private string GetDisplay()
    {
        string message = lastMessage;

        if (message == "THANK YOU")
        {
            lastMessage = currentAmount > 0 ? $"{currentAmount:C}" : "INSERT COIN";
        }
        else if (message.StartsWith("PRICE"))
        {
            lastMessage = currentAmount > 0 ? $"{currentAmount:C}" : "INSERT COIN";
        }

        return message;
    }
}

public class Program
{
    static void Main(string[] args)
    {
        VendingMachine machine = new VendingMachine();
        machine.Start();
    }
}
