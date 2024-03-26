using Spectre.Console;

namespace Acht;

public class AdditionalRoom() : Room("Additional Room", "Very fun additional room")
{
    public override bool EnterRoom()
    {
        Console.WriteLine();
        AnsiConsole.MarkupLine("[bold red]Sike you're still trapped. Solve to escape.[/]");
        Console.WriteLine();

        var answer = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(
                    "Qin Guan is cool"
                )
                .PageSize(3)
                .AddChoices([
                    "Yes", "Very", "Extremely"
                ])
        );

        Console.WriteLine();
        AnsiConsole.MarkupLine("Okay you can go now.");
        Console.WriteLine();

        return true;
    }
}