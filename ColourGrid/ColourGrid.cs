using System;
using System.Collections.Generic;
using System.Linq;
using ColourGrid;
using ColourGridProject.Models;

namespace ColourGridProject
{
    public class Grid
    {
        private Pixel[,] grid;
        private bool _morePixelsToCheck;
        private readonly List<PixelPosition> _pixels = new List<PixelPosition>();

        public Grid(int gridDimension)
        {
            grid = new Pixel[gridDimension, gridDimension];
        }

        public Grid(int gridDimensionX, int gridDimensionY)
        {
            grid = new Pixel[gridDimensionY, gridDimensionX];
        }


        private bool IsPositionValid(PixelPosition pixelPosition)
        {
            if ((pixelPosition.x >= 0 && pixelPosition.x < grid.GetLength(1) && (pixelPosition.y >= 0 && pixelPosition.y < grid.GetLength(0))))
            {
                return true;
            }

            return false;
        }

        public Pixel[,] GetGridContent()
        {
            return this.grid;
        }

        public void FillRow(string colour, int row, int startPosition, int endPosition)
        {
            var (start, end) = GetStartAndEnd(startPosition, endPosition);

            for (int currentPixel = start; currentPixel <= end; currentPixel++)
            {
                var currentPosition = new PixelPosition
                {
                    x = currentPixel,
                    y = row
                };

                if (!IsPositionValid(currentPosition))
                {
                    throw new ApplicationException("Pixel Position out of bands");
                }

                grid[row, currentPixel] = new Pixel
                {
                    Position = currentPosition,
                    Colour = colour
                };
            }
        }

        private static (int start, int end) GetStartAndEnd(int positionA, int positionB)
        {
            var start = positionA < positionB ? positionA : positionB;
            var end = positionA > positionB ? positionA : positionB;
            return (start, end);
        }

        public void FillColumn(string colour, int column, int startPosition, int endPosition)
        {
            var (start, end) = GetStartAndEnd(startPosition, endPosition);


            for (int currentPixel = start; currentPixel <= end; currentPixel++)
            {
                var currentPosition = new PixelPosition
                {
                    x = column,
                    y = currentPixel
                };
                if (!IsPositionValid(currentPosition))
                {
                    throw new ApplicationException("Pixel Position out of bands");
                }

                grid[currentPixel, column] = new Pixel
                {
                    Position = currentPosition,
                    Colour = colour
                };
            }
        }

        public void FillPixel(string colour, PixelPosition pixelPosition)
        {
            if (!IsPositionValid(pixelPosition))
            {
                throw new ApplicationException("Pixel Position out of bands");
            }

            if (grid[pixelPosition.y, pixelPosition.x] != null)
            {
                grid[pixelPosition.y, pixelPosition.x].Colour = colour;
            }
            else
            {
                grid[pixelPosition.y, pixelPosition.x] = new Pixel
                {
                    Position = new PixelPosition
                    {
                        x = pixelPosition.x,
                        y = pixelPosition.y
                    },
                    Colour = colour
                };
            }
        }

        public IEnumerable<PixelPosition> GetAllAdjacentSameColourPixels(PixelPosition pixelPosition)
        {
            var touchingPixels = GetDirectlyTouchingPixels(pixelPosition);

            return touchingPixels;
        }

        private IEnumerable<PixelPosition> GetDirectlyTouchingPixels(PixelPosition pixelPosition)
        {
            var currentColour = GetPixelColour(pixelPosition);

            var upPosition = new PixelPosition {x = pixelPosition.x, y = pixelPosition.y - 1};
            var leftPosition = new PixelPosition {x = pixelPosition.x - 1, y = pixelPosition.y};
            var downPosition = new PixelPosition {x = pixelPosition.x, y = pixelPosition.y + 1};
            var rightPosition = new PixelPosition {x = pixelPosition.x + 1, y = pixelPosition.y};
            PixelPosition[] touchingPositions = {upPosition, leftPosition, downPosition, rightPosition};

            var validTouchingPositions = touchingPositions.Where(IsPositionValid).Where(t => GetPixelColour(t) == currentColour).ToArray();

            var touchingPositionsNotYetSeen = new List<PixelPosition>();
            foreach (var touchingPosition in validTouchingPositions)
            {
                var repeated = _pixels.Any(p => p.x == touchingPosition.x && p.y == touchingPosition.y);
                if (!repeated)
                {
                    touchingPositionsNotYetSeen.Add(touchingPosition);
                }
            }

            _pixels.AddRange(touchingPositionsNotYetSeen);
            _morePixelsToCheck = touchingPositionsNotYetSeen.Any();

            if (_morePixelsToCheck)
            {
                foreach (var validTouchingPosition in touchingPositionsNotYetSeen)
                {
                    var notYetSeenPixelsPositions = GetDirectlyTouchingPixels(validTouchingPosition).Where(p => _pixels.Any(c => c.x != p.x && c.y != p.y));
                    return notYetSeenPixelsPositions;
                }
            }

            return _pixels;
        }

        public string GetPixelColour(PixelPosition pixelPosition)
        {
            return grid[pixelPosition.y, pixelPosition.x]?.Colour;
        }

        public void FloodBlockWithColour(string colour, PixelPosition pixelPosition)
        {
            if (!IsPositionValid(pixelPosition))
            {
                throw new ApplicationException("Pixel Position out of bands");
            }

            var adjacentPixelPositions = GetAllAdjacentSameColourPixels(pixelPosition);
            foreach (var adjacentPixelPosition in adjacentPixelPositions)
            {
                FillPixel(colour, adjacentPixelPosition);
            }
        }
    }
}