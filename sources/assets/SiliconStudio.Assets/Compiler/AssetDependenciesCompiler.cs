﻿// Copyright (c) 2014 Silicon Studio Corp. (http://siliconstudio.co.jp)
// This file is distributed under GPL v3. See LICENSE.md for details.
using System;
using System.Collections.Generic;
using SiliconStudio.Assets.Analysis;
using SiliconStudio.BuildEngine;

namespace SiliconStudio.Assets.Compiler
{
    /// <summary>
    /// An implementation of <see cref="IAssetCompiler"/> that will compile an asset with all its dependencies.
    /// </summary>
    /// <remarks>This class is stateless and can reused or be shared amongst multiple asset compilation</remarks>
    public class AssetDependenciesCompiler : IAssetCompiler
    {
        private static readonly AssetCompilerRegistry assetCompilerRegistry = new AssetCompilerRegistry();

        /// <inheritdoc/>
        public AssetCompilerResult Compile(CompilerContext context, AssetItem assetItem)
        {
            if (context == null) throw new ArgumentNullException("context");
            if (assetItem == null) throw new ArgumentNullException("assetItem");

            var compilerResult = new AssetCompilerResult();

            if (assetItem.Package == null)
            {
                compilerResult.Warning($"Asset [{assetItem}] is not attached to a package");
                return compilerResult;
            }

            var assetCompilerContext = (AssetCompilerContext)context;

            IAssetCompiler mainCompiler;
            try
            {
                mainCompiler = assetCompilerRegistry.GetCompiler(assetItem.Asset.GetType());
            }
            catch (Exception ex)
            {
                compilerResult.Error($"Cannot find a compiler for asset [{assetItem.Id}] from path [{assetItem.Location}]", ex);
                return compilerResult;
            }

            if (mainCompiler == null)
            {
                return compilerResult;
            }

            compilerResult = mainCompiler.Compile(assetCompilerContext, assetItem);
            if (compilerResult.HasErrors)
            {
                return compilerResult;
            }

            var depsCompiler = context.Properties.Get(BuildStepsQueue.PropertyKey);

            depsCompiler.BuildSteps[new AssetBuildOperation(assetItem.Id, assetItem.Version)] = compilerResult.BuildSteps;

            //run time deps
            var dependencies = assetItem.Package.Session.DependencyManager.ComputeDependencies(assetItem.Id, AssetDependencySearchOptions.Out | AssetDependencySearchOptions.Recursive, ContentLinkType.Reference);
            foreach (var assetDependency in dependencies.LinksOut)
            {
                var assetType = assetDependency.Item.Asset.GetType();
                if (mainCompiler.CompileTimeDependencyTypes.Contains(assetType))
                {
                    var result = depsCompiler.CompileAndSubmit(context, compilerResult.BuildSteps, assetDependency.Item, this);
                    if (result.HasErrors)
                    {
                        result.CopyTo(compilerResult);
                        return compilerResult;
                    }
                }
            }

            //compile time
            foreach (var compileTimeDependency in mainCompiler.GetCompileTimeDependencies(assetCompilerContext, assetItem))
            {
                var result = depsCompiler.CompileAndSubmit(context, compilerResult.BuildSteps, compileTimeDependency, this);
                if (result.HasErrors)
                {
                    result.CopyTo(compilerResult);
                    return compilerResult;
                }
            }

            return compilerResult;
        }

        public IEnumerable<AssetItem> GetCompileTimeDependencies(AssetCompilerContext context, AssetItem assetItem)
        {
            yield break;
        }

        public HashSet<Type> CompileTimeDependencyTypes { get; } = new HashSet<Type>();

        /// <summary>
        /// Compiles the given asset with its dependencies.
        /// </summary>
        /// <param name="context">The asset compiler context.</param>
        /// <param name="assetItem">The asset to compile with its dependencies.</param>
        /// <param name="originalItem"></param>
        /// <param name="compilationResult">The result of the compilation.</param>
        protected virtual void CompileWithDependencies(AssetCompilerContext context, AssetItem assetItem, AssetItem originalItem, AssetCompilerResult compilationResult)
        {
            CompilePackage(context, assetItem.Package, compilationResult);
        }

        /// <summary>
        /// Compiles the package contained in the given context and add the resulting build steps in the <see cref="AssetCompilerResult"/>
        /// </summary>
        /// <param name="context">The context which contains the package to compile.</param>
        /// <param name="result">The <see cref="AssetCompilerResult"/> where the build steps will be added.</param>
        /// <returns></returns>
        protected static BuildStep CompilePackage(AssetCompilerContext context, Package package, AssetCompilerResult result)
        {
            // compile the fake package (create the build steps)
            var assetPackageCompiler = new PackageCompiler(new PackageAssetEnumerator(package));
            var dependenciesCompileResult = assetPackageCompiler.Compile(context);

            // Create the result build steps if not existing yet
            if (result.BuildSteps == null)
                result.BuildSteps = new ListBuildStep();

            // Add the dependencies build steps to the current result
            result.BuildSteps.Add(dependenciesCompileResult.BuildSteps);

            // Copy log the dependencies result to the current result
            dependenciesCompileResult.CopyTo(result);

            return dependenciesCompileResult.BuildSteps;
        }
    }
}
