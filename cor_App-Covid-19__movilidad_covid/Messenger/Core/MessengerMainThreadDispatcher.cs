using System;
using System.Reflection;

namespace Messenger.Core
{
	public abstract class MessengerMainThreadDispatcher : MessengerSingleton<IMessengerMainThreadDispatcher>
	{
		protected static void ExceptionMaskedAction(Action action)
		{
			try
			{
				action();
			}
			catch (TargetInvocationException exception)
			{
				//Console.WriteLine("TargetInvocateException masked " + exception.InnerException.ToString());
			}
			catch (Exception exception)
			{
				// note - all exceptions masked!
				//Console.WriteLine("Exception masked " + exception.ToString());
			}
		}
	}
}
