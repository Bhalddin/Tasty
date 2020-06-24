﻿using System;
using System.Linq;

namespace Xenial.Delicious.Execution.TestGroupMiddleware
{
    public static class ExecuteTestGroupMiddleware
    {
        public static TestExecutor UseTestGroupExecutor(this TestExecutor executor)
            => executor.UseGroup(async (context, next) =>
            {
                try
                {
                    var result = await context.CurrentGroup.Executor();
                    if (result)
                    {
                        await next();
                        context.CurrentGroup.TestOutcome = Metadata.TestOutcome.Success;
                    }
                }
                catch (Exception ex)
                {
                    context.CurrentGroup.Exception = ex;
                    context.CurrentGroup.TestOutcome = Metadata.TestOutcome.Failed;
                }
            });
    }
}
