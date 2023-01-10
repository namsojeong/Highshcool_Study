using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinforcement_Learning
{
    public enum GamePlayer
    {
        DynamicProgramming,
        Human,
        None
    }

    class GameManager
    {
        public GamePlayer BlackPlayer;
        public GamePlayer WhitePlayer;

        public void PlayGame()
        {
            while (true)
            {
                BlackPlayer = GetBlackPlayer();
                if (BlackPlayer == GamePlayer.None) return;

                WhitePlayer = GetWhitePlayer();
                if (WhitePlayer == GamePlayer.None) return;

                ManageGame();
            }
        }

        public GamePlayer GetBlackPlayer()
        {
            return PlayerSelection("x 플레이어를 선택해주세요");
        }
        public GamePlayer GetWhitePlayer()
        {
            return PlayerSelection("o 플레이어를 선택해주세요");
        }
        public GamePlayer PlayerSelection(string menuLabel)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine(menuLabel);
                Console.WriteLine(Environment.NewLine);

                Console.WriteLine("1) 동적프로그래밍");
                Console.WriteLine("2) 사람");
                Console.WriteLine("3) 게임 종료");
                Console.Write("선택 (1~3): ");

                switch (Console.ReadLine())
                {
                    case "1":
                        if (Program.DPManager.StateValueFunction.Count > 0)
                        {
                            return GamePlayer.DynamicProgramming;
                        }
                        else
                        {
                            Console.WriteLine("동적 프로그래밍을 수행하세요");
                            Console.WriteLine(Environment.NewLine);
                            Console.WriteLine("아무 키나 누르세요");
                            Console.ReadLine();
                        }
                            break;
                    case "2":
                        Console.WriteLine("사람을 선택하셨습니다.");
                        Console.WriteLine(Environment.NewLine);
                        Console.WriteLine("아무 키나 누르세요");
                        Console.ReadLine();
                        return GamePlayer.Human;
                    case "3":
                        Console.WriteLine("메인 메뉴로 돌아갑니다.");
                        Console.WriteLine(Environment.NewLine);
                        Console.WriteLine("아무 키나 누르세요");
                        Console.ReadLine();
                        return GamePlayer.None;
                    default:
                        break;
                }
            }
        }

        public void ManageGame()
        {
            GameState gameState = new GameState();
            int gameTurnCount = 0;
            int gameMove = 0;

            bool isGameFinished = gameState.IsFinalState();

            while(!isGameFinished)
            {

                gameState.DisplayBoard(gameTurnCount, gameMove, BlackPlayer, WhitePlayer);

                isGameFinished = gameState.IsFinalState();

                Console.WriteLine(Environment.NewLine);

                if(isGameFinished)
                {
                    Console.WriteLine("게임이 끝났습니다. 아무 키나 눌러 주세요");
                    Console.ReadLine();
                }
                else
                {
                    GamePlayer playerForNextTurn = GetGamePlayer(gameState.NextTurn);
                    if(playerForNextTurn == GamePlayer.Human)
                    {
                        gameMove = GetHumanGameNove(gameState);
                    }
                    else
                    {
                        Console.WriteLine("게임이 끝났습니다. 아무 키나 눌러 주세요");
                        Console.ReadLine();

                        gameMove = Program.DPManager.GetNextMove(gameState.BoardStateKey);
                    }

                    gameState.MakeMove(gameMove);
                    gameTurnCount++;
                }
            }
        }

        public int GetHumanGameNove(GameState gameState)
        {
                    Console.WriteLine("다음 행동을 입력하세요. (1~9): ");
            string humanMove = Console.ReadLine();

            while(true)
            {
                try
                {
                    int gameMove = Int32.Parse(humanMove);

                    if(gameMove >= 1 && gameMove <=9 && gameState.IsValidMove(gameMove)) return gameMove;
                    else
                    {
                        Console.WriteLine("-.- !! (1~9) : ");
                        humanMove = Console.ReadLine();
                    }
                }
                catch
                {
                    Console.WriteLine("-.- !! (1~9) : ");
                    humanMove = Console.ReadLine();
                }
            }

        }

        public GamePlayer GetGamePlayer(int turn)
        {
            return turn == 1 ? BlackPlayer : WhitePlayer;
        }
    }

}
