// See https://aka.ms/new-console-template for more information

using System.Globalization;
using nietras.SeparatedValues;
using Spectre.Console;

using var reader = Sep.Reader().FromFile("VehicleData.csv");

var vehicles = new List<Vehicle>();
foreach (var row in reader)
{
    if (row["MaxCapacity"].ToString().Length > 0)
    {
        vehicles.Add(new Truck
        {
            Brand = row["Brand"].ToString(),
            Model = row["Model"].ToString(),
            Year = row["Year"].Parse<int>(),
            Price = row["Price"].Parse<decimal>(),
            MaxCapacity = row["MaxCapacity"].Parse<int>(),
            TowingCapacity = row["TowingCapacity"].Parse<int>(),
        });
    }
    else if (row["Seater"].ToString().Length > 0)
    {
        vehicles.Add(new Car
        {
            Brand = row["Brand"].ToString(),
            Model = row["Model"].ToString(),
            Year = row["Year"].Parse<int>(),
            Price = row["Price"].Parse<decimal>(),
            Seater = row["Seater"].Parse<int>(),
            NumDoors = row["NumDoors"].Parse<int>(),
        });
    }
    else if (row["Type"].ToString().Length > 0)
    {
        vehicles.Add(new Motorcycle
        {
            Brand = row["Brand"].ToString(),
            Model = row["Model"].ToString(),
            Year = row["Year"].Parse<int>(),
            Price = row["Price"].Parse<decimal>(),
            Type = row["Type"].ToString()
        });
    }
}

var vehicle = AnsiConsole.Prompt(
    new SelectionPrompt<string>()
        .Title("Select a type of [green]vehicle[/]?")
        .AddChoices([
            "Truck",
            "Car",
            "Motorcycle"
        ]));

var selectedVehicles = vehicles.Where(v =>
{
    return vehicle switch
    {
        "Truck" => v is Truck,
        "Car" => v is Car,
        "Motorcycle" => v is Motorcycle,
        _ => throw new Exception("Invalid vehicle type")
    };
}).ToList();

var table = new Table();

table.AddColumn("No.");
table.AddColumn("Brand");
table.AddColumn("Model");
table.AddColumn("Year");
table.AddColumn("Price");

switch (vehicle)
{
    case "Truck":
        table.AddColumn("Max Capacity");
        table.AddColumn("Towing Capacity");
        break;
    case "Car":
        table.AddColumn("Seater");
        table.AddColumn("Num Doors");
        break;
    case "Motorcycle":
        table.AddColumn("Type");
        break;
}

foreach (var (v, idx) in selectedVehicles.Select((value, idx) => (value, idx)))
{
    switch (v)
    {
        case Truck t:
            table.AddRow(idx.ToString(), t.Brand, t.Model, t.Year.ToString(),
                t.Price.ToString(CultureInfo.InvariantCulture),
                t.MaxCapacity.ToString(), t.TowingCapacity.ToString());
            break;
        case Car c:
            table.AddRow(idx.ToString(), c.Brand, c.Model, c.Year.ToString(),
                c.Price.ToString(CultureInfo.InvariantCulture),
                c.Seater.ToString(), c.NumDoors.ToString());
            break;
        case Motorcycle m:
            table.AddRow(idx.ToString(), m.Brand, m.Model, m.Year.ToString(),
                m.Price.ToString(CultureInfo.InvariantCulture), m.Type);
            break;
    }
}

AnsiConsole.Write(table);

var selectedVehicle = AnsiConsole.Prompt(
    new SelectionPrompt<string>()
        .Title("Select your vehicle [green]number[/]")
        .AddChoices(
            selectedVehicles.Select((_, idx) => idx.ToString())
        ));

var selectedVehicleIdx = int.Parse(selectedVehicle);

AnsiConsole.WriteLine("Performing maintenance");

selectedVehicles[selectedVehicleIdx].RegularMaintenance().GetAwaiter().GetResult();

AnsiConsole.WriteLine("Performing repair ");

selectedVehicles[selectedVehicleIdx].Repair().GetAwaiter().GetResult();

AnsiConsole.WriteLine("Done! Have a nice day!");

interface IMaintenance
{
    public Task RegularMaintenance();
    public Task Repair();
}

abstract class Vehicle : IMaintenance
{
    public string Brand { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }
    public decimal Price { get; set; }

    public abstract Task RegularMaintenance();
    public abstract Task Repair();
}

class Car : Vehicle
{
    public int Seater { get; set; }
    public int NumDoors { get; set; }

    public override async Task RegularMaintenance()
    {
        await AnsiConsole.Progress()
            .StartAsync(async ctx =>
            {
                var task1 = ctx.AddTask("[green]Inspecting battery[/]");
                var task2 = ctx.AddTask("[green]Checking fluids[/]");
                var task3 = ctx.AddTask("[green]Inspecting suspension and steering components[/]");

                while (!ctx.IsFinished)
                {
                    await Task.Delay(25);
                    task1.Increment(1.5);
                    task2.Increment(0.5);
                    task3.Increment(0.3);
                }
            });
    }

    public override async Task Repair()
    {
        await AnsiConsole.Progress()
            .StartAsync(async ctx =>
            {
                var task1 = ctx.AddTask("[green]Repairing tire rotation[/]");
                var task2 = ctx.AddTask("[green]Repairing coolant flush[/]");

                while (!ctx.IsFinished)
                {
                    await Task.Delay(25);
                    task1.Increment(1.5);
                    task2.Increment(0.5);
                }
            });
    }
}

class Motorcycle : Vehicle
{
    public string Type { get; set; }

    public override async Task RegularMaintenance()
    {
        await AnsiConsole.Progress()
            .StartAsync(async ctx =>
            {
                var task1 = ctx.AddTask("[green]Inspecting lights and signals[/]");
                var task2 = ctx.AddTask("[green]Battery maintenance[/]");

                while (!ctx.IsFinished)
                {
                    await Task.Delay(25);
                    task1.Increment(1.5);
                    task2.Increment(0.5);
                }
            });
    }

    public override async Task Repair()
    {
        await AnsiConsole.Progress()
            .StartAsync(async ctx =>
            {
                var task1 = ctx.AddTask("[green]Changing filter[/]");
                var task2 = ctx.AddTask("[green]Changing spark plug[/]");

                while (!ctx.IsFinished)
                {
                    await Task.Delay(25);
                    task1.Increment(1.5);
                    task2.Increment(0.5);
                }
            });
    }
}

class Truck : Vehicle
{
    public int MaxCapacity { get; set; }
    public int TowingCapacity { get; set; }

    public override async Task RegularMaintenance()
    {
        await AnsiConsole.Progress()
            .StartAsync(async ctx =>
            {
                var task1 = ctx.AddTask("[green]Checking exhaust system[/]");
                var task2 = ctx.AddTask("[green]Checking coolant levels[/]");

                while (!ctx.IsFinished)
                {
                    await Task.Delay(25);
                    task1.Increment(1.5);
                    task2.Increment(0.5);
                }
            });
    }

    public override async Task Repair()
    {
        await AnsiConsole.Progress()
            .StartAsync(async ctx =>
            {
                var task1 = ctx.AddTask("[green]Servicing transmission[/]");
                var task2 = ctx.AddTask("[green]Changing oil[/]");

                while (!ctx.IsFinished)
                {
                    await Task.Delay(25);
                    task1.Increment(1.5);
                    task2.Increment(0.5);
                }
            });
    }
}