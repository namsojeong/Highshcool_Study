using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinforcement_Learning
{
    class QLearningManager
    {
        public Dictionary<int, Dictionary<int, float>> ActionValueFunction;
        public float DiscountFactor = 0.9f;
        public float UpdateStep = 0.01f;


        public QLearningManager()
        {
            // 상태 가치 함수
            ActionValueFunction = new Dictionary<int, Dictionary<int, float>>();
        }

        public void UpdateByQLearning()
        {
            InitializeValueFunction();
            ApplyQLearning();
        }

        public void InitializeValueFunction()
        {
            Console.Clear();
            Console.WriteLine("QLearning 시작");
            Console.WriteLine("가치 함수 초기화");

            ActionValueFunction.Clear();
            ActionValueFunction = Utilities.CreateActionValueFunction();

            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("가치 함수 초기화 완료");
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("아무 키나 눌러주세요");
            Console.ReadLine();
        }

        public void ApplyQLearning()
        {
            Console.Clear();
            Console.WriteLine("가치 함수 업데이트 시작");
            Console.WriteLine(Environment.NewLine);

            int episodeCount = 0;
            bool keepUpdating = true;

            while (keepUpdating)
            {
                GameState firstState = new GameState();
                bool episodeFinished = false;

                while (!episodeFinished)
                {
                    // 정책 + 행동 + 보상
                    // 1. 첫 번째 상태, 행동
                    //    두 번째 상태, 행동, 보상
                    //    첫번째 행동을 선택 => 상태 변경 -> 두 번째 상태 지정, 두 번째 행동, 두 번째 상태에 대한 보상 값


                    // 엡실론 탐욕 정책 첫 번째 행동 선택
                    int firstAction = Utilities.GetEpsilonGreedyAction(firstState.NextTurn, ActionValueFunction[firstState.BoardStateKey]);

                    // 선택된 행동을 통해 다음 상태를 두 번째 상태로 지정
                    GameState secondState = firstState.GetNextState(firstAction);
                    
                    // 두 번째 상태에 대한 보상
                    float reward = secondState.GetReward();

                    // 첫번째 상태, 행동에 대한 가치 함수값
                    float firstStateActionValue = ActionValueFunction[firstState.BoardStateKey][firstAction];

                    // 두 번째 상태
                    float secondStateActionValue = Utilities.GetGreedyActionValue(secondState.NextTurn, ActionValueFunction[secondState.BoardStateKey]);                    

                    // 가치함수 업데이트 
                    float _reward = (reward + DiscountFactor * secondStateActionValue - firstStateActionValue);
                    float updateActionValue = firstStateActionValue + UpdateStep * _reward;

                    ActionValueFunction[firstState.BoardStateKey][firstAction] = updateActionValue;

                    if (secondState.IsFinalState() || ActionValueFunction[secondState.BoardStateKey].Count == 0)
                    {
                        episodeFinished = true;
                        episodeCount++;
                    }
                    else
                    {
                        firstState = secondState;
                    }

                }

                // 에피소드 끝
                if (episodeCount % 1000 == 0)
                {
                    Console.WriteLine($"에피소드를 {episodeCount}개 처리 했습니다");
                }
                if (episodeCount > 1000000)
                {
                    keepUpdating = false;
                }

            }

            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("QLearning 종료합니다 아무키나 누르세요");
            Console.ReadLine();
        }

        public int GetNextMove(int boardStateKey)
        {
            GameState gameState = new GameState(boardStateKey);
            return Utilities.GetEpsilonGreedyAction(gameState.NextTurn, ActionValueFunction[boardStateKey]);
        }
    }
}
