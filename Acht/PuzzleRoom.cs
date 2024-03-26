using Spectre.Console;

namespace Acht;

public class PuzzleRoom() : Room("Puzzle Room", "Very fun puzzle room")
{
    public override bool EnterRoom()
    {
        Console.WriteLine();
        AnsiConsole.MarkupLine("[bold yellow]Welcome to the room of doom PART TWO HAHAHAHA[/]");
        Console.WriteLine();

        if (AnsiConsole.Ask<int>("7+3") != 10)
        {
            return false;
        }
        
        if (AnsiConsole.Ask<int>("8*2") != 16)
        {
            return false;
        }
        
        if (AnsiConsole.Ask<int>("19-2") != 17)
        {
            return false;
        }
        
        Console.WriteLine();
        AnsiConsole.MarkupLine("[bold italic blue]You survived the room of doom PART TWO HAHAHAHAHA. Here's your blue key. Proceed.[/]");
        Console.WriteLine();

        return true;
    }
}