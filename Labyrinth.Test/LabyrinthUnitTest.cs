using Labyrinth.Model;
using Labyrinth.Persistence;
using Microsoft.VisualStudio.TestPlatform.Common.Utilities;
using Moq;
using System;
using System.Data;
using System.Runtime.CompilerServices;
namespace Labyrinth.Test
{
    [TestClass]
    public class LabyrinthUnitTest
    {
        private Mock<IFileReader> _mock = null!;
        private LabyrinthModel _model = null!;
        [TestInitialize]
        public void Initialize()
        {
            _mock = new Mock<IFileReader>();
            string[] mockContent = new string[] { "0 0 0 1 2", "1 1 0 1 0", "0 0 0 0 0", "0 1 1 0 1", "3 0 0 1 1" };
            _mock.Setup(fr => fr.ReadFile(5)).Returns(mockContent);
            _model = new LabyrinthModel(5);
        }
        [TestMethod]
        public void LabyrinthConstructorTest()
        {
            _model.NewGame();
            Assert.AreEqual(5, _model.Size);
            Assert.AreEqual(0, _model.Steps);
            Assert.AreEqual(_model.Size * _model.Size, _model.GameTable.LongLength);
        }
        [TestMethod]
        public void LabyrinthStepTest()
        {
            _model.NewGame();
            Assert.AreEqual(_model.GameTable[_model.Size - 1, 0], Field.Character);
            Assert.AreEqual(_model.GameTable[0, _model.Size - 1], Field.Trophy);
            if (_model.GameTable[_model.Size - 1, 1] == Field.Grass)
            {
                _model.Step(_model.Size - 1, 1);
                Assert.AreEqual(_model.Size - 1, _model.X);
                Assert.AreEqual(1, _model.Y);
            }
            else if (_model.GameTable[_model.Size - 2, 0] == Field.Grass)
            {
                _model.Step(_model.Size - 2, 0);
                Assert.AreEqual(_model.Size - 2, _model.X);
                Assert.AreEqual(0, _model.Y);
            }
            _model.NewGame();
            int i = 1;
            while (_model.GameTable[_model.Size - 1, i] != Field.Wall)
            {
                _model.Step(_model.Size - 1, i);
                i++;
            }
            try
            {
                _model.Step(_model.Size - 1, i);
                Assert.Fail();
            }
            catch (ArgumentException) { }
            _model.NewGame();
            try
            {
                _model.Step(_model.Size - 1, 2);
                Assert.Fail();
            }
            catch (ArgumentException) { }
            _model.NewGame();
            try
            {
                _model.Step(_model.Size - 1, 0);
                Assert.Fail();
            }
            catch (ArgumentException) { }
            _model.NewGame();
            try
            {
                _model.Step(_model.Size, 0);
                Assert.Fail();
                _model.Step(_model.Size - 1, -1);
                Assert.Fail();
            }
            catch (IndexOutOfRangeException) { }
        }
        [TestMethod]
        public void LabyrinthGameOverTest()
        {
            bool eventRaised = false;
            _model.GameWon += delegate (object? sender, GameWonEventArgs e)
            {
                eventRaised = true;
                Assert.IsTrue(e.Steps == _model.Steps);
            };
            _model.NewGame();
            _model.Step(3, 0);
            _model.Step(2, 0);
            _model.Step(2, 1);
            _model.Step(2, 2);
            _model.Step(2, 3);
            _model.Step(2, 4);
            _model.Step(1, 4);
            _model.Step(0, 4);
            Assert.IsTrue(eventRaised);
        }
        [TestMethod]
        public void LabyrinthFieldChangeTest()
        {
            bool eventRaised = false;
            _model.FieldChange += delegate (object? sender, FieldChangeEventArgs e)
            {
                eventRaised = true;
                Assert.IsTrue(e.Field == _model.GameTable[_model.X,_model.Y]);
                Assert.IsTrue(e.X == _model.X);
                Assert.IsTrue(e.Y == _model.Y);
            };
            _model.NewGame();
            _model.Step(3, 0);
            Assert.IsTrue(eventRaised);
        }
    }
}