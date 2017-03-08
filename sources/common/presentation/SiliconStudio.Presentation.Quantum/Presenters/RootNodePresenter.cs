using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SiliconStudio.Core.Annotations;
using SiliconStudio.Core.Reflection;
using SiliconStudio.Quantum;
using SiliconStudio.Quantum.Commands;

namespace SiliconStudio.Presentation.Quantum.Presenters
{
    public class RootNodePresenter : NodePresenterBase
    {
        protected readonly IObjectNode RootNode;

        public RootNodePresenter([NotNull] INodePresenterFactoryInternal factory, [NotNull] IObjectNode rootNode)
            : base(factory, null)
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));
            if (rootNode == null) throw new ArgumentNullException(nameof(rootNode));
            RootNode = rootNode;
            Commands.AddRange(rootNode.Commands);
        }

        public override string Name => "Root";
        public sealed override List<INodeCommand> Commands { get; } = new List<INodeCommand>();
        public override Type Type => RootNode.Type;
        public override Index Index => Index.Empty;
        public override bool IsPrimitive => false;
        public override bool IsEnumerable => RootNode.IsEnumerable;
        public override ITypeDescriptor Descriptor => RootNode.Descriptor;
        public override int? Order => null;
        public override object Value => RootNode.Retrieve();
        public override event EventHandler<ValueChangingEventArgs> ValueChanging;
        public override event EventHandler<ValueChangedEventArgs> ValueChanged;

        protected override IObjectNode ParentingNode => RootNode;

        public override void UpdateValue(object newValue)
        {
            throw new NodePresenterException($"A {nameof(RootNodePresenter)} cannot have its own value updated.");
        }

        public override void AddItem(object value)
        {
            if (!RootNode.IsEnumerable)
                throw new NodePresenterException($"{nameof(RootNodePresenter)}.{nameof(AddItem)} cannot be invoked on objects that are not collection.");

            try
            {
                RootNode.Add(value);
                Refresh();
            }
            catch (Exception e)
            {
                throw new NodePresenterException("An error occurred while adding an item to the node, see the inner exception for more information.", e);
            }
        }

        public override void AddItem(object value, Index index)
        {
            if (!RootNode.IsEnumerable)
                throw new NodePresenterException($"{nameof(RootNodePresenter)}.{nameof(AddItem)} cannot be invoked on objects that are not collection.");

            try
            {
                RootNode.Add(value, index);
                Refresh();
            }
            catch (Exception e)
            {
                throw new NodePresenterException("An error occurred while adding an item to the node, see the inner exception for more information.", e);
            }
        }

        public override void RemoveItem(object value, Index index)
        {
            if (!RootNode.IsEnumerable)
                throw new NodePresenterException($"{nameof(RootNodePresenter)}.{nameof(RemoveItem)} cannot be invoked on objects that are not collection.");

            try
            {
                RootNode.Remove(value, index);
                Refresh();
            }
            catch (Exception e)
            {
                throw new NodePresenterException("An error occurred while removing an item to the node, see the inner exception for more information.", e);
            }
        }

        public override void UpdateItem(object newValue, Index index)
        {
            if (!RootNode.IsEnumerable)
                throw new NodePresenterException($"{nameof(RootNodePresenter)}.{nameof(UpdateItem)} cannot be invoked on objects that are not collection.");

            try
            {
                RootNode.Update(newValue, index);
                Refresh();
            }
            catch (Exception e)
            {
                throw new NodePresenterException("An error occurred while updating an item of the node, see the inner exception for more information.", e);
            }
        }

        internal override Task RunCommand(INodeCommand command, object parameter)
        {
            return command.Execute(RootNode, Index.Empty, parameter);
        }
    }
}
