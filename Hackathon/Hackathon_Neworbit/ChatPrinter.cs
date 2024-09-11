namespace Hackathon_Neworbit;

public static class ChatPrinter
{
    private static void WriteColor(string text, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(text);
        Console.ForegroundColor = ConsoleColor.Gray;
    }

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
        WriteColor(text, ConsoleColor.Red);
    }

    public static void PrintDependency(string text)
    {
        WriteColor(text, ConsoleColor.Cyan);
    }

    public static void PrintInfo(string text)
    {
        WriteColor(text, ConsoleColor.DarkGray);
    }
}
