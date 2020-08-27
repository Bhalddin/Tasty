﻿using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using StreamJsonRpc;

using Xenial.Delicious.Scopes;

namespace Xenial.Delicious.Remote
{
    public delegate Task<bool> IsInteractiveRun();

    public delegate Task<TransportStreamFactory?> TransportStreamFactoryFunctor(CancellationToken token = default);

    public delegate Task<Stream> TransportStreamFactory();

    public delegate Task<ITastyRemote> ConnectToRemote(TastyScope scope, Stream remoteStream);

    public static class TastyRemoteDefaults
    {
        public static Task<bool> IsInteractiveRun()
        {
            var isInteractive = Environment.GetEnvironmentVariable(EnvironmentVariables.InteractiveMode);
            if (!string.IsNullOrEmpty(isInteractive))
            {
                if (bool.TryParse(isInteractive, out var result))
                {
                    return Task.FromResult(result);
                }
            }
            return Task.FromResult(false);
        }

        public static Task<ITastyRemote> AttachToStream(TastyScope scope, Stream remoteStream)
        {
            _ = scope ?? throw new ArgumentNullException(nameof(scope));
            _ = remoteStream ?? throw new ArgumentNullException(nameof(remoteStream));

            var remote = JsonRpc.Attach<ITastyRemote>(remoteStream);

            return Task.FromResult(remote);
        }
    }
}
