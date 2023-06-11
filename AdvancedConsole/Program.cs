Task.Run(() =>
{
    while (true)
    {
        //AdvancedConsole.WriteLine("==========================");
        Thread.Sleep(1000);
    }
});
var tmp = Console.OpenStandardInput();

while (true)
{
    string input = AdvancedConsole.ReadLine();
}
public static class AdvancedConsole
{
    public static string Buffer = string.Empty;
    public static string InputStart = ">>";
    public static (int Left, int Top) InputStartPos;
    public static bool IsInputing = false;
    public static Stream inputStream = Console.OpenStandardInput();
    public static TextReader In = TextReader.Synchronized(inputStream == Stream.Null ?
                StreamReader.Null :
                new StreamReader(
                    stream: inputStream,
                    encoding: Console.InputEncoding,
                    bufferSize: 1,
                    leaveOpen: true)
                {
                    
                });
    public static void WriteLine(dynamic text)
    {
        if (IsInputing)
        {
            Console.SetCursorPosition(InputStartPos.Left, InputStartPos.Top);
            for (int i = 0; i < Buffer.Length + InputStart.Length; i++)
            {
                Console.Write(' ');
            }
            Console.SetCursorPosition(InputStartPos.Left, InputStartPos.Top);
        }
        Console.WriteLine(text);
        if (IsInputing)
        {
            InputStartPos = Console.GetCursorPosition();
        }
        Console.Write(InputStart);
        if (IsInputing)
            Console.Write(Buffer);
    }
    public static string ReadLine()
    {
        
        InputStartPos = Console.GetCursorPosition();
        IsInputing = true;
        Console.Write(InputStart);
        while (true)
        {
            var tmp = In.Read();
            if ((char)tmp == '\n')
            {
                break;
            }
            else if ((char)tmp == '\b' && Buffer.Length > 0)
            {
                Buffer = Buffer.Substring(0, Buffer.Length - 1);
            }
            else if ((char)tmp == '\b' && Buffer.Length == 0)
            {
                Console.Write(InputStart[InputStart.Length - 1]);
            }
            else if ((char)tmp != '\b')
            {
                Buffer += ((char)tmp).ToString();
            }
        }
        string tmp2 = Buffer;
        Buffer = string.Empty;
        IsInputing = false;
        return tmp2;
    }
}
