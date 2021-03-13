using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

interface IGambleGame
{
    string GetName { get; }
    string GetDescription { get; }
    void MainLoop();
    Dictionary<Key, Action> Actions { get; }
    void InitializeActions();
    void GameStart();
}

class Gambling
{
    public static void SetClipboard(string toSet)
    {
        try
        {
            Clipboard.SetText(toSet);
        }
        catch (Exception e)
        {
            Console.WriteLine("Caught: " + e.Message);
        }
    }

    public static string FormatMoney(int money)
    {
        return string.Format(new CultureInfo("en-US"), "{0:C0}", money);
    }

    public static string FormatMoney(string money)
    {
        return string.Format(new CultureInfo("en-US"), "{0:C0}", money);
    }

    [STAThread]
    static void Main(string[] args)
    {
        List<IGambleGame> games = new List<IGambleGame>()
        {
            new Blackjack(),
            new Dice(),
            new DoubleOrNothing(),
            new Roulette(),
            new ScratchCards(),
            new MysteryBox(),
            new VideoPoker(),
            new Craps(),
        };
        IGambleGame selectedGame = null;

        for (int i = 0; i < games.Count; i++)
        {
            Console.Write(i + ": " + games[i].GetName);

            if (i != games.Count - 1)
            {
                Console.Write(", ");
            }
        }

        Console.WriteLine();

        while (selectedGame == null)
        {
            Console.Write("Enter Game ID: ");

            if (int.TryParse(Console.ReadLine(), out int selected))
            {
                if (selected >= 0 && selected < games.Count)
                {
                    selectedGame = games[selected];

                    Console.WriteLine("Selected: " + selectedGame.GetName);
                    Console.WriteLine(selectedGame.GetDescription);

                    break;
                }
            }

            Console.WriteLine("Try again...");
        }

        selectedGame.InitializeActions();

        Dictionary<Key, bool> keyPressed = new Dictionary<Key, bool>();

        foreach (KeyValuePair<Key, Action> action in selectedGame.Actions)
        {
            keyPressed[action.Key] = false;
        }

        selectedGame.GameStart();

        while (true)
        {
            if (Keyboard.IsKeyDown(Key.LeftAlt))
            {
                foreach (KeyValuePair<Key, Action> action in selectedGame.Actions)
                {
                    if (Keyboard.IsKeyDown(action.Key))
                    {
                        if (!keyPressed[action.Key])
                        {
                            keyPressed[action.Key] = true;

                            action.Value();
                        }
                    }
                    else
                    {
                        keyPressed[action.Key] = false;
                    }
                }
            }
            selectedGame.MainLoop();
        }
    }
}
