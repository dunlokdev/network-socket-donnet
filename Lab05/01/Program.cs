using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _01
{
    public class Program
    {
        static void Main(string[] args)
        {
            MyTheardClass mtc1 = new MyTheardClass("Day la tieu trinh thu 1");
            Thread thread1 = new Thread(new ThreadStart(mtc1.RunMyThread));
            thread1.Start();

            MyTheardClass mtc2 = new MyTheardClass("Day la tieu trinh thu 2");
            Thread thread2 = new Thread(new ThreadStart(mtc2.RunMyThread));
            thread2.Start();

            Console.ReadKey();
        }
    }
    class MyTheardClass
    {
        private const int RANDOM_SLEEP_MAX = 1000;
        private const int LOOP_COUNT = 10;

        private String greeting;
        
        public MyTheardClass(String greeting)
        {
            this.greeting = greeting;
        }
        public void RunMyThread()
        {
            Random rand = new Random();
            for (int i = 0; i < LOOP_COUNT; i++)
            {
                Console.WriteLine(greeting + "\tThread ID: {0}", Thread.CurrentThread.GetHashCode());
                try
                {
                    Thread.Sleep(rand.Next(0, RANDOM_SLEEP_MAX));
                }
                catch (ThreadInterruptedException)
                {
                    
                }
            }
        }
    }
}
