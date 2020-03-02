using System;
using System.Collections.Generic;
using System.Linq;
using ColourGrid;
using ColourGridProject.Models;

namespace ColourGridProject
{
    public class GridService
    {
        private Pixel[,] grid;
        private bool _morePixelsToCheck;
        private readonly List<PixelPosition> _pixelsSeen = new List<PixelPosition>();

        public GridService(int gridDimension)
        {
            grid = new Pixel[gridDimension, gridDimension];
        }

        public GridService(int gridDimensionX, int gridDimensionY)
        {
            grid = new Pixel[gridDimensionY, gridDimensionX];
        }


        private bool IsPositionValid(PixelPosition pixelPosition)
        {
            return pixelPosition.X >= 0 && pixelPosition.X < grid.GetLength(1) && (pixelPosition.Y >= 0 && pixelPosition.Y < grid.GetLength(0));
        }

        public Pixel[,] GetGridContent()
        {
            return grid;
        }

        public void FillRow(string colour, int row, int startPosition, int endPosition)
        {
            var (start, end) = GetStartAndEnd(startPosition, endPosition);

            for (int currentPixel = start; currentPixel <= end; currentPixel++)
            {
                var currentPosition = new PixelPosition
                {
                    X = currentPixel,
                    Y = row
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
                    X= column,
                    Y = currentPixel
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

            if (grid[pixelPosition.Y, pixelPosition.X] != null)
            {
                grid[pixelPosition.Y, pixelPosition.X].Colour = colour;
            }
            else
            {
                grid[pixelPosition.Y, pixelPosition.X] = new Pixel
                {
                    Position = new PixelPosition
                    {
                        X = pixelPosition.X,
                        Y = pixelPosition.Y
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

            var touchingPositions = GetTouchingPixelPositions(pixelPosition);

            var validTouchingPositions = touchingPositions.Where(IsPositionValid).Where(t => GetPixelColour(t) == currentColour).ToArray();
           
            var touchingPositionsNotYetSeen = validTouchingPositions.Where(v => !_pixelsSeen.Any(p => p.X == v.X && p.Y == v.Y)).ToList();
            
            _pixelsSeen.AddRange(touchingPositionsNotYetSeen);

            if (!touchingPositionsNotYetSeen.Any()) return _pixelsSeen;
            {
                foreach (var validTouchingPosition in touchingPositionsNotYetSeen)
                {
                    var notYetSeenPixelsPositions = GetDirectlyTouchingPixels(validTouchingPosition).Where(p => _pixelsSeen.Any(c => c.X != p.X && c.Y != p.Y));
                    return notYetSeenPixelsPositions;
                }
            }

            return _pixelsSeen;
        }

        private static PixelPosition[] GetTouchingPixelPositions(PixelPosition pixelPosition)
        {
            var upPosition = new PixelPosition {X = pixelPosition.X, Y = pixelPosition.Y - 1};
            var leftPosition = new PixelPosition {X = pixelPosition.X - 1, Y = pixelPosition.Y};
            var downPosition = new PixelPosition {X = pixelPosition.X, Y = pixelPosition.Y + 1};
            var rightPosition = new PixelPosition {X = pixelPosition.X + 1, Y = pixelPosition.Y};
            PixelPosition[] touchingPositions = {upPosition, leftPosition, downPosition, rightPosition};
            return touchingPositions;
        }

        public string GetPixelColour(PixelPosition pixelPosition)
        {
            return grid[pixelPosition.Y, pixelPosition.X]?.Colour;
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