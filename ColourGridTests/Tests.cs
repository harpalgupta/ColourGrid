using ColourGrid;
using ColourGridProject;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ColourGridProject.Services;

namespace ColourGridTests
{
    [TestFixture]
    class Tests
    {
        private GridService _colourGridService;
        private int _startIndexPosition = 1;

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void GivenAColourGridInitalised2x2_WhenGetGridContentCalled_ThenReturnGridArray()
        {
            
            _colourGridService = new GridService(2,_startIndexPosition);
            Assert.That(_colourGridService.GetGridContent().GetLength(0), Is.EqualTo(2+_startIndexPosition));
            Assert.That(_colourGridService.GetGridContent().GetLength(1), Is.EqualTo(2+_startIndexPosition));
        }

        [Test]
        public void GivenAGrid4x1AColourAndStartPositionAndEndPosition_WhenFillRowCalled_ThenPixelsAreColoured()
        {
            _colourGridService = new GridService(4,1,_startIndexPosition);
            var expectedColour = "red";
            _colourGridService.FillRow(expectedColour, 1, 1, 2);

            var grid = _colourGridService.GetGridContent();
            Assert.That(grid[1, 1].Colour, Is.EqualTo(expectedColour));
            Assert.That(grid[1, 2].Colour, Is.EqualTo(expectedColour));
        }

        [Test]
        public void GivenAnFillColumnWithAColourAndStartPositionAndEndPosition_ThenPixelsAreColoured()
        {
            _colourGridService = new GridService(4,_startIndexPosition);
            var expectedColour = "red";
            _colourGridService.FillColumn(expectedColour, 1, 1, 2);

            var grid = _colourGridService.GetGridContent();

            Assert.That(grid[1, 1].Colour, Is.EqualTo(expectedColour));
            Assert.That(grid[2, 1].Colour, Is.EqualTo(expectedColour));
        }

        [Test]
        public void GivenAPixelPositionAndColour_WhenFillPixelIsCalled_PixelShouldBeExpectedColour()
        {
            _colourGridService = new GridService(4,_startIndexPosition);
            var expectedColour = "red";
            var pixelPosition = new PixelPosition {X = 1, Y = 2};

            _colourGridService.FillPixel(expectedColour, pixelPosition);
            Assert.That(_colourGridService.GetPixelColour(pixelPosition), Is.EqualTo(expectedColour));
        }


        [Test]
        public void GivenABlockOfColouredPixels_AndPixelPositionInBlockGivrn_ThenAdjacentPixelsAreReturned()
        {
            _colourGridService = new GridService(4,_startIndexPosition);
            var expectedColour = "red";
            _colourGridService.FillRow(expectedColour, 1, 1, 2);
            _colourGridService.FillRow(expectedColour, 2, 1, 2);

            var adjecent = _colourGridService.GetAllAdjacentSameColourPixels(new PixelPosition {X = 1, Y = 1});
            var grid = _colourGridService.GetGridContent();
            var resultPixel = grid[1, 1];
            Assert.That(resultPixel.Colour, Is.EqualTo(expectedColour));
            var pixelPositions = adjecent as PixelPosition[] ?? adjecent.ToArray();
            Assert.That(pixelPositions.Count(), Is.EqualTo(4));
            Assert.That(pixelPositions.Any(p => p.X == 1 && p.Y == 1));
            Assert.That(pixelPositions.Any(p => p.X == 2 && p.Y == 1));
            Assert.That(pixelPositions.Any(p => p.X == 1 && p.Y == 2));
            Assert.That(pixelPositions.Any(p => p.X == 2 && p.Y == 2));
        }

        [Test]
        public void GivenABlockOfColouredPixels_AndPixelPositionInBlockGiven__WhenFloodCalledWithPixelPos_ThenAdjacentPixelsAreReturned()
        {
            _colourGridService = new GridService(4,_startIndexPosition);
            var expectedColour = "red";
            var expectedNewFloodColour = "white";
            var start = 1;
            var end = 2;
            _colourGridService.FillRow(expectedColour, 1, start, end);
            _colourGridService.FillRow(expectedColour, 2, start, end);

            _colourGridService.FloodBlockWithColour(expectedNewFloodColour, new PixelPosition {X = 1, Y = 1});

            Assert.That(_colourGridService.GetPixelColour(new PixelPosition {X = 2, Y = 1}), Is.EqualTo(expectedNewFloodColour));
            Assert.That(_colourGridService.GetPixelColour(new PixelPosition {X = 1, Y = 1}), Is.EqualTo(expectedNewFloodColour));
            Assert.That(_colourGridService.GetPixelColour(new PixelPosition {X = 1, Y = 2}), Is.EqualTo(expectedNewFloodColour));
            Assert.That(_colourGridService.GetPixelColour(new PixelPosition {X = 2, Y = 2}), Is.EqualTo(expectedNewFloodColour));
        }

        [Test]
        public void GivenABlockOfColouredPixels_AndPixelPositionIsInValid__WhenFloodCalledWithPixelPos_ThenExceptionThrown()
        {
            _colourGridService = new GridService(4,_startIndexPosition);
            var expectedColour = "red";
            var expectedNewFloodColour = "white";
            _colourGridService.FillRow(expectedColour, 1, 1, 2);
            _colourGridService.FillRow(expectedColour, 2, 1, 2);

            Assert.Throws<ApplicationException>(() => _colourGridService.FloodBlockWithColour(expectedNewFloodColour, new PixelPosition {X = 9, Y = 9}));
        }

        [Test]
        public void GivenAnInvalidPixelPosition_WhenFillPixelIsCalled_ThenExceptionThrown()
        {
            _colourGridService = new GridService(4,_startIndexPosition);

            Assert.Throws<ApplicationException>(() => _colourGridService.FillPixel("red", new PixelPosition {X = 9, Y = 9}));
        }

        [Test]
        public void GivenAPixelPositionWithAnOutOfBandEnd_WhenFillRowIsCalled_ThenExceptionThrown()
        {
            _colourGridService = new GridService(4,_startIndexPosition);

            Assert.Throws<ApplicationException>(() => _colourGridService.FillRow("red", 0, 0, 9));
        }
        
        [Test]
        public void GivenAPixelPositionWithAnOutOfBandEnd_WhenFillColumnIsCalled_ThenExceptionThrown()
        {
            _colourGridService = new GridService(4,_startIndexPosition);

            Assert.Throws<ApplicationException>(() => _colourGridService.FillColumn("red", 0, 0, 9));
        }
    }
}