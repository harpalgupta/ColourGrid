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
        private Grid _colourGrid;

        [SetUp]
        public void Setup()
        {
            //colourGrid = new Grid(2);
        }

        [Test]
        public void GivenAColourGridInitalised2x2_WhenGetGridContentCalled_ThenReturnGridArray()
        {
            _colourGrid = new Grid(2);
            Assert.That(_colourGrid.GetGridContent().GetLength(0), Is.EqualTo(2));
            Assert.That(_colourGrid.GetGridContent().GetLength(1), Is.EqualTo(2));
        }

        [Test]
        public void GivenAGrid4x1AColourAndStartPositionAndEndPosition_WhenFillRowCalled_ThenPixelsAreColoured()
        {
            _colourGrid = new Grid(4,1);
            var expectedColour = "red";
            _colourGrid.FillRow(expectedColour, 0, 1, 2);

            var grid = _colourGrid.GetGridContent();
            Assert.That(grid[0, 1].Colour, Is.EqualTo(expectedColour));
            Assert.That(grid[0, 2].Colour, Is.EqualTo(expectedColour));
        }

        [Test]
        public void GivenAnFillColumnWithAColourAndStartPositionAndEndPosition_ThenPixelsAreColoured()
        {
            _colourGrid = new Grid(4);
            var expectedColour = "red";
            _colourGrid.FillColumn(expectedColour, 0, 1, 2);

            var grid = _colourGrid.GetGridContent();

            Assert.That(grid[1, 0].Colour, Is.EqualTo(expectedColour));
            Assert.That(grid[2, 0].Colour, Is.EqualTo(expectedColour));
        }

        [Test]
        public void GivenAPixelPositionAndColour_WhenFillPixelIsCalled_PixelShouldBeExpectedColour()
        {
            _colourGrid = new Grid(4);
            var expectedColour = "red";
            var pixelPosition = new PixelPosition {x = 1, y = 2};

            _colourGrid.FillPixel(expectedColour, pixelPosition);
            Assert.That(_colourGrid.GetPixelColour(pixelPosition), Is.EqualTo(expectedColour));
        }


        [Test]
        public void GivenABlockOfColouredPixels_AndPixelPositionInBlockGivrn_ThenAdjacentPixelsAreReturned()
        {
            _colourGrid = new Grid(4);
            var expectedColour = "red";
            _colourGrid.FillRow(expectedColour, 0, 1, 2);
            _colourGrid.FillRow(expectedColour, 1, 1, 2);

            var adjecent = _colourGrid.GetAllAdjacentSameColourPixels(new PixelPosition {x = 1, y = 0});
            var grid = _colourGrid.GetGridContent();
            var resultPixel = grid[0, 1];
            Assert.That(resultPixel.Colour, Is.EqualTo(expectedColour));
            var pixelPositions = adjecent as PixelPosition[] ?? adjecent.ToArray();
            Assert.That(pixelPositions.Count(), Is.EqualTo(4));
            Assert.That(pixelPositions.Any(p => p.x == 1 && p.y == 0));
            Assert.That(pixelPositions.Any(p => p.x == 2 && p.y == 0));
            Assert.That(pixelPositions.Any(p => p.x == 1 && p.y == 1));
            Assert.That(pixelPositions.Any(p => p.x == 2 && p.y == 1));
        }

        [Test]
        public void GivenABlockOfColouredPixels_AndPixelPositionInBlockGiven__WhenFloodCalledWithPixelPos_ThenAdjacentPixelsAreReturned()
        {
            _colourGrid = new Grid(4);
            var expectedColour = "red";
            var expectedNewFloodColour = "white";
            _colourGrid.FillRow(expectedColour, 0, 1, 2);
            _colourGrid.FillRow(expectedColour, 1, 1, 2);

            _colourGrid.FloodBlockWithColour(expectedNewFloodColour, new PixelPosition {x = 1, y = 0});

            Assert.That(_colourGrid.GetPixelColour(new PixelPosition {x = 2, y = 0}), Is.EqualTo(expectedNewFloodColour));
            Assert.That(_colourGrid.GetPixelColour(new PixelPosition {x = 2, y = 0}), Is.EqualTo(expectedNewFloodColour));
            Assert.That(_colourGrid.GetPixelColour(new PixelPosition {x = 1, y = 1}), Is.EqualTo(expectedNewFloodColour));
            ;
            Assert.That(_colourGrid.GetPixelColour(new PixelPosition {x = 2, y = 1}), Is.EqualTo(expectedNewFloodColour));
        }

        [Test]
        public void GivenABlockOfColouredPixels_AndPixelPositionIsInValid__WhenFloodCalledWithPixelPos_ThenExceptionThrown()
        {
            _colourGrid = new Grid(4);
            var expectedColour = "red";
            var expectedNewFloodColour = "white";
            _colourGrid.FillRow(expectedColour, 0, 1, 2);
            _colourGrid.FillRow(expectedColour, 1, 1, 2);

            Assert.Throws<ApplicationException>(() => _colourGrid.FloodBlockWithColour(expectedNewFloodColour, new PixelPosition {x = 9, y = 9}));
        }

        [Test]
        public void GivenAnInvalidPixelPosition_WhenFillPixelIsCalled_ThenExceptionThrown()
        {
            _colourGrid = new Grid(4);
            var expectedColour = "red";

            Assert.Throws<ApplicationException>(() => _colourGrid.FillPixel("red", new PixelPosition {x = 9, y = 9}));
        }

        [Test]
        public void GivenAPixelPositionWithAnOutOfBandEnd_WhenFillRowIsCalled_ThenExceptionThrown()
        {
            _colourGrid = new Grid(4);
            var expectedColour = "red";

            Assert.Throws<ApplicationException>(() => _colourGrid.FillRow("red", 0, 0, 9));
        }
        
        [Test]
        public void GivenAPixelPositionWithAnOutOfBandEnd_WhenFillColumnIsCalled_ThenExceptionThrown()
        {
            _colourGrid = new Grid(4);
            var expectedColour = "red";

            Assert.Throws<ApplicationException>(() => _colourGrid.FillColumn("red", 0, 0, 9));
        }
    }
}