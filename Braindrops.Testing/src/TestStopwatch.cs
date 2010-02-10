using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Braindrops.Testing
{
    public class TestStopwatch : IDisposable
    {
        private static int _counter;
        private readonly Stopwatch _currentWatch;
        private readonly int _id;

        private readonly List<decimal> _lapsInMilliseconds = new List<decimal>();

        private readonly Action<string> _log = s => Console.WriteLine(s);
        private readonly int _millisecondDecimals;
        private long _lapSum;
        private long? _pausedAt;
        private long _pausedInLap;
        private long _pausedTotal;

        public TestStopwatch() : this(true, 8)
        {
        }

        public TestStopwatch(bool writeLog, int millisecondDecimals)
        {
            _millisecondDecimals = millisecondDecimals;
            if (!writeLog)
            {
                _log = null;
            }

            _id = ++_counter;
            _currentWatch = new Stopwatch();

            if (_log != null)
            {
                _log(string.Format("Start measuring #{0}", _id));
            }

            _currentWatch.Start();
        }

        public int LapCount
        {
            get { return _lapsInMilliseconds.Count; }
        }

        public decimal LapAverage
        {
            get { return Math.Round(_lapsInMilliseconds.Average(), _millisecondDecimals); }
        }

        public decimal SlowestLap
        {
            get { return Math.Round(_lapsInMilliseconds.Max(), _millisecondDecimals); }
        }

        public decimal FastesLap
        {
            get { return Math.Round(_lapsInMilliseconds.Min(), _millisecondDecimals); }
        }

        public decimal Median
        {
            get
            {
                return Math.Round((from d in _lapsInMilliseconds orderby d select d)
                                      .Skip((int) Math.Floor((double) LapCount/2))
                                      .Take(1 + (LapCount%2)).Average(), _millisecondDecimals);
            }
        }

        public decimal CurrentLap
        {
            get
            {
                long lap = CurrentLapTicks;

                decimal lapMs = lap/((decimal) Stopwatch.Frequency/1000);
                return Math.Round(lapMs, _millisecondDecimals);
            }
        }

        protected long CurrentLapTicks
        {
            get
            {
                long elapsed = _currentWatch.ElapsedTicks - _lapSum;
                return elapsed - _pausedInLap;
            }
        }

        protected long ElapsedTicks
        {
            get { return (_pausedAt ?? _currentWatch.ElapsedTicks) - _pausedTotal; }
        }

        public decimal Elapsed
        {
            get
            {
                decimal ms = ElapsedTicks/((decimal) Stopwatch.Frequency/1000);
                return Math.Round(ms, _millisecondDecimals);
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            checkNotPaused();

            _currentWatch.Stop();

            if (_log != null)
            {
                _log(string.Format("Time elapsed in ms (#{0}): {1}", _id, Elapsed));
            }
        }

        #endregion

        public void NewLap()
        {
            checkNotPaused();

            long elapsed = _currentWatch.ElapsedTicks;
            long lap = elapsed - _lapSum - _pausedInLap;

            _lapSum = elapsed;
            _pausedInLap = 0;

            using (Pause())
            {
                decimal lapMs = lap/((decimal) Stopwatch.Frequency/1000);
                _lapsInMilliseconds.Add(lapMs);

                if (_log != null)
                {
                    _log(string.Format("Lap {0} of #{1} took: {2}ms", _lapsInMilliseconds.Count, _id, Elapsed));
                }
            }
        }

        public static void MeasureLaps(string name, int lapCount, Action lapAction)
        {
            MeasureLaps(name, lapCount, w => lapAction());
        }

        public static void MeasureLaps(string name, int lapCount, Action<TestStopwatch> lapAction)
        {
            var sw = new TestStopwatch(false, 6);
            lapCount.Times(
                              () =>
                                  {
                                      lapAction(sw);
                                      sw.NewLap();
                                  });
            sw.Dispose();

            decimal average = sw.LapAverage;
            decimal max = sw.SlowestLap;
            decimal min = sw.FastesLap;
            decimal sum = sw.Elapsed;
            decimal median = sw.Median;

            Console.WriteLine("{0,-50} {1,15}ms, Median: {5,12}ms, Average: {2,12}ms, Max: {4,12}ms, Min: {3,12}ms",
                              lapCount + "x " + name, sum, average, min, max, median);
        }

        public IDisposable Pause()
        {
            checkNotPaused();
            _pausedAt = _currentWatch.ElapsedTicks;
            return new PauseHelper(this);
        }

        private void checkNotPaused()
        {
            if (_pausedAt.HasValue)
                throw new InvalidOperationException("Paused. Resume to proceed!");
        }

        public void Resume()
        {
            if (!_pausedAt.HasValue)
            {
                throw new InvalidOperationException("Not paused. Can't resume!");
            }

            long pausedFor = (_currentWatch.ElapsedTicks - _pausedAt.Value);
            _pausedTotal += pausedFor;
            _pausedInLap += pausedFor;

            _pausedAt = null;
        }

        #region Nested type: PauseHelper

        private struct PauseHelper : IDisposable
        {
            private readonly TestStopwatch _watch;

            public PauseHelper(TestStopwatch watch)
            {
                _watch = watch;
            }

            #region IDisposable Members

            public void Dispose()
            {
                _watch.Resume();
            }

            #endregion
        }

        #endregion
    }
}