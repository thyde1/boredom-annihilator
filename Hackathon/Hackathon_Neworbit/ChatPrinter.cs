namespace Hackathon_Neworbit;

public static class ChatPrinter
{
    public static void PrintPrompt()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("> ");
        Console.ForegroundColor = ConsoleColor.Gray;
    }

    public static string GetUserInput()
    {
        PrintPrompt();
        Console.ForegroundColor = ConsoleColor.Green;
        var input =  Console.ReadLine()!;
        Console.ForegroundColor = ConsoleColor.Gray;
        return input;
    }

    public static void PrintExclamation(string text)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(text);
        Console.ForegroundColor = ConsoleColor.Gray;
    }
}
