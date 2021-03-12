using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

class DoubleOrNothing : IGambleGame
{
    public string GetName => "Double or Nothing";

    public string GetDescription => "ALT+O to Reset, ALT+I to Double";

    public Dictionary<Key, Action> Actions { get; private set; } = new Dictionary<Key, Action>();

    int betAmount = -1;
    int total = -1;
    bool isBust = false;
    int[] chanceForBust = { 15, 25 }; // 15 in 25 chance of going bust

    public void GameStart()
    {
        while (betAmount == -1) {
            Console.Write("Enter Bet Amount: ");

            if (int.TryParse(Console.ReadLine(), out betAmount)){
                Console.WriteLine("Bet amount set to: " + Gambling.FormatMoney(betAmount));
            }
        }
        Reset();
    }

    public void Reset()
    {
        isBust = false;
        total = betAmount;
        Gambling.SetClipboard("[Double or Nothing] Game Started: " + Gambling.FormatMoney(total));
    }

    public void Double()
    {
        if (!isBust)
        {
            Random rand = new Random();

            if(rand.Next(1, chanceForBust[1] + 1) <= chanceForBust[0])
            {
                isBust = true;
                Gambling.SetClipboard("[Double or Nothing] (BUST): " + Gambling.FormatMoney(total) + "!");
            }
            else
            {
                total *= 2;
                Gambling.SetClipboard("[Double or Nothing] (DOUBLE): " + Gambling.FormatMoney(total) + "!");
            }
        }
    }

    public void InitializeActions()
    {
        Actions[Key.O] = () => Reset();
        Actions[Key.I] = () => Double();
    }

    public void MainLoop()
    {
        
    }
}