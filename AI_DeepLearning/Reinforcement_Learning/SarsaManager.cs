using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinforcement_Learning
{

    class SarsaManager
    {
        public Dictionary<int, Dictionary<int, float>> ActionValueFunction;
        public float DiscountFactor = 0.9f;
        public float UpdateStep = 0.01f;

        int num00 = 0;
        int num10 = 0;
        int num11 = 0;
        int num21 = 0;
        int num22 = 0;
        int num32 = 0;
        int num33 = 0;
        int num43 = 0;
        int num44 = 0;

        public SarsaManager()
        {
            // 상태 가치 함수
            ActionValueFunction = new Dictionary<int, Dictionary<int, float>>();
        }

        public void UpdateBySarsa()
        {
            InitializeValueFunction();
            ApplySarsa();
        }

        public void InitializeValueFunction()
        {
            Console.Clear();
            Console.WriteLine("SARSA 시작");
            Console.WriteLine("가치 함수 초기화");

            ActionValueFunction.Clear();
            ActionValueFunction = Utilities.CreateActionValueFunction();

            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("가치 함수 초기화 완료");
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("아무 키나 눌러주세요");
            Console.ReadLine();
        }

        public int GetNextMove(int boardStateKey)
        {
            GameState gameState = new GameState(boardStateKey);
            return Utilities.GetEpsilonGreedyAction(gameState.NextTurn, ActionValueFunction[boardStateKey]);
        }


        public void ApplySarsa()
        {
            Console.Clear();
            Console.WriteLine("Sarsa 적용");
            Console.WriteLine(Environment.NewLine);

            int episodeCount = 0;
            bool keepUpdating = true;

            while(keepUpdating)
            {
                // 초기 상태부터 시작해서 쭉 ~ 랜덤하게 다음 수를 생성
                // Sarsa 샘플링 작업을 가치함수를 업데이트 한다.

                GameState firstState = new GameState();
                // 샘플링 하는 그 과정을 Episode라고 함
                // 아까 100만이 에피소드카운트임
                bool episodeFinished = false;

                while(!episodeFinished)
                {
                    // 정책 + 행동 + 보상
                    // 1. 첫 번째 상태, 행동
                    //    두 번째 상태, 행동, 보상
                    //    첫번째 행동을 선택 => 상태 변경 -> 두 번째 상태 지정, 두 번째 행동, 두 번째 상태에 대한 보상 값


                    // 엡실론 탐욕 정책 첫 번째 행동 선택
                    int firstAction = Utilities.GetEpsilonGreedyAction(firstState.NextTurn, ActionValueFunction[firstState.BoardStateKey]);

                    // 선택된 행동을 통해 다음 상태를 두 번째 상태로 지정
                    GameState secondState = firstState.GetNextState(firstAction);

                    int secondAction = Utilities.GetEpsilonGreedyAction(secondState.NextTurn, ActionValueFunction[secondState.BoardStateKey]);
                    // 두 번째 상태에 대한 보상
                    float reward = secondState.GetReward();

                    float firstStateActionValue = ActionValueFunction[firstState.BoardStateKey][firstAction];
                    float secondStateActionValue = 0f;

                    // -100 이면 위험
                    if(secondAction !=0)
                    {
                        secondStateActionValue = ActionValueFunction[secondState.BoardStateKey][secondAction];
                    }

                     
                    // 가치함수 업데이트 
                    float _reward = (reward + DiscountFactor * secondStateActionValue - firstStateActionValue);
                    float updateActionValue = firstStateActionValue + UpdateStep * _reward;

                    ActionValueFunction[firstState.BoardStateKey][firstAction] = updateActionValue;

                    if(secondState.IsFinalState() || ActionValueFunction[secondState.BoardStateKey].Count == 0)
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
                if(episodeCount > 1000000)
                {
                    keepUpdating = false;
                }

            }

            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("Sarsa 종료합니다 아무키나 누르세요");
            Console.ReadLine();
        }
    }
}
