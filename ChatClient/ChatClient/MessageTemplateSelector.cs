using System.Windows;
using System.Windows.Controls;

namespace ChatClient
{
    public class MessageTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var element = container as FrameworkElement;

            if (element == null || !(item is Message))
            {
                return base.SelectTemplate(item, container);
            }
            if (((Message) item).FirstConnectionId != null)
            {
                return element.FindResource("NotCurrentUserMessage") as DataTemplate;
            }
            if (!((Message) item).MessageText.StartsWith("you: "))
            {
                ((Message) item).MessageText = "you: " + ((Message) item).MessageText;
            }
            return element.FindResource("CurrentUserMessage") as DataTemplate;
        }
    }
}
