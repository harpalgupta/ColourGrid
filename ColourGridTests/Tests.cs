using ColourGrid;
using ColourGridProject;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ColourGridTests
{
    [TestFixture]
    class Tests
    {
        private GridService _colourGridService;

        [SetUp]
        public void Setup()
        {
            //colourGrid = new Grid(2);
        }

        [Test]
        public void GivenAColourGridInitalised2x2_WhenGetGridContentCalled_ThenReturnGridArray()
        {
            _colourGridService = new GridService(2);
            Assert.That(_colourGridService.GetGridContent().GetLength(0), Is.EqualTo(2));
            Assert.That(_colourGridService.GetGridContent().GetLength(1), Is.EqualTo(2));
        }

        [Test]
        public void GivenAGrid4x1AColourAndStartPositionAndEndPosition_WhenFillRowCalled_ThenPixelsAreColoured()
        {
            _colourGridService = new GridService(4,1);
            var expectedColour = "red";
            _colourGridService.FillRow(expectedColour, 0, 1, 2);

            var grid = _colourGridService.GetGridContent();
            Assert.That(grid[0, 1].Colour, Is.EqualTo(expectedColour));
            Assert.That(grid[0, 2].Colour, Is.EqualTo(expectedColour));
        }

        [Test]
        public void GivenAnFillColumnWithAColourAndStartPositionAndEndPosition_ThenPixelsAreColoured()
        {
            _colourGridService = new GridService(4);
            var expectedColour = "red";
            _colourGridService.FillColumn(expectedColour, 0, 1, 2);

            var grid = _colourGridService.GetGridContent();

            Assert.That(grid[1, 0].Colour, Is.EqualTo(expectedColour));
            Assert.That(grid[2, 0].Colour, Is.EqualTo(expectedColour));
        }

        [Test]
        public void GivenAPixelPositionAndColour_WhenFillPixelIsCalled_PixelShouldBeExpectedColour()
        {
            _colourGridService = new GridService(4);
            var expectedColour = "red";
            var pixelPosition = new PixelPosition {X = 1, Y = 2};

            _colourGridService.FillPixel(expectedColour, pixelPosition);
            Assert.That(_colourGridService.GetPixelColour(pixelPosition), Is.EqualTo(expectedColour));
        }


        [Test]
        public void GivenABlockOfColouredPixels_AndPixelPositionInBlockGivrn_ThenAdjacentPixelsAreReturned()
        {
            _colourGridService = new GridService(4);
            var expectedColour = "red";
            _colourGridService.FillRow(expectedColour, 0, 1, 2);
            _colourGridService.FillRow(expectedColour, 1, 1, 2);

            var adjecent = _colourGridService.GetAllAdjacentSameColourPixels(new PixelPosition {X = 1, Y = 0});
            var grid = _colourGridService.GetGridContent();
            var resultPixel = grid[0, 1];
            Assert.That(resultPixel.Colour, Is.EqualTo(expectedColour));
            var pixelPositions = adjecent as PixelPosition[] ?? adjecent.ToArray();
            Assert.That(pixelPositions.Count(), Is.EqualTo(4));
            Assert.That(pixelPositions.Any(p => p.X == 1 && p.Y == 0));
            Assert.That(pixelPositions.Any(p => p.X == 2 && p.Y == 0));
            Assert.That(pixelPositions.Any(p => p.X == 1 && p.Y == 1));
            Assert.That(pixelPositions.Any(p => p.X == 2 && p.Y == 1));
        }

        [Test]
        public void GivenABlockOfColouredPixels_AndPixelPositionInBlockGiven__WhenFloodCalledWithPixelPos_ThenAdjacentPixelsAreReturned()
        {
            _colourGridService = new GridService(4);
            var expectedColour = "red";
            var expectedNewFloodColour = "white";
            _colourGridService.FillRow(expectedColour, 0, 1, 2);
            _colourGridService.FillRow(expectedColour, 1, 1, 2);

            _colourGridService.FloodBlockWithColour(expectedNewFloodColour, new PixelPosition {X = 1, Y = 0});

            Assert.That(_colourGridService.GetPixelColour(new PixelPosition {X = 2, Y = 0}), Is.EqualTo(expectedNewFloodColour));
            Assert.That(_colourGridService.GetPixelColour(new PixelPosition {X = 2, Y = 0}), Is.EqualTo(expectedNewFloodColour));
            Assert.That(_colourGridService.GetPixelColour(new PixelPosition {X = 1, Y = 1}), Is.EqualTo(expectedNewFloodColour));
            ;
            Assert.That(_colourGridService.GetPixelColour(new PixelPosition {X = 2, Y = 1}), Is.EqualTo(expectedNewFloodColour));
        }

        [Test]
        public void GivenABlockOfColouredPixels_AndPixelPositionIsInValid__WhenFloodCalledWithPixelPos_ThenExceptionThrown()
        {
            _colourGridService = new GridService(4);
            var expectedColour = "red";
            var expectedNewFloodColour = "white";
            _colourGridService.FillRow(expectedColour, 0, 1, 2);
            _colourGridService.FillRow(expectedColour, 1, 1, 2);

            Assert.Throws<ApplicationException>(() => _colourGridService.FloodBlockWithColour(expectedNewFloodColour, new PixelPosition {X = 9, Y = 9}));
        }

        [Test]
        public void GivenAnInvalidPixelPosition_WhenFillPixelIsCalled_ThenExceptionThrown()
        {
            _colourGridService = new GridService(4);

            Assert.Throws<ApplicationException>(() => _colourGridService.FillPixel("red", new PixelPosition {X = 9, Y = 9}));
        }

        [Test]
        public void GivenAPixelPositionWithAnOutOfBandEnd_WhenFillRowIsCalled_ThenExceptionThrown()
        {
            _colourGridService = new GridService(4);

            Assert.Throws<ApplicationException>(() => _colourGridService.FillRow("red", 0, 0, 9));
        }
        
        [Test]
        public void GivenAPixelPositionWithAnOutOfBandEnd_WhenFillColumnIsCalled_ThenExceptionThrown()
        {
            _colourGridService = new GridService(4);

            Assert.Throws<ApplicationException>(() => _colourGridService.FillColumn("red", 0, 0, 9));
        }
    }
}