using Doug.Models;
using Doug.Services;
using Stateless;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Doug.StateMachines
{
    public class BotStateMachine : InstanceStateMachine
    {
        public enum State
        {
            Idle,
            CoffeeBreakBuilding,
            CoffeeRemind,
            CoffeePostponed,
            CoffeeBreak
        }

        public enum Event
        {
            CoffeeEmoji,
            CoffeeResolve,
            CoffeeCancel,
            CoffeePostpone,
            CoffeeRemindTimeout,
            CoffeeBreakEnd,
            SkipCommand
        }

        private StateMachine<State, Event> Machine;
        private StateMachine<State, Event>.TriggerWithParameters<User> CoffeeEmojiEvent;
        private StateMachine<State, Event>.TriggerWithParameters<User> SkipCommandEvent;

        private TimeService TimeService;
        private MessagingService MessagingService;
        private readonly List<User> Participants;

        private List<User> Roster;
        private List<User> AvailableParticipants;

        public BotStateMachine(TimeService timeService, MessagingService messagingService, List<User> participants)
        {
            this.TimeService = timeService;
            this.MessagingService = messagingService;
            this.Participants = participants;
            this.Roster = new List<User>();
            this.AvailableParticipants = new List<User>(participants);


            this.Machine = new StateMachine<State, Event>(State.Idle);
            this.CoffeeEmojiEvent = Machine.SetTriggerParameters<User>(Event.CoffeeEmoji);
            this.SkipCommandEvent = Machine.SetTriggerParameters<User>(Event.SkipCommand);

            ConfigureStateMachine();
        }

        public void CoffeeEmoji(User participant)
        {
            Machine.Fire(CoffeeEmojiEvent, participant);
        }

        public State GetCurrentState()
        {
            return Machine.State;
        }

        public void Skip(User user)
        {
            Machine.Fire(SkipCommandEvent, user);
        }

        public void Resolve()
        {
            Machine.Fire(Event.CoffeeResolve);
        }

        public void Remind()
        {
            Machine.Fire(Event.CoffeeRemindTimeout);
        }

        private void ConfigureStateMachine()
        {
            Machine.Configure(State.Idle)
                .PermitIf(CoffeeEmojiEvent, State.CoffeeBreakBuilding, userId => TimeService.IsCoffeeTime(TimeZoneInfo.Local))
                .IgnoreIf(CoffeeEmojiEvent, userId => !TimeService.IsCoffeeTime(TimeZoneInfo.Local));

            Machine.Configure(State.CoffeeBreakBuilding)
                .OnEntry(() => TimeService.Timeout(30000, Remind))
                .OnEntryFrom(CoffeeEmojiEvent, CountParticipant)
                .PermitReentryIf(CoffeeEmojiEvent, userId => true)
                .Permit(Event.CoffeeRemindTimeout, State.CoffeeRemind)
                .Permit(Event.CoffeeCancel, State.Idle)
                .Permit(Event.CoffeePostpone, State.CoffeePostponed)
                .Permit(Event.CoffeeResolve, State.CoffeeBreak)
                .InternalTransition(SkipCommandEvent, (user, t) => OnSkip(user))
                .OnExit(TimeService.CancelTimeout);

            Machine.Configure(State.CoffeeRemind)
                .OnEntry(SendCalloutMessage)
                .Permit(Event.CoffeeEmoji, State.CoffeeBreakBuilding)
                .Permit(Event.CoffeeResolve, State.CoffeeBreak);
        }

        private void CountParticipant(User participant)
        {
            if (Roster.All(user => user != participant)
                && AvailableParticipants.Any(user => user == participant))
            {
                Roster.Add(participant);
            }

            if (IsEveryoneReady()) Machine.Fire(Event.CoffeeResolve);
        }

        private bool IsEveryoneReady()
        {
            return AvailableParticipants.All(user => Roster.Any(participant => participant == user));
        }

        private void OnSkip(User user)
        {
            AvailableParticipants.Remove(user);

            if (IsEveryoneReady()) Machine.Fire(Event.CoffeeResolve);
        }

        private void SendCalloutMessage()
        {
            var message = new Message();
            message.ConversationId = "123123112";
            message.Text = "missing 3 bad bois";
            MessagingService.SendMessage(message);
        }
    }
}
