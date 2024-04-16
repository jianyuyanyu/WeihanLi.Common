#if !NET5_0_OR_GREATER
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
 
using System.Collections.Generic;
 
namespace System.Threading.Tasks
{
    /// <summary>
    /// Represents the producer side of a <see cref="Tasks.Task"/> unbound to a
    /// delegate, providing access to the consumer side through the <see cref="Tasks.Task"/> property.
    /// </summary>
    /// <remarks>
    /// <para>
    /// It is often the case that a <see cref="Tasks.Task"/> is desired to
    /// represent another asynchronous operation.
    /// <see cref="TaskCompletionSource">TaskCompletionSource</see> is provided for this purpose. It enables
    /// the creation of a task that can be handed out to consumers, and those consumers can use the members
    /// of the task as they would any other. However, unlike most tasks, the state of a task created by a
    /// TaskCompletionSource is controlled explicitly by the methods on TaskCompletionSource. This enables the
    /// completion of the external asynchronous operation to be propagated to the underlying Task. The
    /// separation also ensures that consumers are not able to transition the state without access to the
    /// corresponding TaskCompletionSource.
    /// </para>
    /// <para>
    /// All members of <see cref="TaskCompletionSource"/> are thread-safe
    /// and may be used from multiple threads concurrently.
    /// </para>
    /// </remarks>
    public class TaskCompletionSource
    {
        private readonly TaskCompletionSource<object?> _taskCompletionSource;

        /// <summary>Creates a <see cref="TaskCompletionSource"/>.</summary>
        public TaskCompletionSource() => _taskCompletionSource = new();


        /// <summary>Creates a <see cref="TaskCompletionSource"/> with the specified options.</summary>
        /// <remarks>
        /// The <see cref="Tasks.Task"/> created by this instance and accessible through its <see cref="Task"/> property
        /// will be instantiated using the specified <paramref name="creationOptions"/>.
        /// </remarks>
        /// <param name="creationOptions">The options to use when creating the underlying <see cref="Tasks.Task"/>.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The <paramref name="creationOptions"/> represent options invalid for use
        /// with a <see cref="TaskCompletionSource"/>.
        /// </exception>
        public TaskCompletionSource(TaskCreationOptions creationOptions) :
            this(null, creationOptions)
        {
        }
 
        /// <summary>Creates a <see cref="TaskCompletionSource"/> with the specified state.</summary>
        /// <param name="state">The state to use as the underlying
        /// <see cref="Tasks.Task"/>'s AsyncState.</param>
        public TaskCompletionSource(object? state) :
            this(state, TaskCreationOptions.None)
        {
        }
 
        /// <summary>Creates a <see cref="TaskCompletionSource"/> with the specified state and options.</summary>
        /// <param name="creationOptions">The options to use when creating the underlying <see cref="Tasks.Task"/>.</param>
        /// <param name="state">The state to use as the underlying <see cref="Tasks.Task"/>'s AsyncState.</param>
        /// <exception cref="ArgumentOutOfRangeException">The <paramref name="creationOptions"/> represent options invalid for use with a <see cref="TaskCompletionSource"/>.</exception>
        public TaskCompletionSource(object? state, TaskCreationOptions creationOptions) =>
            _taskCompletionSource = new(state, creationOptions);
 
        /// <summary>
        /// Gets the <see cref="Tasks.Task"/> created
        /// by this <see cref="TaskCompletionSource"/>.
        /// </summary>
        /// <remarks>
        /// This property enables a consumer access to the <see cref="Task"/> that is controlled by this instance.
        /// The <see cref="SetResult"/>, <see cref="SetException(Exception)"/>, <see cref="SetException(IEnumerable{Exception})"/>,
        /// and <see cref="SetCanceled"/> methods (and their "Try" variants) on this instance all result in the relevant state
        /// transitions on this underlying Task.
        /// </remarks>
        public Task Task => _taskCompletionSource.Task;
 
        /// <summary>Transitions the underlying <see cref="Tasks.Task"/> into the <see cref="TaskStatus.Faulted"/> state.</summary>
        /// <param name="exception">The exception to bind to this <see cref="Tasks.Task"/>.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="exception"/> argument is null.</exception>
        /// <exception cref="InvalidOperationException">
        /// The underlying <see cref="Tasks.Task"/> is already in one of the three final states:
        /// <see cref="TaskStatus.RanToCompletion"/>,
        /// <see cref="TaskStatus.Faulted"/>, or
        /// <see cref="TaskStatus.Canceled"/>.
        /// </exception>
        public void SetException(Exception exception) => _taskCompletionSource.SetException(exception);
 
        /// <summary>Transitions the underlying <see cref="Tasks.Task"/> into the <see cref="TaskStatus.Faulted"/> state.</summary>
        /// <param name="exceptions">The collection of exceptions to bind to this <see cref="Tasks.Task"/>.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="exceptions"/> argument is null.</exception>
        /// <exception cref="ArgumentException">There are one or more null elements in <paramref name="exceptions"/>.</exception>
        /// <exception cref="InvalidOperationException">
        /// The underlying <see cref="Tasks.Task"/> is already in one of the three final states:
        /// <see cref="TaskStatus.RanToCompletion"/>,
        /// <see cref="TaskStatus.Faulted"/>, or
        /// <see cref="TaskStatus.Canceled"/>.
        /// </exception>
        public void SetException(IEnumerable<Exception> exceptions) => _taskCompletionSource.SetException(exceptions);
 
