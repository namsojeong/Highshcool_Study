using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Reinforcement_Learning
{
    public class ConnectedPosition
    {
        public int row;
        public int col;
        public ConnectedPosition(int r, int c)
        {
            row = r;
            col = c;
        }
    }

    public class GameParameters
    {
        public static int StateCount = 19560; // 222211110 10진수 표현 => 경우의수
        public static int ActionMinIndex = 1;
        public static int ActionMaxIndex = 9;
    }


    public class GameState
    {
        public int[,] BoardState;
        public int NextTurn;
        public int BoardStateKey;
        public int NumberOfBlacks;
        public int NumberOfWhites;
        public int GameWinner;

        public GameState()
        {
            BoardState = new int[,] { { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 } };
            NextTurn = 1;
            BoardStateKey = 1;
            NumberOfBlacks = 0;
            NumberOfWhites = 0;
            GameWinner = 0;
        }


        public GameState(int boardStateKey)
        {
            BoardState = new int[3, 3];
            BoardStateKey = boardStateKey;
            NextTurn = boardStateKey % 3;
            GameWinner = 0;
            PopulateBoard(boardStateKey / 3);
        }

        public void PopulateBoard(int boardState)
        {

            int boardValueProcessing = boardState;
            NumberOfBlacks = 0;
            NumberOfWhites = 0;

            for (int i = 8; i >= 0; i--)
            {
                int boardValue = boardValueProcessing % 3;
                boardValueProcessing = boardValueProcessing / 3;

                BoardState[i / 3, i % 3] = boardValue;

                if (boardValue == 1)
                    NumberOfBlacks++;

                if (boardValue == 2)
                    NumberOfWhites++;
            }
        }


        public bool IsValidSecondStage()
        {
            if (NumberOfBlacks == 4 && NumberOfWhites == 4)
                return true;
            return false;
        }

        public bool IsValidFirstStage()
        {
            if (NumberOfBlacks > 4)
                return false;
            if (NumberOfWhites > 3)
                return false;

            if (NumberOfBlacks == NumberOfWhites || NumberOfBlacks == NumberOfWhites + 1)
                return true;

            return false;
        }

        public int GetFirstStageTurn()
        {
            if (NumberOfBlacks == NumberOfWhites)
                return 1;
            if (NumberOfBlacks == NumberOfWhites + 1)
                return 2;

            return 0;
        }


        public bool IsFinalState()
        {
            GameWinner = 0;

            if (BoardState[0, 0] == BoardState[0, 1] && BoardState[0, 1] == BoardState[0, 2])
            {
                if (BoardState[0, 0] != 0)
                {
                    GameWinner = BoardState[0, 0];
                    return true;
                }
            }
            if (BoardState[1, 0] == BoardState[1, 1] && BoardState[1, 1] == BoardState[1, 2])
            {
                if (BoardState[1, 0] != 0)
                {
                    GameWinner = BoardState[1, 0];
                    return true;
                }
            }
            if (BoardState[2, 0] == BoardState[2, 1] && BoardState[2, 1] == BoardState[2, 2])
            {
                if (BoardState[2, 0] != 0)
                {
                    GameWinner = BoardState[2, 0];
                    return true;
                }
            }
            if (BoardState[0, 0] == BoardState[1, 0] && BoardState[1, 0] == BoardState[2, 0])
            {
                if (BoardState[0, 0] != 0)
                {
                    GameWinner = BoardState[0, 0];
                    return true;
                }
            }
            if (BoardState[0, 1] == BoardState[1, 1] && BoardState[1, 1] == BoardState[2, 1])
            {
                if (BoardState[0, 1] != 0)
                {
                    GameWinner = BoardState[0, 1];
                    return true;
                }
            }
            if (BoardState[0, 2] == BoardState[1, 2] && BoardState[1, 2] == BoardState[2, 2])
            {
                if (BoardState[0, 2] != 0)
                {
                    GameWinner = BoardState[0, 2];
                    return true;
                }
            }
            if (BoardState[0, 0] == BoardState[1, 1] && BoardState[1, 1] == BoardState[2, 2])
            {
                if (BoardState[0, 0] != 0)
                {
                    GameWinner = BoardState[0, 0];
                    return true;
                }
            }
            if (BoardState[0, 2] == BoardState[1, 1] && BoardState[1, 1] == BoardState[2, 0])
            {
                if (BoardState[0, 2] != 0)
                {
                    GameWinner = BoardState[0, 2];
                    return true;
                }
            }

            return false;
        }

        public bool IsValidMove(int move)
        {


            int row = (move - 1) / 3;
            int col = (move - 1) % 3;

            if (IsValidFirstStage())
            {
                if (BoardState[row, col] == 0)
                    return true;
            }
            else if (IsValidSecondStage())
            {
                if (BoardState[row, col] != NextTurn)
                    return false;

                IEnumerable<ConnectedPosition> ConnectedPositions = GetConnectedPosition(row, col);
                IEnumerable<ConnectedPosition> ConnectedEmptySpots = ConnectedPositions.Where(e => BoardState[e.row, e.col] == 0);

                if (ConnectedEmptySpots.Count() > 0)
                {
                    IEnumerable<ConnectedPosition> ConnectedOpponents = ConnectedPositions.Where(e => BoardState[e.row, e.col] != 0 && BoardState[e.row, e.col] != NextTurn);

                    if (ConnectedOpponents.Count() > 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }


        private IEnumerable<ConnectedPosition> GetConnectedPosition(int row, int col)
        {

            List<ConnectedPosition> connectedPositionList = new List<ConnectedPosition>();

            if (row == 0 && col == 0)
            {
                connectedPositionList.Add(new ConnectedPosition(0, 1));
                connectedPositionList.Add(new ConnectedPosition(1, 1));
                connectedPositionList.Add(new ConnectedPosition(1, 0));
            }
            if (row == 0 && col == 1)
            {
                connectedPositionList.Add(new ConnectedPosition(0, 0));
                connectedPositionList.Add(new ConnectedPosition(1, 1));
                connectedPositionList.Add(new ConnectedPosition(0, 2));
            }
            if (row == 0 && col == 2)
            {
                connectedPositionList.Add(new ConnectedPosition(0, 1));
                connectedPositionList.Add(new ConnectedPosition(1, 1));
                connectedPositionList.Add(new ConnectedPosition(1, 2));
            }
            if (row == 1 && col == 0)
            {
                connectedPositionList.Add(new ConnectedPosition(0, 0));
                connectedPositionList.Add(new ConnectedPosition(1, 1));
                connectedPositionList.Add(new ConnectedPosition(2, 0));
            }
            if (row == 1 && col == 1)
            {
                connectedPositionList.Add(new ConnectedPosition(0, 0));
                connectedPositionList.Add(new ConnectedPosition(0, 1));
                connectedPositionList.Add(new ConnectedPosition(0, 2));
                connectedPositionList.Add(new ConnectedPosition(1, 0));
                connectedPositionList.Add(new ConnectedPosition(1, 2));
                connectedPositionList.Add(new ConnectedPosition(2, 0));
                connectedPositionList.Add(new ConnectedPosition(2, 1));
                connectedPositionList.Add(new ConnectedPosition(2, 2));
            }
            if (row == 1 && col == 2)
            {
                connectedPositionList.Add(new ConnectedPosition(0, 2));
                connectedPositionList.Add(new ConnectedPosition(1, 1));
                connectedPositionList.Add(new ConnectedPosition(2, 2));
            }
            if (row == 2 && col == 0)
            {
                connectedPositionList.Add(new ConnectedPosition(1, 0));
                connectedPositionList.Add(new ConnectedPosition(1, 1));
                connectedPositionList.Add(new ConnectedPosition(2, 1));
            }
            if (row == 2 && col == 1)
            {
                connectedPositionList.Add(new ConnectedPosition(2, 0));
                connectedPositionList.Add(new ConnectedPosition(1, 1));
                connectedPositionList.Add(new ConnectedPosition(2, 2));
            }
            if (row == 2 && col == 2)
            {
                connectedPositionList.Add(new ConnectedPosition(1, 2));
                connectedPositionList.Add(new ConnectedPosition(1, 1));
                connectedPositionList.Add(new ConnectedPosition(2, 1));
            }
            return connectedPositionList;
        }


        public GameState GetNextState(int move)
        {
            GameState nextState = new GameState(BoardStateKey);
            nextState.MakeMove(move);
            return nextState;
        }

        public void MakeMove(int move)
        {

            int row = (move - 1) / 3;
            int col = (move - 1) % 3;

            if (IsValidFirstStage())
            {

                BoardState[row, col] = NextTurn;

                if (NextTurn == 1)
                    NumberOfBlacks++;
                else if (NextTurn == 2)
                    NumberOfWhites++;
            }
            else if (IsValidSecondStage())
            {
                int emptyRow = 0;
                int emptyCol = 0;

                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (BoardState[i, j] == 0)
                        {
                            emptyRow = i;
                            emptyCol = j;
                            break;
                        }
                    }
                }
                BoardState[row, col] = 0;
                BoardState[emptyRow, emptyCol] = NextTurn;
            }

            if (NextTurn == 1)
                NextTurn = 2;
            else if (NextTurn == 2)
                NextTurn = 1;

            int boardStateKey = 0;

            for (int i = 0; i < 9; i++)
            {
                boardStateKey = boardStateKey * 3;
                boardStateKey = boardStateKey + BoardState[i / 3, i % 3];
            }

            BoardStateKey = boardStateKey * 3 + NextTurn;
        }

        public float GetReward()
        {

            if (IsFinalState())
            {
                if (GameWinner == 1)
                    return 100.0f;
                else if (GameWinner == 2)
                    return -100.0f;
            }

            return 0.0f;
        }

        public string GetTurnMark()
        {
            return NextTurn == 1 ? "x" : "o";
        }

        // GetGBV : GetGameBoardValue
        private string GetGBVal(int row, int col)
        {
            switch(BoardState[row, col])
            {
                case 1:
                    return "○";
                case 2:
                    return "●";
                default:
                    return "x";
            }
        }

        public void DisplayBoard(int turnCount, int lastMove, GamePlayer blackPlayer, GamePlayer whitePlayer)
        {
            Console.Clear();
            Console.WriteLine(Environment.NewLine);

            Console.WriteLine($"X : {blackPlayer}, O : {whitePlayer}");
            Console.WriteLine($"게임 턴 : {turnCount}, ");
            Console.WriteLine($"{GetTurnMark()}");
            Console.WriteLine(Environment.NewLine);

            if (IsValidFirstStage())
            {
                Console.WriteLine("1단계 진행 중입니다.");
            }
            else
            {
                Console.WriteLine("2단계 진행 중입니다.");
            }

            if (lastMove != 0)
            {
                Console.WriteLine($"지난 행동, row : {(lastMove - 1) / 3}, col : {(lastMove - 1) % 3}");

            }
            
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine($"        {GetGBVal(0, 0)}         {GetGBVal(0, 1)}        {GetGBVal(0, 2)}");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine($"        {GetGBVal(1, 0)}         {GetGBVal(1, 1)}        {GetGBVal(1, 2)}");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine($"        {GetGBVal(2, 0)}         {GetGBVal(2, 1)}        {GetGBVal(2, 2)}");

            IsFinalState();
            Console.WriteLine(Environment.NewLine);
            switch(GameWinner)
            {
                case 1: Console.WriteLine("흑 돌이 이겼습니다.");break;
                case 2: Console.WriteLine("흰 돌이 이겼습니다.");break;
                default: Console.WriteLine("게임이 진행 중입니다.");break;
            }
        }
    }
}
