// Copyright (c) 2011-2017 Silicon Studio Corp. All rights reserved. (https://www.siliconstudio.co.jp)
// See LICENSE.md for full license information.
using System;
using System.Collections.Generic;
using System.Reflection;
using SiliconStudio.Core.Annotations;
using SiliconStudio.Core.Reflection;

namespace SiliconStudio.Core.Yaml
{
    /// <summary>
    /// Base class for Yaml serializer
    /// </summary>
    public abstract class YamlSerializerBase : IDisposable
    {
        // TODO: This code is not robust in case of reloading assemblies into the same process
        protected readonly List<Assembly> RegisteredAssemblies = new List<Assembly>();
        protected readonly object Lock = new object();

        protected YamlSerializerBase()
        {
            AssemblyRegistry.AssemblyRegistered += AssemblyRegistered;
            AssemblyRegistry.AssemblyUnregistered += AssemblyUnregistered;
            foreach (var assembly in AssemblyRegistry.FindAll())
            {
                RegisteredAssemblies.Add(assembly);
            }
        }

        public void Dispose()
        {
            AssemblyRegistry.AssemblyRegistered -= AssemblyRegistered;
            AssemblyRegistry.AssemblyUnregistered -= AssemblyUnregistered;
            lock (Lock)
            {
                RegisteredAssemblies.Clear();
            }
        }

        /// <summary>
        /// Reset the assembly cache used by this class.
        /// </summary>
        public virtual void ResetCache()
        {
        }

        private void AssemblyRegistered(object sender, [NotNull] AssemblyRegisteredEventArgs e)
        {
            // Process only our own assemblies
            if (!e.Categories.Contains(AssemblyCommonCategories.Engine))
                return;

            lock (Lock)
            {
                RegisteredAssemblies.Add(e.Assembly);

                // Reset the current serializer as the set of assemblies has changed
                ResetCache();
            }
        }

        private void AssemblyUnregistered(object sender, [NotNull] AssemblyRegisteredEventArgs e)
        {
            // Process only our own assemblies
            if (!e.Categories.Contains(AssemblyCommonCategories.Engine))
                return;

            lock (Lock)
            {
                RegisteredAssemblies.Remove(e.Assembly);

                // Reset the current serializer as the set of assemblies has changed
                ResetCache();
            }
        }
    }
}
