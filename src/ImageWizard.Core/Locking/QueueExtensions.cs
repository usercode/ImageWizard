// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ImageWizard.Core.Locking;

public static class QueueExtensions
{
    public static TaskCompletionSource<T> Enqueue<T>(this Queue<TaskCompletionSource<T>> queue, CancellationToken cancellationToken)
    {
        TaskCompletionSource<T> item = new TaskCompletionSource<T>(TaskCreationOptions.RunContinuationsAsynchronously);

        queue.Enqueue(item);

        if (cancellationToken.CanBeCanceled)
        {
            CancellationTokenRegistration registerCallback = cancellationToken.Register(() =>
            {
                item.TrySetCanceled();
            });

            item.Task.ContinueWith(_ =>
            {
                registerCallback.Dispose();
            },
            CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
        }

        return item;
    }
}
