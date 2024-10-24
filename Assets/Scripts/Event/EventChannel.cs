using System;
using System.Collections.Generic;

namespace Event
{
    public class EventChannel
    {
        private readonly Dictionary<Type, Action> _actionsDictionary = new();
        private readonly Dictionary<Type, IEventData> _eventDataDictionary = new();


        public EventChannel()
        {
        }


        public void AddAction<T>() where T : IEventData
        {
            _actionsDictionary[typeof(T)] = null;
        }

        public void Subscribe<T>(Action action) where T : IEventData
        {
            var type = typeof(T);
            if (_actionsDictionary.TryGetValue(type, out var outAction) == false)
            {
                _actionsDictionary[type] = null;
            }

            _actionsDictionary[type] += action;
        }

        public void UnSubscribe<T>(Action action)
        {
            _actionsDictionary[typeof(T)] -= action;
        }

        public T GetData<T>() where T : IEventData
        {
            return (T)_eventDataDictionary[typeof(T)];
        }

        public void Rise<T>(IEventData data)
        {
            if (_actionsDictionary.TryGetValue(typeof(T), out Action value))
            {
                var type = typeof(T);

                _eventDataDictionary[type] = data;

                value?.Invoke();

                _eventDataDictionary[type] = null;
            }
        }
    }

    public interface IEventData
    {
    }
}