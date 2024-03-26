using System.Globalization;
using nietras.SeparatedValues;
using Spectre.Console;

var items = new Dictionary<string, Item>();
var orders = new Dictionary<int, Order>();
var rewardCards = new Dictionary<string, RewardsCard>();
var customers = new Dictionary<string, Customer>();

using var menuReader = Sep.Reader().FromFile("./Menu.csv");
using var customerReader = Sep.Reader().FromFile("./RestaurantCustomers.csv");
using var rewardCardsReader = Sep.Reader().FromFile("./RewardCard.csv");

foreach (var row in menuReader)
{
    items[row["Name"].ToString()] = new Item(
        name: row["Name"].ToString(),
        quantity: 0,
        price: row["Price"].Parse<double>()
    );
}

foreach (var row in customerReader)
{
    customers[row["CustomerID"].ToString()] = new Customer(
        customerId: row["CustomerID"].ToString(),
        name: row["Name"].ToString()
    );
}

foreach (var row in rewardCardsReader)
{
    rewardCards[row["CustomerID"].ToString()] = new RewardsCard(
        customerId: row["CustomerID"].ToString(),
        points: row["Points"].Parse<int>()
    );
}

while (true)
{
    var option = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .Title("Choose an [green]option[/]?")
            .AddChoices([
                "Create customer",
                "Create order",
                "Add item",
                "Remove item",
                "Checkout order",
                "Exit"
            ]));

    switch (option)
    {
        case "Create customer":
        {
            var id = AnsiConsole.Ask<string>("Customer ID");
            var name = AnsiConsole.Ask<string>("Customer Name");
            customers[id] = new Customer(id, name);
            break;
        }
        case "Create order":
        {
            var id = AnsiConsole.Ask<string>("Customer ID");
            if (!customers.ContainsKey(id))
            {
                AnsiConsole.MarkupLine("[red]Customer does not exist.[/]");
                continue;
            }

            var orderId = orders.Max((kv) => kv.Value.OrderId) + 1;
            orders[orderId] = new Order(orderId, id, []);

            AnsiConsole.MarkupLine($"Created order [bold green]{orderId}[/]!");
            break;
        }
        case "Add item":
        {
            var table = new Table();
            table.AddColumn("Name");
            table.AddColumn("Quantity");
            table.AddColumn("Price");

            foreach (var (_, item) in items)
            {
                table.AddRow(item.Name, item.Quantity.ToString(), item.Price.ToString(CultureInfo.InvariantCulture));
            }

            AnsiConsole.Write(table);

            var id = AnsiConsole.Ask<int>("Order ID");
            if (!orders.TryGetValue(id, out var order))
            {
                AnsiConsole.MarkupLine("[red]Order does not exist.[/]");
                continue;
            }

            var selectedItem = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("What's your chosen [green]item[/]?")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to reveal more items)[/]")
                    .AddChoices(items.Select(i => i.Key)));

            var quantity = AnsiConsole.Ask<int>("How many items?");

            var i = new Item(
                name: items[selectedItem].Name,
                quantity: quantity,
                price: items[selectedItem].Price
            );

            order.Items.Add(i);

            AnsiConsole.MarkupLine($"[bold]Subtotal:[/] {i.Subtotal}");

            break;
        }

        case "Remove item":
        {
            var id = AnsiConsole.Ask<int>("Order ID");
            if (!orders.TryGetValue(id, out var order))
            {
                AnsiConsole.MarkupLine("[red]Order does not exist.[/]");
                continue;
            }

            var table = new Table();
            table.AddColumn("Name");
            table.AddColumn("Quantity");
            table.AddColumn("Price");

            foreach (var item in order.Items)
            {
                table.AddRow(item.Name, item.Quantity.ToString(), item.Price.ToString(CultureInfo.InvariantCulture));
            }

            AnsiConsole.Write(table);

            // TODO LOL I give up no more time

            break;
        }
    }
}

class Customer(string customerId, string name);

class Item(string name, int quantity, double price)
{
    public string Name => name;
    public int Quantity => quantity;
    public double Price => price;
    public double Subtotal => quantity * price;
}

class Order(int orderId, string customerId, List<Item> items)
{
    public int OrderId => orderId;
    public string CustomerId => customerId;
    public List<Item> Items => items;
    public double TotalAmount => items.Aggregate(0.0, (acc, item) => acc + item.Subtotal);
}

class RewardsCard(string customerId, int points);