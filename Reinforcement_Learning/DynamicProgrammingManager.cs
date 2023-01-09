using System;
using System.Collections.Generic;
using System.Text;

namespace Reinforcement_Learning
{
    class DynamicProgrammingManager
    {
        public Dictionary<int, float> StateValueFunction;
        public float DiscountFactor = 0.9f;

        int num00 = 0;
        int num10 = 0;
        int num11 = 0;
        int num21 = 0;
        int num22 = 0;
        int num32 = 0;
        int num33 = 0;
        int num43 = 0;
        int num44 = 0;

        public DynamicProgrammingManager()
        {
            StateValueFunction = new Dictionary<int, float>();
        }

        public void UpdateByDynamicProgramming()
        {
            // 초기화
            InitializeValueFunction();
            // 동적프로그래밍 사용
            ApplyDynamicProgramming();
        }

        public void InitializeValueFunction()
        {
            Console.Clear();
            Console.WriteLine("동적 프로그래밍 시작");
            Console.WriteLine("가치 함수 초기화");

            // 상태 숫자랑 딕셔너리 초기화
            StateCountReset();
            StateValueFunction.Clear();

            for (int i = 0; i < GameParameters.StateCount; i++)
            {
                GameState state = new GameState();
                state.PopulateBoard(i);

                if(state.IsValidSecondStage()) // 2단계
                {
                    StateValueFunction.Add(i * 3 + 1, 0.0f); // 검은 돌 차례 대 상태를 가치 함수로 저장
                    StateValueFunction.Add(i * 3 + 2, 0.0f); // 하얀 돌 차례 대 상태를 가치 함수로 저장

                    if(state.NumberOfBlacks == 4 && state.NumberOfWhites == 4)
                    {
                        num44++;
                    }
                }
                else if (state.IsValidFirstStage()) // 1단계
                {
                    StateValueFunction.Add(i * 3 + state.GetFirstStageTurn(), 0.0f); // 하얀 돌 차례 대 상태를 가치 함수로 저장

                    if (state.NumberOfBlacks == 0 && state.NumberOfWhites == 0) num00++;
                    if (state.NumberOfBlacks == 1 && state.NumberOfWhites == 0) num10++;
                    if (state.NumberOfBlacks == 1 && state.NumberOfWhites == 1) num11++;
                    if (state.NumberOfBlacks == 2 && state.NumberOfWhites == 1) num21++;
                    if (state.NumberOfBlacks == 2 && state.NumberOfWhites == 2) num22++;
                    if (state.NumberOfBlacks == 3 && state.NumberOfWhites == 2) num32++;
                    if (state.NumberOfBlacks == 3 && state.NumberOfWhites == 3) num33++;
                    if (state.NumberOfBlacks == 4 && state.NumberOfWhites == 3) num43++;
                }
            }

            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("가치 함수 초기화 완료");
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine($"Black 0, White 0 : {num00}");
            Console.WriteLine($"Black 1, White 0 : {num10}");
            Console.WriteLine($"Black 1, White 1 : {num11}");
            Console.WriteLine($"Black 2, White 1 : {num21}");
            Console.WriteLine($"Black 2, White 2 : {num22}");
            Console.WriteLine($"Black 3, White 2 : {num32}");
            Console.WriteLine($"Black 3, White 3 : {num33}");
            Console.WriteLine($"Black 4, White 3 : {num43}");
            Console.WriteLine($"Black 4, White 4 : {num44}");
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("아무 키나 누르세요");
            Console.ReadLine();

        }

        public void ApplyDynamicProgramming()
        {
        }

        public void StateCountReset()
        {
            num00 = 0;
            num10 = 0;
            num11 = 0;
            num21 = 0;
            num22 = 0;
            num32 = 0;
            num33 = 0;
            num43 = 0;
            num44 = 0;
        }

    }
}
