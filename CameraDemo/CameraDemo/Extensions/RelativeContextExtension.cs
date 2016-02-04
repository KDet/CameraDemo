using System;
using CameraDemo.XamFormsUI.Behaviors;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CameraDemo.XamFormsUI.Extensions
{
    /// <summary>
    /// Custom markup extension that gets the BindingContext of a UI element
    /// </summary>
    /// <remarks>
    /// Source https://github.com/corradocavalli/Xamarin.Forms.Behaviors/blob/master/Xamarin.Behaviors/Extensions/RelativeContextExtension.cs
    /// </remarks>
    [ContentProperty("Name")]
    public class RelativeContextExtension : IMarkupExtension
    {
        private BindableObject _attachedObject;
        private Element _rootElement;

        /// <summary>Gets or sets the element name.</summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        private void OnContextChanged(object sender, EventArgs e)
        {
            //If used with EventToCommand, markup extension automatically acts on CommandNameContext
            EventToCommandBehavior command = _attachedObject as EventToCommandBehavior;
            if (command != null)
                command.CommandNameContext = _rootElement.BindingContext;
            else
                _attachedObject.BindingContext = _rootElement.BindingContext;
        }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
                throw new ArgumentNullException(nameof(serviceProvider));
            IRootObjectProvider rootObjectProvider =
                serviceProvider.GetService(typeof(IRootObjectProvider)) as IRootObjectProvider;
            if (rootObjectProvider == null)
                throw new ArgumentException("serviceProvider does not provide an IRootObjectProvider");
            if (string.IsNullOrEmpty(Name)) throw new ArgumentNullException(nameof(Name));

            Element nameScope = rootObjectProvider.RootObject as Element;
            Element element = nameScope.FindByName<Element>(Name);
            if (element == null)
                throw new ArgumentNullException($"Can't find element named '{Name}'");
            object context = element.BindingContext;

            _rootElement = element;
            IProvideValueTarget ipvt = (IProvideValueTarget)serviceProvider.GetService(typeof(IProvideValueTarget));
            _attachedObject = (BindableObject)ipvt.TargetObject;
            _attachedObject.BindingContextChanged += OnContextChanged;

            return context ?? new object();
        }
    }
}
