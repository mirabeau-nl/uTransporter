using System.Diagnostics;

namespace Mirabeau.uTransporter.Extensions
{
    /// <summary>
    ///  uTransporter Timer - Custom Timer
    /// </summary>
    public class Timer : Stopwatch
    {
        private readonly Stopwatch _time;

        /// <summary>
        /// Initializes a new instance of the <see cref="Timer"/> class. 
        /// public constructor
        /// </summary>
        public Timer()
        {
            _time = new Stopwatch();
        }

        /// <summary>
        /// starts the timer
        /// </summary>
        public void StartTimer()
        {
            _time.Start();
        }

        /// <summary>
        /// stops the timer
        /// </summary>
        public void StopTimer()
        {
            _time.Stop();
        }

        /// <summary>
        /// resets the timer to zero
        /// </summary>
        public void ResetTimer()
        {
            _time.Reset();
        }

        /// <summary>
        /// restarts the timer from sero
        /// </summary>
        public void RestartTimer()
        {
            _time.Restart();
        }

        /// <summary>
        /// check's if a current timer is running
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool IsTimerRunning()
        {
            return _time.IsRunning;
        }
    }
}