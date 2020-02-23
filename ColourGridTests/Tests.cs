using ColourGrid;
using ColourGridProject;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ColourGridTests
{
    [TestFixture]
    class Tests
    {
        private Grid colourGrid;

        [SetUp]
        public void Setup()
        {
            //colourGrid = new Grid(2);
        }

        [Test]
        public void GivenAColourGridInitalised2x2_WhenGetGridContentCalled_ThenReturnGridArray()
        {
            colourGrid = new Grid(2);
            Assert.That(colourGrid.GetGridContent().GetLength(0), Is.EqualTo(2));
            Assert.That(colourGrid.GetGridContent().GetLength(1), Is.EqualTo(2));

        }
        [Test]
        public void GivenAnFillRowWithAColourAndStartPositionAndEndPosition_ThenPixelsAreColoured()
        {
            colourGrid = new Grid(4);
            var expectedColour = "red";
            colourGrid.FillRow(expectedColour, 0, 1, 2);

            var grid = colourGrid.GetGridContent();
            Assert.That(grid[0,1], Is.EqualTo(expectedColour));
            Assert.That(grid[0,2], Is.EqualTo(expectedColour));

        }
        
        [Test]
        public void GivenAnFillColumnWithAColourAndStartPositionAndEndPosition_ThenPixelsAreColoured()
        {
            colourGrid = new Grid(4);
            var expectedColour = "red";
            colourGrid.FillColumn(expectedColour, 0, 1, 2);

            var grid = colourGrid.GetGridContent();
            Assert.That(grid[1,0], Is.EqualTo(expectedColour));
            Assert.That(grid[2,0], Is.EqualTo(expectedColour));

        }
        
        [Test]
        public void GivenAnFillPixelWithAColourAndStartPositionAndEndPosition_ThenPixelsAreColoured()
        {
            colourGrid = new Grid(4);
            var expectedColour = "red";
            int[] intarray = {0, 1}; 
            colourGrid.FillPixel(expectedColour, intarray);

            var grid = colourGrid.GetGridContent();
            Assert.That(grid[0,1], Is.EqualTo(expectedColour));

        }

        [Test]
        public void GivenABlockOfColouredPixels_AndPixelPositionInBlockGivrn_ThenAdjacentPixelsAreReturned()
        {
            colourGrid = new Grid(4);
            var expectedColour = "red";
            colourGrid.FillRow(expectedColour, 0, 1, 2);
            colourGrid.FillRow(expectedColour, 1, 1, 2);

            var adjecent = colourGrid.GetAllAdjacentSameColourPixels(new int[] { 0, 1 });
            var grid = colourGrid.GetGridContent();
            Assert.That(grid[0, 1], Is.EqualTo(expectedColour));

        }


    }
}
