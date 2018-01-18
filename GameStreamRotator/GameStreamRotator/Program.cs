using System;

namespace GameStreamRotator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var rotator = new Rotator();

            var watcher = new ProcessWatcher(() => rotator.Rotate());
            watcher.WatchForProcessStart("nvstreamer.exe");

            Console.WriteLine("Press return to quit.");
            Console.ReadLine();
        }
    }
}