        /// <summary>
        /// Attempts to transition the underlying <see cref="Tasks.Task"/> into the <see cref="TaskStatus.Faulted"/> state.
        /// </summary>
        /// <param name="exception">The exception to bind to this <see cref="Tasks.Task"/>.</param>
        /// <returns>True if the operation was successful; otherwise, false.</returns>
        /// <remarks>
        /// This operation will return false if the <see cref="Tasks.Task"/> is already in one of the three final states:
        /// <see cref="TaskStatus.RanToCompletion"/>,
        /// <see cref="TaskStatus.Faulted"/>, or
        /// <see cref="TaskStatus.Canceled"/>.
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="exception"/> argument is null.</exception>
        public bool TrySetException(Exception exception) => _taskCompletionSource.TrySetException(exception);
 
        /// <summary>
        /// Attempts to transition the underlying <see cref="Tasks.Task"/> into the <see cref="TaskStatus.Faulted"/> state.
        /// </summary>
        /// <param name="exceptions">The collection of exceptions to bind to this <see cref="Tasks.Task"/>.</param>
        /// <returns>True if the operation was successful; otherwise, false.</returns>
        /// <remarks>
        /// This operation will return false if the <see cref="Tasks.Task"/> is already in one of the three final states:
        /// <see cref="TaskStatus.RanToCompletion"/>,
        /// <see cref="TaskStatus.Faulted"/>, or
        /// <see cref="TaskStatus.Canceled"/>.
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="exceptions"/> argument is null.</exception>
        /// <exception cref="ArgumentException">There are one or more null elements in <paramref name="exceptions"/>.</exception>
        /// <exception cref="ArgumentException">The <paramref name="exceptions"/> collection is empty.</exception>
        public bool TrySetException(IEnumerable<Exception> exceptions) => _taskCompletionSource.TrySetException(exceptions);
 
        /// <summary>
        /// Transitions the underlying <see cref="Tasks.Task"/> into the <see cref="TaskStatus.RanToCompletion"/> state.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// The underlying <see cref="Tasks.Task"/> is already in one of the three final states:
        /// <see cref="TaskStatus.RanToCompletion"/>,
        /// <see cref="TaskStatus.Faulted"/>, or
        /// <see cref="TaskStatus.Canceled"/>.
        /// </exception>
        public void SetResult() => _taskCompletionSource.SetResult(null);
 
        /// <summary>
        /// Attempts to transition the underlying <see cref="Tasks.Task"/> into the <see cref="TaskStatus.RanToCompletion"/> state.
        /// </summary>
        /// <returns>True if the operation was successful; otherwise, false.</returns>
        /// <remarks>
        /// This operation will return false if the <see cref="Tasks.Task"/> is already in one of the three final states:
        /// <see cref="TaskStatus.RanToCompletion"/>,
        /// <see cref="TaskStatus.Faulted"/>, or
        /// <see cref="TaskStatus.Canceled"/>.
        /// </remarks>
        public bool TrySetResult() => _taskCompletionSource.TrySetResult(null);
 
        /// <summary>
        /// Transitions the underlying <see cref="Tasks.Task"/> into the <see cref="TaskStatus.Canceled"/> state.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// The underlying <see cref="Tasks.Task"/> is already in one of the three final states:
        /// <see cref="TaskStatus.RanToCompletion"/>,
        /// <see cref="TaskStatus.Faulted"/>, or
        /// <see cref="TaskStatus.Canceled"/>.
        /// </exception>
        public void SetCanceled() => SetCanceled(default);
 
        /// <summary>
        /// Attempts to transition the underlying <see cref="Tasks.Task"/> into the <see cref="TaskStatus.Canceled"/> state.
        /// </summary>
        /// <returns>True if the operation was successful; otherwise, false.</returns>
        /// <remarks>
        /// This operation will return false if the <see cref="Tasks.Task"/> is already in one of the three final states:
        /// <see cref="TaskStatus.RanToCompletion"/>,
        /// <see cref="TaskStatus.Faulted"/>, or
        /// <see cref="TaskStatus.Canceled"/>.
        /// </remarks>
        public bool TrySetCanceled() => TrySetCanceled(default);
 
        /// <summary>
        /// Attempts to transition the underlying <see cref="Tasks.Task"/> into the <see cref="TaskStatus.Canceled"/> state.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token with which to cancel the <see cref="Tasks.Task"/>.</param>
        /// <returns>True if the operation was successful; otherwise, false.</returns>
        /// <remarks>
        /// This operation will return false if the <see cref="Tasks.Task"/> is already in one of the three final states:
        /// <see cref="TaskStatus.RanToCompletion"/>,
        /// <see cref="TaskStatus.Faulted"/>, or
        /// <see cref="TaskStatus.Canceled"/>.
        /// </remarks>
        public bool TrySetCanceled(CancellationToken cancellationToken) => _taskCompletionSource.TrySetCanceled(cancellationToken);
    }
}
#endif