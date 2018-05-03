using System;
using System.Threading;
using System.Threading.Tasks;

namespace XamarinCognitiveServices.Utils
{
    internal delegate Task TimerCallback(object state);

    sealed class Timer : IDisposable
    {
        #region private properties
        static Task CompletedTask = Task.FromResult(false);

        TimerCallback _callback;
        Task _delay;
        bool _disposed;
        int _period;
        object _state;
        CancellationTokenSource _tokenSource;
        #endregion


        #region public methods
        /// <summary>
        /// Initializes a new instance of the <see cref="T:XamarinCognitiveServices.Utils.Timer"/> class.
        /// </summary>
        /// <param name="callback">Callback.</param>
        /// <param name="state">State.</param>
        /// <param name="dueTime">Due time.</param>
        /// <param name="period">Period.</param>
        public Timer(TimerCallback callback, object state, int dueTime, int period)
        {
            _callback = callback;
            _state = state;
            _period = period;
            Reset(dueTime);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:XamarinCognitiveServices.Utils.Timer"/> class.
        /// </summary>
        /// <param name="callback">Callback.</param>
        /// <param name="state">State.</param>
        /// <param name="dueTime">Due time.</param>
        /// <param name="period">Period.</param>
        public Timer(TimerCallback callback, object state, TimeSpan dueTime, TimeSpan period)
            : this(callback, state, (int)dueTime.TotalMilliseconds, (int)period.TotalMilliseconds)
        {
        }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="T:XamarinCognitiveServices.Utils.Timer"/> is reclaimed by garbage collection.
        /// </summary>
        ~Timer()
        {
            Dispose(false);
        }

        /// <summary>
        /// Releases all resource used by the <see cref="T:XamarinCognitiveServices.Utils.Timer"/> object.
        /// </summary>
        /// <remarks>Call <see cref="Dispose"/> when you are finished using the
        /// <see cref="T:XamarinCognitiveServices.Utils.Timer"/>. The <see cref="Dispose"/> method leaves the
        /// <see cref="T:XamarinCognitiveServices.Utils.Timer"/> in an unusable state. After calling
        /// <see cref="Dispose"/>, you must release all references to the
        /// <see cref="T:XamarinCognitiveServices.Utils.Timer"/> so the garbage collector can reclaim the memory that
        /// the <see cref="T:XamarinCognitiveServices.Utils.Timer"/> was occupying.</remarks>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Change the specified dueTime and period.
        /// </summary>
        /// <returns>The change.</returns>
        /// <param name="dueTime">Due time.</param>
        /// <param name="period">Period.</param>
        public void Change(int dueTime, int period)
        {
            this._period = period;
            Reset(dueTime);
        }

        /// <summary>
        /// Change the specified dueTime and period.
        /// </summary>
        /// <returns>The change.</returns>
        /// <param name="dueTime">Due time.</param>
        /// <param name="period">Period.</param>
        public void Change(TimeSpan dueTime, TimeSpan period)
        {
            Change((int)dueTime.TotalMilliseconds, (int)period.TotalMilliseconds);
        }
        #endregion

        #region private methods
        /// <summary>
        /// Dispose the specified cleanUpManagedObjects.
        /// </summary>
        /// <returns>The dispose.</returns>
        /// <param name="cleanUpManagedObjects">If set to <c>true</c> clean up managed objects.</param>
        void Dispose(bool cleanUpManagedObjects)
        {
            if (cleanUpManagedObjects)
            {
                Cancel();
            }
            _disposed = true;
        }

        /// <summary>
        /// Reset the specified due.
        /// </summary>
        /// <returns>The reset.</returns>
        /// <param name="due">Due.</param>
        void Reset(int due)
        {
            Cancel();
            if (due >= 0)
            {
                _tokenSource = new CancellationTokenSource();
                Action tick = null;
                tick = () =>
                {
                    Task.Run(() => _callback(_state));
                    if (!_disposed && _period >= 0)
                    {
                        if (_period > 0)
                            _delay = Task.Delay(_period, _tokenSource.Token);
                        else
                            _delay.ContinueWith(t => tick(), _tokenSource.Token);
                    }
                    if (due > 0)
                        _delay = Task.Delay(due, _tokenSource.Token);
                    else
                        _delay = CompletedTask;
                    _delay.ContinueWith(t => tick(), _tokenSource.Token);
                };
            }
        }

        /// <summary>
        /// Cancel this instance.
        /// </summary>
        void Cancel()
        {
            if (_tokenSource != null)
            {
                _tokenSource.Cancel();
                _tokenSource.Dispose();
                _tokenSource = null;
            }
        }
        #endregion

    }
}
