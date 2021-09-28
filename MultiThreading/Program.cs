using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading
{
    class Program
    {
        private static bool notPrinted = true;
        private static readonly object lockCompleted = new object();
        private static ManualResetEvent resetEvent = new ManualResetEvent(false);

        static void Main(string[] args)
        {
            Thread t = new Thread(PrintOnce)
            { IsBackground = true };

            t.Name = "Executes using ThreadStart";
            t.Start();

            ThreadPool.QueueUserWorkItem(new WaitCallback(PrintNTimesWithNoParam), 5);
            resetEvent.WaitOne();

            Task task = new Task(ExecuteTask);
            task.Start();

            Task<string> taskThatReturns = new Task<string>(ExecuteTaskAndReturns);
            taskThatReturns.Start();
            Console.WriteLine(taskThatReturns.Result);
            PrintOnce( );
        }

        public static void PrintOnce( )
        {
            lock (lockCompleted)
            {
                if (notPrinted)
                {
                    Console.WriteLine($"Hello Asish from : Thread Id {Thread.CurrentThread.Name}" );
                    notPrinted = false;
                }
            }
        }

        public static void ExecuteTask()
        {
            Console.WriteLine($"Hello Asish : using Task");
        }

        public static string ExecuteTaskAndReturns()
        {
            return "Hello from ExecuteTaskAndReturns";
        }

        public static void PrintOnceParameterized(object item)
        {
            lock (lockCompleted)
            {
                if (notPrinted)
                {
                    Console.WriteLine("Hello Asish");
                    notPrinted = false;
                }
            }
        }

        public static void PrintNTimesWithNoParam(object n)
        {
            Console.WriteLine(Thread.CurrentThread.Name);  // Name a Thread.
            Console.Write(String.Concat(Enumerable.Repeat("Asish " + " ", (int)n)));
            Console.WriteLine();
            resetEvent.Set();
        }
    }
}
