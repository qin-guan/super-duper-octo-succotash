using Spectre.Console;

namespace Acht;

public class StartingRoom() : Room("Starting Room", "Very fun starting room")
{
    public override bool EnterRoom()
    {
        Console.WriteLine();
        AnsiConsole.MarkupLine(
            "[bold red]Welcome to the room of doom.[/]\nAnswer these questions if you want to live."
        );
        Console.WriteLine();

        if (AnsiConsole.Confirm("Penguins live in the arctic"))
        {
            return false;
        }

        if (!AnsiConsole.Confirm("Influenza is a virus"))
        {
            return false;
        }

        if (!AnsiConsole.Confirm("Roald Dahl is dead"))
        {
            return false;
        }

        Console.WriteLine();
        AnsiConsole.MarkupLine(
            "[bold italic yellow]You survived the room of doom. Here's your yellow key. Proceed.[/]");
        Console.WriteLine();

        return true;
    }
}