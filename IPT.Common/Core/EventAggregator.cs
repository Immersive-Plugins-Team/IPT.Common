using System;
using System.Collections.Generic;
using System.Linq;

namespace IPT.Common.Core
{
    /// <summary>
    /// A thread-safe event aggregator for decoupled messaging.
    /// Enables components to communicate without holding direct references to each other.
    /// </summary>
    public sealed class EventAggregator
    {
        private readonly Dictionary<Type, List<object>> _subscriptions = new Dictionary<Type, List<object>>();
        private readonly object _lock = new object();

        public EventAggregator() { }

        /// <summary>
        /// Publishes a message to all subscribers of the message type.
        /// </summary>
        public void Publish<TMessage>(TMessage message)
        {
            var messageType = typeof(TMessage);
            List<object> subscribers;

            lock (_lock)
            {
                if (!_subscriptions.ContainsKey(messageType)) return;
                subscribers = _subscriptions[messageType].ToList();
            }

            foreach (var subscriber in subscribers)
            {
                if (subscriber is Action<TMessage> action) action(message);
            }
        }

        /// <summary>
        /// Subscribes a callback to a specific message type.
        /// </summary>
        /// <returns>An IDisposable token that can be used to unsubscribe.</returns>
        public IDisposable Subscribe<TMessage>(Action<TMessage> action)
        {
            var messageType = typeof(TMessage);

            lock (_lock)
            {
                if (!_subscriptions.ContainsKey(messageType)) _subscriptions[messageType] = new List<object>();
                _subscriptions[messageType].Add(action);
            }

            return new Subscription<TMessage>(this, action);
        }

        private void Unsubscribe<TMessage>(Action<TMessage> action)
        {
            var messageType = typeof(TMessage);

            lock (_lock)
            {
                if (_subscriptions.ContainsKey(messageType))
                {
                    _subscriptions[messageType].Remove(action);
                    if (_subscriptions[messageType].Count == 0) _subscriptions.Remove(messageType);
                }
            }
        }

        private class Subscription<TMessage> : IDisposable
        {
            private readonly EventAggregator _eventAggregator;
            private readonly Action<TMessage> _action;
            private bool _isDisposed;

            public Subscription(EventAggregator eventAggregator, Action<TMessage> action)
            {
                _eventAggregator = eventAggregator;
                _action = action;
            }

            public void Dispose()
            {
                if (!_isDisposed)
                {
                    _eventAggregator.Unsubscribe(_action);
                    _isDisposed = true;
                }
            }
        }
    }
}
