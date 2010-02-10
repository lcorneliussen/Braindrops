using System;
using System.Threading;

namespace Braindrops.Testing
{
    public class StopAndGoThread : IDisposable
    {
        private readonly ManualResetEvent _go = new ManualResetEvent(false);
        private readonly string _name;
        private readonly ManualResetEvent _stop = new ManualResetEvent(false);
        private readonly Thread _thread;
        private readonly bool _write;
        private bool _abortOnNextRun;
        private int _counter;
        private Exception _error;

        private Action _next;
        private bool _running;
        private bool _started;

        public StopAndGoThread() : this(null, true)
        {
        }

        public StopAndGoThread(string name) : this(name, true)
        {
        }

        public StopAndGoThread(string name, bool write)
        {
            _write = write;
            _thread = new Thread(runner);
            _name = name ?? "thread#" + _thread.ManagedThreadId;
            _thread.Name = _name;
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (_started)
            {
                Join();

                _abortOnNextRun = true;
                _go.Set();
                _thread.Join();
            }

            if (_write)
                Console.WriteLine("Thread '{0}' disposed. Ran {1} task{2}.", _name, _counter,
                                  _counter > 1 ? "s" : string.Empty);
        }

        #endregion

        private void runner()
        {
            while (_go.WaitOne())
            {
                _go.Reset();

                if (_abortOnNextRun)
                {
                    return;
                }

                try
                {
                    _counter++;
                    if (_write) Console.WriteLine("Run thread '{0}', task {1}.", _name, _counter);
                    _next();
                    if (_write) Console.WriteLine("Exit thread '{0}', task {1}.", _name, _counter);
                    if (_write) Console.WriteLine();
                }
                catch (Exception e)
                {
                    _error = e;
                }

                _stop.Set();
            }
        }

        public void Run(Action action)
        {
            Join();
            Start(action);
            Join();
        }

        private void Start(Action action)
        {
            if (!_started)
            {
                _thread.Start();
                _started = true;
            }
            _next = action;
            _running = true;
            _go.Set();
        }

        public void Join()
        {
            if (!_running)
            {
                return;
            }

            _stop.WaitOne();
            _stop.Reset();

            _running = false;

            if (_error != null)
            {
                Exception e = _error;
                _error = null;
                throw new Exception(string.Format("Error on running task {0} in thread '{1}'.", _counter, _name), e);
            }
        }

        public void RunAsync(Action action)
        {
            Join();
            Start(action);
        }

        public static void RunOnce(string name, Action action)
        {
            using (var t = new StopAndGoThread(name))
            {
                t.Run(action);
            }
        }
    }
}