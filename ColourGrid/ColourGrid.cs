using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ColourGrid;

namespace ColourGridProject
{
    public class Grid
    {
        private string[,] grid;
        private int gridDimension;
        private List<PixelPosition> sameColourPixelsToIterate = new List<PixelPosition>();
        private List<PixelPosition> previousRelatedSameColourPixels = new List<PixelPosition>();
        private bool MorePixelsToCheck = false;

        public Grid(int gridDimension)
        {
            this.gridDimension = gridDimension;
            grid = new string[gridDimension, gridDimension];

        }

        public string[,] GetGridContent()
        {
            return grid;
        } 

        public void FillRow( string colour,int row, int startPosition, int endPosition)
        {
            var lowest = startPosition < endPosition ? startPosition : endPosition;
            var highest = startPosition > endPosition ? startPosition : endPosition;

            for (int currentPixel = lowest; currentPixel <= highest; currentPixel++)
            {
                grid[row, currentPixel] = colour;

            }

        }

        public void FillColumn(string colour, int column, int startPosition, int endPosition)
        {
            var lowest = startPosition < endPosition ? startPosition : endPosition;
            var highest = startPosition > endPosition ? startPosition : endPosition;

            for (int currentPixel = lowest; currentPixel <= highest; currentPixel++)
            {
                grid[currentPixel,column] = colour;

            }

        }

        public void FillPixel(string expectedColour, int[] pixelPosition)
        {
            grid[pixelPosition[0], pixelPosition[1]] = expectedColour;
        }

        public IEnumerable<PixelPosition> GetAllAdjacentSameColourPixels(int[] pixelPosition)
        {


            var touchingPixels = GetDirectlyTouchingPixels(pixelPosition);

            while (MorePixelsToCheck)
            {
                foreach (var pixels in touchingPixels)
                {


                }

            }
           



            


            return sameColourPixelsToIterate;


        }

        private IEnumerable<PixelPosition> GetDirectlyTouchingPixels(int[] pixelPosition)
        {
            var pixels = new List<PixelPosition>();
            bool xIsZero = false;
            bool yIsZero = false;

            bool xIsAtEnd = false;
            bool yIsAtEnd = false;


            if (pixelPosition[0] == 0)
            {
                xIsZero = true;
            }

            if (pixelPosition[1] == 0)
            {
                yIsZero = true;
            }

            if (pixelPosition[0] == gridDimension - 1)
            {
                xIsAtEnd = true;
            }

            if (pixelPosition[1] == gridDimension - 1)
            {
                yIsAtEnd = true;
            }


            var currentColour = grid[pixelPosition[0], pixelPosition[1]];

            var upPosition = yIsZero ? null : new PixelPosition { x = pixelPosition[0], y = pixelPosition[1] - 1 };
            var leftPosition = xIsZero ? null : new PixelPosition { x = pixelPosition[0] - 1, y = pixelPosition[1] };
            var downPosition = yIsAtEnd ? null : new PixelPosition { x = pixelPosition[0], y = pixelPosition[1] + 1 };
            var rightPosition = xIsAtEnd ? null : new PixelPosition { x = pixelPosition[0] + 1, y = pixelPosition[1] };

            if (upPosition != null && grid[upPosition.x, upPosition.y] == currentColour)
            {
                pixels.Add(upPosition);
                MorePixelsToCheck = true;
            }
            if (leftPosition != null && grid[leftPosition.x, leftPosition.y] == currentColour)
            {
                pixels.Add(leftPosition);
                MorePixelsToCheck = true;
            }
            if (downPosition != null && grid[downPosition.x, downPosition.y] == currentColour)
            {
                pixels.Add(downPosition);
                MorePixelsToCheck = true;
            }
            if (rightPosition != null && grid[rightPosition.x, rightPosition.y] == currentColour)
            {
                pixels.Add(rightPosition);
                MorePixelsToCheck = true;
            }

            if (pixels.Count == 0)
            {
                MorePixelsToCheck = false;

            }
            return pixels;


        }

        public string GetPixelColour(int[] pixelPosition)
        {
            return grid[pixelPosition[0],pixelPosition[1]];
        }


    }
}