// Copyright (c) 2014-2017 Silicon Studio Corp. All rights reserved. (https://www.siliconstudio.co.jp)
// See LICENSE.md for full license information.
using System;
using System.Collections;
using System.Collections.Generic;

namespace SiliconStudio.Shaders.Ast.Hlsl
{
    /// <summary>
    /// A technique pass.
    /// </summary>
    public partial class Pass : Node, IAttributes
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "Pass" /> class.
        /// </summary>
        public Pass()
        {
            Attributes = new List<AttributeBase>();
            Items = new List<Expression>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the attributes.
        /// </summary>
        /// <value>
        ///   The attributes.
        /// </value>
        public List<AttributeBase> Attributes { get; set; }

        /// <summary>
        ///   Gets or sets the items.
        /// </summary>
        /// <value>
        ///   The items.
        /// </value>
        /// <remarks>
        ///   An item is either a <see cref = "MethodInvocationExpression" /> or a <see cref = "AssignmentExpression" />.
        /// </remarks>
        public List<Expression> Items { get; set; }

        /// <summary>
        ///   Gets or sets the name.
        /// </summary>
        /// <value>
        ///   The name.
        /// </value>
        public Identifier Name { get; set; }

        #endregion

        #region Public Methods

        /// <inheritdoc />
        public override IEnumerable<Node> Childrens()
        {
            ChildrenList.Clear();
            ChildrenList.Add(Name);
            ChildrenList.AddRange(Attributes);
            ChildrenList.AddRange(Items);
            return ChildrenList;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return string.Format("pass {0}{{...}}", Name != null ? Name + " " : string.Empty);
        }

        #endregion
    }
}
