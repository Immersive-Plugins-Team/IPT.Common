using System;
using Rage;

namespace IPT.Common.Fibers
{
    /// <summary>
    /// An abstract class for defining a start/stoppable long-running fiber.
    /// </summary>
    public abstract class GenericFiber
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GenericFiber"/> class.
        /// </summary>
        /// <param name="interval">How long the fiber should sleep between executions.</param>
        /// <param name="name">A descriptive name for the fiber.  Does not need to be unique.</param>
        protected GenericFiber(string name, int interval)
        {
            Name = $"{name.ToUpper()}-{Guid.NewGuid()}";
            IsRunning = false;
            Interval = interval;
        }

        /// <summary>
        /// Gets the name of the fiber.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets a value indicating whether or not the fiber is currently running.
        /// </summary>
        public bool IsRunning { get; private set; }

        /// <summary>
        /// Gets a value indicating the time between executions.
        /// </summary>
        protected int Interval { get; private set; }

        /// <summary>
        /// Starts the fiber.
        /// </summary>
        public virtual void Start()
        {
            if (IsRunning) return;
            IsRunning = true;
            GameFiber.StartNew(Run, Name);
        }

        /// <summary>
        /// Stops the fiber.
        /// </summary>
        public virtual void Stop()
        {
            IsRunning = false;
        }

        /// <summary>
        /// This method is executing on each fiber execution.
        /// </summary>
        protected abstract void DoSomething();

        /// <summary>
        /// This is the void method used to start the fiber.
        /// </summary>
        protected virtual void Run()
        {
            while (IsRunning)
            {
                if (Interval == 0) GameFiber.Yield();
                else GameFiber.Sleep(Interval);
                DoSomething();
            }
        }
    }
}
