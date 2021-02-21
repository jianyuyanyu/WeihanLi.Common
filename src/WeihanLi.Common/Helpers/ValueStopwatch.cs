﻿using System;
using System.Diagnostics;

namespace WeihanLi.Common.Helpers
{
    /// <summary>
    /// Value-type replacement for <see cref="Stopwatch"/> which avoids allocations.
    /// </summary>
    /// <remarks>
    /// Inspired on <seealso href="https://github.com/dotnet/extensions/blob/master/src/Shared/src/ValueStopwatch/ValueStopwatch.cs"/>.
    /// </remarks>
    public struct ValueStopwatch
    {
        private static readonly double _timestampToTicks = TimeSpan.TicksPerSecond / (double)Stopwatch.Frequency;

        private long _startTimestamp, _stopTimestamp;

        private ValueStopwatch(long startTimestamp)
        {
            _startTimestamp = startTimestamp;
            _stopTimestamp = 0;
        }

        /// <summary>Gets the total elapsed time measured by the current instance.</summary>
        /// <returns>A read-only <see cref="T:System.TimeSpan"></see> representing the total elapsed time measured by the current instance.</returns>
        public TimeSpan Elapsed
        {
            get
            {
                var end = _stopTimestamp > 0 ? _stopTimestamp : Stopwatch.GetTimestamp();
                var timestampDelta = end - _startTimestamp;
                var ticks = (long)(_timestampToTicks * timestampDelta);
                return new TimeSpan(ticks);
            }
        }

        /// <summary>Gets a value indicating whether the <see cref="ValueStopwatch"></see> timer is running.</summary>
        /// <returns>true if the <see cref="ValueStopwatch"></see> instance is currently running and measuring elapsed time for an interval; otherwise, false.</returns>
        public bool IsRunning => _stopTimestamp == 0;

        /// <summary>Stops time interval measurement, resets the elapsed time to zero, and starts measuring elapsed time.</summary>
        public void Restart()
        {
            _startTimestamp = Stopwatch.GetTimestamp();
            _stopTimestamp = 0;
        }

        /// <summary>Stops measuring elapsed time for an interval.</summary>
        public void Stop() => _stopTimestamp = Stopwatch.GetTimestamp();

        /// <summary>
        /// Creates a new <see cref="ValueStopwatch"/> that is ready to be used.
        /// </summary>
        public static ValueStopwatch StartNew() => new(Stopwatch.GetTimestamp());
    }
}
