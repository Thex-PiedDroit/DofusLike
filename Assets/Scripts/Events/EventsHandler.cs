
using System;
using System.Collections.Generic;
using UnityEngine.Assertions;


public class EventsHandler<T> where T : Enum
{
#region Variables (private)

	private struct QueuedEvent
	{
		public T m_eventType;
		public object m_data;


		public QueuedEvent(T eventType, object data)
		{
			m_eventType = eventType;
			m_data = data;
		}
	}


	private Dictionary<int /*iEventID*/, List<IEventsListener<T>>> m_listeners = null;
	private List<T> m_events = null;

	private Queue<QueuedEvent> m_queuedEventTriggers = null;

	private bool m_isDispatchingEvent = false;

	#endregion


	public EventsHandler()
	{
		m_listeners = new Dictionary<int, List<IEventsListener<T>>>();
		m_events = new List<T>();
		m_queuedEventTriggers = new Queue<QueuedEvent>();
	}

	public void Register(IEventsListener<T> listener, T[] events)
	{
		for (int i = 0; i < events.Length; ++i)
		{
			T eventType = events[i];

			int eventID = GetEventID(eventType);

			if (!m_listeners.ContainsKey(eventID))
			{
				m_events.Add(eventType);
				m_listeners[eventID] = new List<IEventsListener<T>>();
			}
			else if (m_listeners[eventID].Contains(listener))
			{
				continue;
			}

			m_listeners[eventID].Add(listener);
		}
	}

	public void Unregister(IEventsListener<T> listener, T[] events)
	{
		for (int i = 0; i < events.Length; ++i)
		{
			T eventType = events[i];

			int eventID = GetEventID(eventType);

			int listenerPlaceInList = m_listeners[eventID].Find(listener);

			if (listenerPlaceInList != -1)
			{
				m_listeners[eventID].RemoveSwapLast(listenerPlaceInList);
			}
		}
	}

	public void Dispatch(T eventType, object data)
	{
		if (m_isDispatchingEvent)
		{
			EnqueueEvent(eventType, data);
			return;
		}

		DoDispatchEvent(eventType, data);
	}

	private void DoDispatchEvent(T eventType, object data)
	{
		int eventID = GetEventID(eventType);
		if (!m_listeners.ContainsKey(eventID))
			return;

		m_isDispatchingEvent = true;

		DispatchToListeners(m_listeners[eventID], eventType, data);

		if (m_queuedEventTriggers.Count > 0)
			DispatchNextQueuedEvent();

		m_isDispatchingEvent = false;
	}

	private void DispatchToListeners(List<IEventsListener<T>> listeners, T eventType, object data)
	{
		for (int i = 0, n = listeners.Count; i < n; ++i)
		{
			Assert.IsNotNull(listeners[i], string.Format("Someone is still listening to event \"{0}\" but is now null. Please make sure to unregister OnDestroy.", eventType.ToString()));

			listeners[i].HandleEvent(eventType, data);
		}
	}

	private void EnqueueEvent(T eventType, object data)
	{
		m_queuedEventTriggers.Enqueue(new QueuedEvent(eventType, data));
	}

	private void DispatchNextQueuedEvent()
	{
		QueuedEvent nextEvent = m_queuedEventTriggers.Dequeue();
		Dispatch(nextEvent.m_eventType, nextEvent.m_data);
	}

	static private int GetEventID(T eventType)
	{
		return eventType.GetHashCode();
	}
}

public interface IEventsListener<T>
{
	void HandleEvent(T eventType, object data);
}
