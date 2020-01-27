using System;
using System.Timers;
namespace QuranApp
{
    class Updater
    {
        private static Timer timer;
        private static bool TimerOn = false;
        public static void Start()
        {
            SetTimer();
            Console.WriteLine("السلام عليكم");
            Console.WriteLine("\nPress the Enter key to exit the application...\n");
            Console.WriteLine("The application started at {0:HH:mm:ss.fff}", DateTime.Now);
            Console.ReadLine();
            QuitApp();

        }

        private static void QuitApp()
        {
            if (TimerOn)
            {
                timer.Stop();
                timer.Dispose();
            }
            Console.WriteLine("Terminating the application...");
        }

        private static void SetTimer()
        {
            TimerOn = true;
            timer = new Timer();
            timer.Elapsed += Update;

            //timer.Elapsed += new ElapsedEventHandler(Update);

            timer.Interval = 100;
            timer.AutoReset = true;
            timer.Enabled = true;
        }
        private static int count = 0;
        private static void Update(object source, ElapsedEventArgs e)
        {
            count++;
            Console.WriteLine($"Count = {count}");
        }
    }
}
