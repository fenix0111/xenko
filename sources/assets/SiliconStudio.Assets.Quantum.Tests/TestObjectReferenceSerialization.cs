﻿// Copyright (c) 2011-2017 Silicon Studio Corp. All rights reserved. (https://www.siliconstudio.co.jp)
// See LICENSE.md for full license information.
using System.IO;
using NUnit.Framework;
using SiliconStudio.Assets.Quantum.Tests.Helpers;
using SiliconStudio.Assets.Tests.Helpers;
using SiliconStudio.Quantum;

// ReSharper disable ConvertToLambdaExpression

namespace SiliconStudio.Assets.Quantum.Tests
{
    [TestFixture]
    public class TestObjectReferenceSerialization
    {
        private const string SimpleReferenceYaml = @"!SiliconStudio.Assets.Quantum.Tests.Helpers.Types+MyAssetWithRef,SiliconStudio.Assets.Quantum.Tests
Id: 00000001-0001-0000-0100-000001000000
Tags: []
MyObject1:
    Value: MyInstance
    Id: 00000002-0002-0000-0200-000002000000
MyObject2: ref!! 00000002-0002-0000-0200-000002000000
MyObjects: {}
MyNonIdObjects: []
";

        [Test]
        public void TestSimpleReference()
        {
            Types.AssetWithRefPropertyGraphDefinition.IsObjectReferenceFunc = (targetNode, index) =>
            {
                return (targetNode as IMemberNode)?.Name == nameof(Types.MyAssetWithRef.MyObject2);
            };
            var obj = new Types.MyReferenceable { Id = GuidGenerator.Get(2), Value = "MyInstance" };
            var asset = new Types.MyAssetWithRef { MyObject1 = obj, MyObject2 = obj };
            var context = new AssetTestContainer<Types.MyAssetWithRef, Types.MyAssetBasePropertyGraph>(asset);
            context.BuildGraph();
            SerializationHelper.SerializeAndCompare(context.AssetItem, context.Graph, SimpleReferenceYaml, false);

            context = AssetTestContainer<Types.MyAssetWithRef, Types.MyAssetBasePropertyGraph>.LoadFromYaml(SimpleReferenceYaml);
            Assert.AreEqual(context.Asset.MyObject1, context.Asset.MyObject2);
            Assert.AreEqual(GuidGenerator.Get(2), context.Asset.MyObject1.Id);
        }
    }
}
