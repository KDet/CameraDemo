using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows.Input;
using Xamarin.Forms;

namespace CameraDemo.XamFormsUI.Behaviors
{
    //Refactored from https://github.com/corradocavalli/Xamarin.Forms.Behaviors/blob/master/Xamarin.Behaviors/Library/EventToCommand.cs
    public abstract class EventToCommandBehavior<TBindable> : Behavior<TBindable>
        where TBindable : BindableObject
    {
        public static readonly BindableProperty EventNameProperty =
            BindableProperty.Create<EventToCommandBehavior<TBindable>, string>(p => p.EventName, string.Empty);
        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create<EventToCommandBehavior<TBindable>, ICommand>(p => p.Command, null);
        public static readonly BindableProperty CommandParameterProperty =
            BindableProperty.Create<EventToCommandBehavior<TBindable>, object>(p => p.CommandParameter, null);
        public static readonly BindableProperty CommandNameProperty =
            BindableProperty.Create<EventToCommandBehavior<TBindable>, string>(p => p.CommandName, null);
        public static readonly BindableProperty CommandNameContextProperty =
            BindableProperty.Create<EventToCommandBehavior<TBindable>, object>(p => p.CommandNameContext, null);

        private Delegate _handler;
        private EventInfo _eventInfo;

        public string EventName
        {
            get { return (string)GetValue(EventNameProperty); }
            set { SetValue(EventNameProperty, value); }
        }
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }
        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }
        public string CommandName
        {
            get { return (string)GetValue(CommandNameProperty); }
            set { SetValue(CommandNameProperty, value); }
        }
        public object CommandNameContext
        {
            get { return GetValue(CommandNameContextProperty); }
            set { SetValue(CommandNameContextProperty, value); }
        }

        protected override void OnAttachedTo(BindableObject bindable)
        {
            base.OnAttachedTo(bindable);
            IEnumerable<EventInfo> runtimeEvents = bindable.GetType().GetRuntimeEvents();
            var events = runtimeEvents as EventInfo[] ?? runtimeEvents.ToArray();
            if (events.Length == 0)
                return;
            _eventInfo = events.FirstOrDefault(e => e.Name == EventName);
            if (_eventInfo == null)
                throw new ArgumentException(
                    $"EventToCommand: Can't find any event named '{EventName}' on attached type");
            AddEventHandler(_eventInfo, bindable, OnFired);
        }
        protected override void OnDetachingFrom(BindableObject bindable)
        {
            base.OnDetachingFrom(bindable);
            if (_handler == null)
                return;
            _eventInfo.RemoveEventHandler(bindable, _handler);
        }
        protected virtual void OnFired(EventArgs args)
        {
            if (!string.IsNullOrEmpty(CommandName) && Command == null)
                CreateRelativeBinding();
            if (Command == null)
                throw new InvalidOperationException("No command available, Is Command properly properly set up?");
            if (!Command.CanExecute(CommandParameter))
                return;
            Command.Execute(CommandParameter);
        }

        private void AddEventHandler(EventInfo eventInfo, object item, Action<EventArgs> action)
        {
            List<ParameterExpression> list =
                eventInfo.EventHandlerType.GetRuntimeMethods()
                    .First(rtm => rtm.Name == "Invoke")
                    .GetParameters()
                    .Select(p => Expression.Parameter(p.ParameterType))
                    .ToList();

            ParameterExpression eventArgs =
                list.FirstOrDefault(par => par.Type.GetTypeInfo().IsSubclassOf(typeof(EventArgs)));

            Expression body = Expression.Call(Expression.Constant(this), action.GetMethodInfo(), eventArgs);
            _handler = Expression.Lambda(eventInfo.EventHandlerType, body, list).Compile();
            eventInfo.AddEventHandler(item, _handler);
        }
        private void CreateRelativeBinding()
        {
            if (CommandNameContext == null)
                throw new ArgumentNullException(
                    "CommandNameContext property cannot be null when using CommandName property, " +
                    "consider using CommandNameContext={b:RelativeContext [ElementName]} markup markup extension.");
            if (Command != null)
                throw new InvalidOperationException(
                    "Both Command and CommandName properties specified, only one mode supported.");
            PropertyInfo runtimeProperty = CommandNameContext.GetType().GetRuntimeProperty(CommandName);
            if (runtimeProperty == null)
                throw new ArgumentNullException($"Can't find a command named '{CommandName}'");
            Command = runtimeProperty.GetValue(CommandNameContext) as ICommand;
            if (Command == null)
                throw new ArgumentNullException($"Can't create binding with CommandName '{CommandName}'");
        }
    }

    public class EventToCommandBehavior : EventToCommandBehavior<BindableObject> { }
}
