using System;
using System.Collections.Generic;
using UnityEngine;

namespace Asteroids.Events
{
    public static class EventManager
    {
        static Dictionary<Type, List<IEventListenerGeneric>> _events;

        static EventManager()
        {
            _events = new Dictionary<Type, List<IEventListenerGeneric>>();
        }

        /// <summary>
        /// Start listening to events
        /// </summary>
        /// <typeparam name="Event">Type of event to listen to</typeparam>
        /// <param name="listener">Register itself as event listener</param>
        public static void Listen<Event>(this IEventListener<Event> listener) where Event : struct
        {
            Type eType = typeof(Event);
            List<IEventListenerGeneric> _eventList;

            if (!_events.ContainsKey(eType))
            {
                _eventList = new List<IEventListenerGeneric>();
                _events.Add(eType, _eventList);
            }
            else
            {
                _eventList = _events[eType];
            }

            if (!_eventList.Contains(listener))
                _eventList.Add(listener);
        }

        /// <summary>
        /// Stop listening to events
        /// </summary>
        /// <typeparam name="Event">Type of event to unlisten to</typeparam>
        /// <param name="listener">Remove itself as event listener</param>
        public static void Unlisten<Event>(this IEventListener<Event> listener) where Event : struct
        {
            List<IEventListenerGeneric> _eventList;

            if (_events.TryGetValue(typeof(Event), out _eventList))
            {
                _eventList.Remove(listener);
            }
            else
            {
                throw new UnityException("Cannot unlisten this listener. It doesn't exist.");
            }
        }

        /// <summary>
        /// Trigger an event
        /// </summary>
        /// <typeparam name="Event">A generic struct</typeparam>
        /// <param name="newEvent">Event message to send to all lintener</param>
        public static void Trigger<Event>(Event newEvent) where Event : struct
        {
            List<IEventListenerGeneric> _eventList;
            if (!_events.TryGetValue(typeof(Event), out _eventList))
                throw new UnityException("'Event' doesn't exist. Make sure to listen to it first.");

            foreach (IEventListener<Event> e in _eventList)
                e.OnEvent(newEvent);
        }
    }

    public interface IEventListenerGeneric { }

    /// <summary>
    /// Inherit this interface to start listening to events.
    /// Don't forget to call "this.Listen(...)"
    /// </summary>
    /// <typeparam name="Event">Type of the event</typeparam>
    public interface IEventListener<Event> : IEventListenerGeneric where Event : struct
    {
        void OnEvent(Event e);
    }
}
