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
        private List<User> participants;

        private readonly User BOB = new User("12hd89dj20m13");
        private readonly User MONIQUE = new User("as8120d1d0918dh");

        [TestInitialize]
        public void Setup()
        {
            participants = new List<User>();
            participants.Add(BOB);
            participants.Add(MONIQUE);
            TimeServiceMock.Setup(timer => timer.IsCoffeeTime(It.IsAny<TimeZoneInfo>())).Returns(true);
        }

        [TestMethod]
        public void WhenCreatingStateMachine_ThenIsInIdleState()
        {
            BotStateMachine = new BotStateMachine(TimeServiceMock.Object, participants);
            Assert.AreEqual(BotStateMachine.State.Idle, BotStateMachine.GetCurrentState());
        }

        [TestMethod]
        public void GivenValidTime_WhenMachineReceiveCoffeeEmoji_ThenItGoesInCoffeeBuilding()
        {
            BotStateMachine = new BotStateMachine(TimeServiceMock.Object, participants);

            BotStateMachine.CoffeeEmoji(BOB);
            Assert.AreEqual(BotStateMachine.State.CoffeeBreakBuilding, BotStateMachine.GetCurrentState());
        }

        [TestMethod]
        public void GivenInvalidTime_WhenMachineReceiveCoffeeEmoji_ThenItStayIdle()
        {
            TimeServiceMock.Setup(timer => timer.IsCoffeeTime(It.IsAny<TimeZoneInfo>())).Returns(false);
            BotStateMachine = new BotStateMachine(TimeServiceMock.Object, participants);

            BotStateMachine.CoffeeEmoji(BOB);
            Assert.AreEqual(BotStateMachine.State.Idle, BotStateMachine.GetCurrentState());
        }

        [TestMethod]
        public void GivenOneParticipant_WhenMachineReceiveOneReady_ThenItsCoffeeBreak()
        {
            participants = new List<User>();
            participants.Add(BOB);
            BotStateMachine = new BotStateMachine(TimeServiceMock.Object, participants);

            BotStateMachine.CoffeeEmoji(BOB);
            Assert.AreEqual(BotStateMachine.State.CoffeeBreak, BotStateMachine.GetCurrentState());
        }

        [TestMethod]
        public void GivenTwoParticipants_WhenMachineReceiveOneReady_ThenItStayInCoffeeBuilding()
        {
            BotStateMachine = new BotStateMachine(TimeServiceMock.Object, participants);

            BotStateMachine.CoffeeEmoji(BOB);
            Assert.AreEqual(BotStateMachine.State.CoffeeBreakBuilding, BotStateMachine.GetCurrentState());
        }
    }
}
