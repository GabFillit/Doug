using Doug.Services;
using Doug.StateMachines;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using Doug.Models;

namespace Test.StateMachines
{
    [TestClass]
    public class BotStateMachineTest
    {
        private BotStateMachine BotStateMachine;

        private Mock<TimeService> TimeServiceMock = new Mock<TimeService>();
        private Mock<MessagingService> MessagingServiceMock = new Mock<MessagingService>();
        private List<User> participants;

        private readonly User BOB = new User("12hd89dj20m13");
        private readonly User MONIQUE = new User("as8120d1d0918dh");
        private readonly User ROBERT = new User("dj8d8723d98d");

        [TestInitialize]
        public void Setup()
        {
            participants = new List<User>();
            participants.Add(BOB);
            participants.Add(MONIQUE);
            TimeServiceMock.Setup(timer => timer.IsCoffeeTime(It.IsAny<TimeZoneInfo>())).Returns(true);
        }

        private void CreateStateMachine()
        {
            BotStateMachine = new BotStateMachine(TimeServiceMock.Object, MessagingServiceMock.Object, participants);
        }

        [TestMethod]
        public void WhenCreatingStateMachine_ThenIsInIdleState()
        {
            CreateStateMachine();
            Assert.AreEqual(BotStateMachine.State.Idle, BotStateMachine.GetCurrentState());
        }

        [TestMethod]
        public void GivenValidTime_WhenMachineReceiveCoffeeEmoji_ThenItGoesInCoffeeBuilding()
        {
            CreateStateMachine();

            BotStateMachine.CoffeeEmoji(BOB);
            Assert.AreEqual(BotStateMachine.State.CoffeeBreakBuilding, BotStateMachine.GetCurrentState());
        }

        [TestMethod]
        public void GivenInvalidTime_WhenMachineReceiveCoffeeEmoji_ThenItStayIdle()
        {
            TimeServiceMock.Setup(timer => timer.IsCoffeeTime(It.IsAny<TimeZoneInfo>())).Returns(false);
            CreateStateMachine();

            BotStateMachine.CoffeeEmoji(BOB);
            Assert.AreEqual(BotStateMachine.State.Idle, BotStateMachine.GetCurrentState());
        }

        [TestMethod]
        public void GivenOneParticipant_WhenMachineReceiveOneReady_ThenItsCoffeeBreak()
        {
            participants = new List<User>();
            participants.Add(BOB);
            CreateStateMachine();

            BotStateMachine.CoffeeEmoji(BOB);
            Assert.AreEqual(BotStateMachine.State.CoffeeBreak, BotStateMachine.GetCurrentState());
        }

        [TestMethod]
        public void GivenTwoParticipants_WhenMachineReceiveOneReady_ThenItStayInCoffeeBuilding()
        {
            CreateStateMachine();

            BotStateMachine.CoffeeEmoji(BOB);
            Assert.AreEqual(BotStateMachine.State.CoffeeBreakBuilding, BotStateMachine.GetCurrentState());
        }

        [TestMethod]
        public void GivenTwoParticipants_WhenMachineReceiveTwoReady_ThenItsCoffeeBreak()
        {
            CreateStateMachine();

            BotStateMachine.CoffeeEmoji(BOB);
            BotStateMachine.CoffeeEmoji(MONIQUE);
            Assert.AreEqual(BotStateMachine.State.CoffeeBreak, BotStateMachine.GetCurrentState());
        }

        [TestMethod]
        public void GivenTwoParticipantsAndOneReady_WhenSkippingOne_ThenItsCoffeeBreak()
        {
            CreateStateMachine();

            BotStateMachine.CoffeeEmoji(MONIQUE);
            BotStateMachine.Skip(BOB);

            Assert.AreEqual(BotStateMachine.State.CoffeeBreak, BotStateMachine.GetCurrentState());
        }

        [TestMethod]
        public void GivenMultipleParticipantsAndTwoReady_WhenSkippingOneMultipleTime_ThenItsCoffeeBreak()
        {
            participants.Add(ROBERT);
            CreateStateMachine();

            BotStateMachine.CoffeeEmoji(MONIQUE);
            BotStateMachine.Skip(BOB);
            BotStateMachine.Skip(BOB);
            BotStateMachine.Skip(BOB);
            BotStateMachine.Skip(BOB);
            BotStateMachine.CoffeeEmoji(ROBERT);

            Assert.AreEqual(BotStateMachine.State.CoffeeBreak, BotStateMachine.GetCurrentState());
        }

        [TestMethod]
        public void GivenUnreadyParticipants_WhenResolve_ThenItsCoffeeBreak()
        {
            participants.Add(ROBERT);
            CreateStateMachine();
            BotStateMachine.CoffeeEmoji(MONIQUE);

            BotStateMachine.Resolve();

            Assert.AreEqual(BotStateMachine.State.CoffeeBreak, BotStateMachine.GetCurrentState());
        }

        [TestMethod]
        public void GivenUnreadyParticipantsAndOneReady_WhenTimeout_ThenGoInRemindState()
        {
            CreateStateMachine();
            BotStateMachine.CoffeeEmoji(MONIQUE);

            TimeServiceMock.Verify(timeService => timeService.Timeout(It.IsAny<int>(), It.IsAny<Action>()));
            BotStateMachine.Remind();

            Assert.AreEqual(BotStateMachine.State.CoffeeRemind, BotStateMachine.GetCurrentState());
        }

        [TestMethod]
        public void GivenReminding_WhenEnter_ThenSendRemindMessage()
        {
            CreateStateMachine();
            BotStateMachine.CoffeeEmoji(MONIQUE);

            BotStateMachine.Remind();

            MessagingServiceMock.Verify(messageService => messageService.SendMessage(It.IsAny<Message>()));
        }

        [TestMethod]
        public void GivenReminding_WhenCoffeeEmoji_ThenGoInCoffeeBuilding()
        {
            participants.Add(ROBERT);
            CreateStateMachine();
            BotStateMachine.CoffeeEmoji(MONIQUE);
            BotStateMachine.Remind();

            BotStateMachine.CoffeeEmoji(BOB);

            Assert.AreEqual(BotStateMachine.State.CoffeeBreakBuilding, BotStateMachine.GetCurrentState());
        }

        [TestMethod]
        public void GivenRemindingLastParticipant_WhenLastParticipantDoesCoffeeEmoji_ThenGoInCoffeeBreak()
        {
            CreateStateMachine();
            BotStateMachine.CoffeeEmoji(MONIQUE);
            BotStateMachine.Remind();

            BotStateMachine.CoffeeEmoji(BOB);

            Assert.AreEqual(BotStateMachine.State.CoffeeBreak, BotStateMachine.GetCurrentState());
        }
    }
}
