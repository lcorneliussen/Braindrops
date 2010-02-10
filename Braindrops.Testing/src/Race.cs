using System;
using System.Collections.Generic;

namespace Braindrops.Testing
{
    public enum RaceControlCommand
    {
        Run,
        RunAsync,
        Join
    }

    public class RaceControl<RunnerType>
    {
        public RaceControl(RaceControlCommand command, RunnerType runner) : this(command, runner, null)
        {
        }

        public RaceControl(RaceControlCommand command, RunnerType runner, Action asyncAction)
        {
            Command = command;
            Runner = runner;
            AsyncAction = asyncAction;
        }

        public RaceControlCommand Command { get; private set; }
        public RunnerType Runner { get; private set; }
        public Action AsyncAction { get; private set; }
    }

    public class Race<RunnerType> : IDisposable
    {
        private readonly Dictionary<RunnerType, StopAndGoThread> _threads =
            new Dictionary<RunnerType, StopAndGoThread>();

        #region IDisposable Members

        public void Dispose()
        {
            foreach (var pair in _threads)
            {
                pair.Value.Dispose();
            }

            _threads.Clear();
        }

        #endregion

        public void Execute(Func<Race<RunnerType>, IEnumerable<RaceControl<RunnerType>>> action)
        {
            IEnumerator<RaceControl<RunnerType>> enumerator = action(this).GetEnumerator();

            if (!enumerator.MoveNext())
            {
                return;
            }

            bool done = false;
            do
            {
                RaceControl<RunnerType> current = enumerator.Current;
                StopAndGoThread thread = getThread(current.Runner);
                switch (current.Command)
                {
                    case RaceControlCommand.Run:
                        thread.Run(() => done = !enumerator.MoveNext());
                        break;
                    case RaceControlCommand.RunAsync:
                        thread.RunAsync(current.AsyncAction);
                        done = !enumerator.MoveNext();
                        break;
                    case RaceControlCommand.Join:
                        thread.Join();
                        done = !enumerator.MoveNext();
                        break;
                }
            } while (!done);
        }

        public RaceControl<RunnerType> RunAs(RunnerType runner)
        {
            return new RaceControl<RunnerType>(RaceControlCommand.Run, runner);
        }

        public RaceControl<RunnerType> RunAsync(RunnerType runner, Action asyncAction)
        {
            return new RaceControl<RunnerType>(RaceControlCommand.RunAsync, runner, asyncAction);
        }

        public RaceControl<RunnerType> Join(RunnerType runner)
        {
            return new RaceControl<RunnerType>(RaceControlCommand.Join, runner);
        }

        private StopAndGoThread getThread(RunnerType runner)
        {
            StopAndGoThread thread;
            if (!_threads.TryGetValue(runner, out thread))
            {
                thread = new StopAndGoThread(runner.ToString());
                _threads[runner] = thread;
            }

            return thread;
        }
    }
}