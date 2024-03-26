using Acht;
using Spectre.Console;

AssertRoomSuccess(new StartingRoom());
AssertRoomSuccess(new PuzzleRoom());
AssertRoomSuccess(new KeyRoom());

AnsiConsole.Confirm("[bold italic green]Congrats, you made it out alive! Do you accept your success?[/]");

AssertRoomSuccess(new AdditionalRoom());

AnsiConsole.MarkupLine("[italic green]Congrats, you made it out alive! Bye bye![/]");

return;

void AssertRoomSuccess(Room room)
{
    var success = room.EnterRoom();
    if (success) return;
    
    AnsiConsole.MarkupLine("[bold red]Oops you died. Bye bye![/]");
    Environment.Exit(0);
}