using System;
using System.Management;

namespace GameStreamRotator
{
    // Code borrowed from Academy of Programmer
    // On StackOverflow here: https://stackoverflow.com/questions/848618/net-events-for-process-executable-start
    public class ProcessWatcher
    {
        private string _processNameProperty = "ProcessName";
        private string _process;

        private Action _rotate;

        public ProcessWatcher(Action act)
        {
            _rotate = act;
        }

        public void WatchForProcessStart(string process)
        {
            _process = process;
            ManagementEventWatcher processStartWatcher = null;
            ManagementEventWatcher processStopWatcher = null;
            try
            {
                WqlEventQuery startEventQuery = new WqlEventQuery();
                startEventQuery.EventClassName = "Win32_ProcessStartTrace";
                processStartWatcher = new ManagementEventWatcher(startEventQuery);
                processStartWatcher.EventArrived += new EventArrivedEventHandler(ProcessStartEventArrived);

                WqlEventQuery stopEventQuery = new WqlEventQuery();
                stopEventQuery.EventClassName = "Win32_ProcessStopTrace";
                processStopWatcher = new ManagementEventWatcher(stopEventQuery);
                processStopWatcher.EventArrived += new EventArrivedEventHandler(ProcessStopEventArrived);

                processStartWatcher.Start();
                processStopWatcher.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Screen will not rotate when GameStream starts. (Please make sure to run as administator)");
            }
        }

        public void ProcessStartEventArrived(object sender, EventArrivedEventArgs e)
        {
            var process = e.NewEvent.Properties[_processNameProperty];

            if (process.Value.ToString() == _process)
            {
                Console.WriteLine("Rotating screen.");
                _rotate.Invoke();
            }
        }

        public void ProcessStopEventArrived(object sender, EventArrivedEventArgs e)
        {
            var process = e.NewEvent.Properties[_processNameProperty];

            if (process.Value.ToString() == _process)
            {
                Console.WriteLine("Rotating screen back.");
                _rotate.Invoke();
            }
        }
    }
}
