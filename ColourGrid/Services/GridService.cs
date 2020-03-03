using System;
using System.Collections.Generic;
using System.Linq;
using ColourGrid;
using ColourGridProject.Models;

namespace ColourGridProject.Services
{
    public class GridService
    {
        private readonly Pixel[,] _grid;
        private readonly List<PixelPosition> _pixelsSeen = new List<PixelPosition>();

        public GridService(int gridDimension)
        {
            _grid = new Pixel[gridDimension, gridDimension];
        }

        public GridService(int gridDimensionX, int gridDimensionY)
        {
            _grid = new Pixel[gridDimensionY, gridDimensionX];
        }

        public Pixel[,] GetGridContent()
        {
            return _grid;
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

                _grid[row, currentPixel] = new Pixel
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
                    X = column,
                    Y = currentPixel
                };

                if (!IsPositionValid(currentPosition))
                {
                    throw new ApplicationException("Pixel Position out of bands");
                }

                ;

                _grid[currentPixel, column] = new Pixel
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

            ;

            if (_grid[pixelPosition.Y, pixelPosition.X] != null)
            {
                _grid[pixelPosition.Y, pixelPosition.X].Colour = colour;
            }
            else
            {
                _grid[pixelPosition.Y, pixelPosition.X] = new Pixel
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
            return GetDirectlyTouchingPixels(pixelPosition);
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

        public string GetPixelColour(PixelPosition pixelPosition)
        {
            return _grid[pixelPosition.Y, pixelPosition.X]?.Colour;
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

        private bool IsPositionValid(PixelPosition pixelPosition)
        {
            return pixelPosition.X >= 0 && pixelPosition.X < _grid.GetLength(1) && (pixelPosition.Y >= 0 && pixelPosition.Y < _grid.GetLength(0));
        }

        private static IEnumerable<PixelPosition> GetTouchingPixelPositions(PixelPosition pixelPosition)
        {
            var upPosition = new PixelPosition {X = pixelPosition.X, Y = pixelPosition.Y - 1};
            var leftPosition = new PixelPosition {X = pixelPosition.X - 1, Y = pixelPosition.Y};
            var downPosition = new PixelPosition {X = pixelPosition.X, Y = pixelPosition.Y + 1};
            var rightPosition = new PixelPosition {X = pixelPosition.X + 1, Y = pixelPosition.Y};
            PixelPosition[] touchingPositions = {upPosition, leftPosition, downPosition, rightPosition};
            return touchingPositions;
        }
    }
}