﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SiliconStudio.Core.Annotations;

namespace SiliconStudio.Presentation.Quantum.Presenters
{
    /// <summary>
    /// Base class for node commands that are not asynchronous.
    /// </summary>
    public abstract class SyncNodePresenterCommandBase : NodePresenterCommandBase
    {
        /// <inheritdoc/>
        public sealed override Task Execute(INodePresenter nodePresenter, object parameter, object preExecuteResult)
        {
            ExecuteSync(nodePresenter, parameter, preExecuteResult);
            return Task.CompletedTask;
        }

        protected abstract void ExecuteSync([NotNull] INodePresenter nodePresenter, object parameter, object preExecuteResult);
    }
}