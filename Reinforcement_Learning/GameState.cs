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


    class GameState
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

        public void PopulateBoard(int boardState)
        {
            // 주어진 보드 상태 값을 3진수로 변환시키면서
            // 보드 상태 생성

            int boardValueProcessing = boardState;

            NumberOfBlacks = 0;
            NumberOfWhites = 0;

            for (int i = 8; i >= 0; i--)
            {
                int boardValue = boardValueProcessing % 3;
                boardValueProcessing /= 3;

                BoardState[i/3, i % 3] = boardValue;

                if(boardValue == 1) NumberOfBlacks++;
                if(boardValue == 2) NumberOfWhites++;

            }

        }

        public bool IsValidFirstStage()
        {
            if (NumberOfBlacks > 4) return false;
            if(NumberOfWhites > 3) return false;

            if(NumberOfBlacks == NumberOfWhites || NumberOfWhites+1 == NumberOfBlacks)
            {
                return true;
            }

            return false;
        }

        public bool IsValidSecondStage()
        {
            if (NumberOfBlacks == 4 && NumberOfWhites == 4) return true;
            return false;
        }

        public int GetFirstStageTurn()
        {
            if (NumberOfWhites == NumberOfBlacks) return 1;
            if (NumberOfWhites+1 == NumberOfBlacks) return 2;

            return 0;
        }

        public bool IsFinalState()
        {
            GameWinner = 0;

            for(int i=0;i<3;i++)
            {
                if (BoardState[i, 0] == BoardState[i, 1] && BoardState[i, 1] == BoardState[i, 2])
                {
                    if (BoardState[i, 0] != 0)
                    {
                        GameWinner = BoardState[i, 0];
                        return true;
                    }
                }
            }
            for(int i=0;i<3;i++)
            {
                if (BoardState[0, i] == BoardState[1, i] && BoardState[1, i] == BoardState[2, i])
                {
                    if (BoardState[0, i] != 0)
                    {
                        GameWinner = BoardState[i, 0];
                        return true;
                    }
                }
            }

            if (BoardState[0, 2] == BoardState[1, 1] && BoardState[1, 1] == BoardState[2, 2])
            {
                if (BoardState[0, 2] != 0)
                {
                    GameWinner = BoardState[0, 0];
                    return true;
                }
            }
            if (BoardState[0, 0] == BoardState[1, 1] && BoardState[1, 1] == BoardState[2, 2])
            {
                if (BoardState[0, 2] != 0)
                {
                    GameWinner = BoardState[0, 0];
                    return true;
                }
            }
            if (BoardState[0, 2] == BoardState[1, 1] && BoardState[1, 1] == BoardState[2, 0])
            {
                if (BoardState[0, 2] != 0)
                {
                    GameWinner = BoardState[0, 0];
                    return true;
                }
            }
            return false;
        }

        public bool IsValidMove(int move)
        {
            // 게임 상태에 주어진 행동을 적용할 수 있는가?
            int row = (move - 1) / 3;
            int col = (move - 1) % 3;

            if(IsValidFirstStage())
            {
                if (BoardState[row, col] == 0) return true;
            }
            else if(IsValidSecondStage())
            {
                if (BoardState[row, col] != NextTurn)
                {
                    return false;
                }

                IEnumerable<ConnectedPosition> ConnectedPositions = GetConnectedPosition(row, col);
                IEnumerable<ConnectedPosition> connectedEmptySports = ConnectedPositions.Where(e => BoardState[e.row, e.col]==0);

                if(connectedEmptySports.Count()>0)
                {
                    IEnumerable<ConnectedPosition> connectedPositions
                        = ConnectedPositions.Where(e => BoardState[e.row, e.col] != 0 && BoardState[e.row, e.col] != NextTurn);

                    if(connectedPositions.Count()>0)
                    {
                        return true;
                    }
                }
                return false;
            }

            return false;
        }

        private IEnumerable<ConnectedPosition> GetConnectedPosition(int row, int col)
        {
            List<ConnectedPosition> connectedPositionList = new List<ConnectedPosition>();

            int[] dx = { 0, 0, -1, 1, -1, 1, 1, -1};
            int[] dy = { 1, -1, 0, 0, 1, -1, 1 , -1};

            for(int i=0;i<8; i++)
            {
                int nx = col + dx[i];
                int ny = row + dy[i];

                if (ny < 0 || nx < 0 || nx > 2 || ny > 2) continue;
                connectedPositionList.Add(new ConnectedPosition(ny, nx));
            }

            return connectedPositionList;
        }
    }
}
