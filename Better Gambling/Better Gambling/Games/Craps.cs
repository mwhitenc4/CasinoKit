using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

class Craps : IGambleGame
{
    public string GetName => "Craps";

    public string GetDescription => "ALT+O To Reset, ALT+I To Advance Game";

    public Dictionary<Key, Action> Actions { get; private set; } = new Dictionary<Key, Action>();

    int point = -1;
    bool gameOver = false;

    public void GameStart()
    {
        point = -1;
        gameOver = false;
        Gambling.SetClipboard("[Craps] Game Started");
    }

    public void AdvanceGame()
    {
        if (gameOver) return;

        Random rand = new Random();
        int dice = rand.Next(1, 7) + rand.Next(1, 7);
        if(point == -1)
        {
            if(dice == 7 || dice == 11)
            {
                Gambling.SetClipboard("[Craps] (PLAYER WIN) You roll a: " + dice);
                gameOver = true;
            }
            else if(dice == 2 || dice == 3 || dice == 12)
            {
                Gambling.SetClipboard("[Craps] (PLAYER LOSE) You roll a: " + dice);
                gameOver = true;
            }
            else
            {
                point = dice;
                Gambling.SetClipboard("[Craps] (POINT SET) You roll a: " + dice + ", roll another " + dice + " to win!");
            }
        }
        else
        {
            if(dice == 7)
            {
                Gambling.SetClipboard("[Craps] (PLAYER LOSE) You roll a: " + dice);
                gameOver = true;
            }
            else if(dice == point)
            {
                Gambling.SetClipboard("[Craps] (PLAYER WIN) You roll a: " + dice);
                gameOver = true;
            }
            else
            {
                Gambling.SetClipboard("[Craps] You roll a: " + dice + ", keep rolling, you need a " + point + " to win!");
            }
        }
    }

    public void InitializeActions()
    {
        Actions[Key.O] = () => GameStart();
        Actions[Key.I] = () => AdvanceGame();
    }

    public void MainLoop()
    {
        
    }
}
