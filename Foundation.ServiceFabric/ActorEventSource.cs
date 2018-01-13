namespace Foundation.ServiceFabric
{
    using System;
    using System.Diagnostics.Tracing;
    using System.Threading.Tasks;
    using Microsoft.ServiceFabric.Actors.Runtime;

    [EventSource(Name = "Insignia-Foundation-Actors")]
    public sealed class ActorEventSource : EventSource
    {
        public static readonly ActorEventSource Current = new ActorEventSource();

        static ActorEventSource()
        {
            // A workaround for the problem where ETW activities do not get tracked until Tasks infrastructure is initialized.
            // This problem will be fixed in .NET Framework 4.6.2.
            Task.Run(() => { });
        }

        // Instance constructor is private to enforce singleton semantics
        private ActorEventSource() : base() { }

        #region Keywords
        // Event keywords can be used to categorize events. 
        // Each keyword is a bit flag. A single event can be associated with multiple keywords (via EventAttribute.Keywords property).
        // Keywords must be defined as a public class named 'Keywords' inside EventSource that uses them.
        public static class Keywords
        {
            public const EventKeywords HostInitialization = (EventKeywords)0x1L;
            public const EventKeywords Diagnostic = (EventKeywords)0x2L;
            public const EventKeywords Performance = (EventKeywords)0x4L;
            public const EventKeywords Start = (EventKeywords)0x8L;
            public const EventKeywords End = (EventKeywords)0x10L;
        }
        #endregion

        #region Events

        #region ActorHostInitializationFailed

        [NonEvent]
        public void ActorHostInitializationFailed(Exception exception)
        {
            if (!IsEnabled()) return;

            ActorHostInitializationFailed(exception.ToString());
        }

        private const int ActorHostInitializationFailedEventId = 1;
        [Event(ActorHostInitializationFailedEventId, Level = EventLevel.Error, Message = "Actor host initialization failed", Keywords = Keywords.HostInitialization)]
        private void ActorHostInitializationFailed(string exception)
        {
            if (!IsEnabled()) return;

            WriteEvent(ActorHostInitializationFailedEventId, exception);
        }

        #endregion ActorHostInitializationFailed
        #region ActorException

        [NonEvent]
        public void ActorException(Actor actor, Exception exception)
        {
            if (!IsEnabled()) return;

            ActorException(actor.ActorService.ActorTypeInformation.ServiceName, actor.Id.ToString(), exception.ToString());
        }

        private const int ExceptionEventId = 2;
        [Event(ExceptionEventId, Level = EventLevel.Error, Message = "{0}:{1} threw an exception", Keywords = Keywords.Diagnostic)]
        private void ActorException(string actorName, string identity, string exception)
        {
            if (!IsEnabled()) return;

            WriteEvent(ExceptionEventId, actorName, identity, exception);
        }

        #endregion ActorException
        #region Activating

        [NonEvent]
        public void Activating(Actor actor)
        {
            if (!IsEnabled()) return;

            Activating(actor.ActorService.ActorTypeInformation.ServiceName, actor.Id.ToString());
        }

        private const int ActivatingEventId = 3;
        [Event(ActivatingEventId, Level = EventLevel.Informational, Message = "{0}:{1} activating", Keywords = Keywords.Performance | Keywords.Start)]
        private void Activating(string actorName, string actorIdentity)
        {
            if (!IsEnabled()) return;

            WriteEvent(ActivatingEventId, actorName, actorIdentity);
        }

        #endregion Activating
        #region Activated

        [NonEvent]
        public void Activated(Actor actor)
        {
            if (!IsEnabled()) return;

            Activated(actor.ActorService.ActorTypeInformation.ServiceName, actor.Id.ToString());
        }

        private const int ActivatedEventId = 4;
        [Event(ActivatedEventId, Level = EventLevel.Informational, Message = "{0}:{1} activated", Keywords = Keywords.Performance | Keywords.End)]
        private void Activated(string actorName, string actorIdentity)
        {
            if (!IsEnabled()) return;

            WriteEvent(ActivatedEventId, actorName, actorIdentity);
        }

        #endregion Activated
        #region ActorMessage

        [NonEvent]
        public void ActorMessage(Actor actor, string message, params object[] args)
        {
            if (!IsEnabled()) return;

            var finalMessage = string.Format(message, args);
            ActorMessage(actor.ActorService.ActorTypeInformation.ServiceName, actor.Id.ToString(), finalMessage);
        }

        private const int ActorMessageEventId = 5;

        [Event(ActorMessageEventId, Level = EventLevel.Informational, Message = "{0}: {2}")]
        private void ActorMessage(string actorName, string actorIdentity, string message)
        {
            WriteEvent(ActorMessageEventId, actorName, actorIdentity, message);
        }

        #endregion ActorMessage
        #region ActorError

        [NonEvent]
        public void ActorError(Actor actor, string message, params object[] args)
        {
            if (!IsEnabled()) return;

            var finalError = string.Format(message, args);
            ActorError(actor.ActorService.ActorTypeInformation.ServiceName, actor.Id.ToString(), finalError);
        }

        private const int ActorErrorEventId = 6;

        [Event(ActorErrorEventId, Level = EventLevel.Error, Message = "{0}: {2}", Keywords = Keywords.Diagnostic)]
        private void ActorError(string actorName, string actorIdentity, string message)
        {
            WriteEvent(ActorErrorEventId, actorName, actorIdentity, message);
        }

        #endregion ActorError

        #endregion Events
    }
}
