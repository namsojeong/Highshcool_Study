using System;

namespace Reinforcement_Learning
{
    class Program
    {
        public static DynamicProgrammingManager DPManager;

        static void Main(string[] args)
        {
            DPManager = new DynamicProgrammingManager();

            bool showMenu = true; // true:메뉴창 false:게임창

            while(showMenu)
            {
                showMenu = MainMenu();


            }
            
        }

        private static bool MainMenu()
        {
            Console.Clear();

            Console.WriteLine("메뉴 번호를 선택해주세요");
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("1) 동적프로그래밍 실행");
            Console.WriteLine("2) 게임하기");
            Console.WriteLine("3) 종료");
            Console.WriteLine(Environment.NewLine);
            Console.Write("동작 선택 : ");

            switch(Console.ReadLine())
            {
                case "1":
                    DPManager.UpdateByDynamicProgramming();
                    return true;
                case "2":
                    return true;
                case "3":
                    return false;
                default:
                    return true;
            }

        }
    }
}