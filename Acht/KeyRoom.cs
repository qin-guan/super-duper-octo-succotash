using Spectre.Console;

namespace Acht;

public class KeyRoom() : Room("Key Room", "Very fun key room")
{
    public override bool EnterRoom()
    {
        Console.WriteLine();
        AnsiConsole.MarkupLine("[bold red]Welcome to the room of CONFUSION MUAHAHA[/]");
        Console.WriteLine();

        var answer = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(
                    "What do you [bold]throw out[/] when you want to use it, but [italic]take in[/] when you don't want to use it?"
                )
                .PageSize(3)
                .AddChoices([
                    "Anchor", "Tree", "Fish"
                ])
        );

        if (answer is not "Anchor")
        {
            return false;
        }

        Console.WriteLine();
        AnsiConsole.MarkupLine(
            "[bold italic green]You survived the room of CONFUSION MUAHAHA. Here's your green key. Proceed.[/]");
        Console.WriteLine();

        return true;
    }
}