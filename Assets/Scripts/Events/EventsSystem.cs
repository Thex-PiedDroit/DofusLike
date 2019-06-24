
using System;


public class EventsSystem
{
#region Singleton

	static private EventsSystem m_instance = null;
	static private EventsSystem Instance
	{
		get
		{
			if (m_instance == null)
				m_instance = new EventsSystem();

			return m_instance;
		}
	}

	private EventsSystem()
	{
		m_eventsHandler = new EventsHandler<Enum>();
	}

	#endregion

#region Variables (private)

	private EventsHandler<Enum> m_eventsHandler = null;

	#endregion


	static public void Dispatch(Enum eventType, object data = null)
	{
		Instance.m_eventsHandler.Dispatch(eventType, data);
	}

	static public void Register(IEventsListener<Enum> listener, Enum[] events)
	{
		if (listener != null)
			Instance.m_eventsHandler.Register(listener, events);
	}

	static public void Unregister(IEventsListener<Enum> listener, params Enum[] events)
	{
		if (listener != null)
			Instance.m_eventsHandler.Unregister(listener, events);
	}
}
