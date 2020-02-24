﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using ColourGrid;
using ColourGridProject.Models;

namespace ColourGridProject
{
    public class Grid
    {
        private Pixel[,] grid;
        private int gridDimension;
        private bool MorePixelsToCheck = false;
        private List<PixelPosition> pixels = new List<PixelPosition>();

        public Grid(int gridDimension)
        {
            this.gridDimension = gridDimension;
            this.grid = new Pixel[gridDimension, gridDimension];
        }


        private bool IsPositionValid(PixelPosition pixelPosition)
        {
            if ((pixelPosition.x >= 0 && pixelPosition.x < gridDimension) && (pixelPosition.y >= 0 && pixelPosition.y < gridDimension))
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
            var start = startPosition < endPosition ? startPosition : endPosition;
            var end = startPosition > endPosition ? startPosition : endPosition;

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
                };
                grid[row, currentPixel] = new Pixel
                {
                    position = currentPosition,
                    Colour = colour
                };
            }
        }

        public void FillColumn(string colour, int column, int startPosition, int endPosition)
        {
            var lowest = startPosition < endPosition ? startPosition : endPosition;
            var highest = startPosition > endPosition ? startPosition : endPosition;

            for (int currentPixel = lowest; currentPixel <= highest; currentPixel++)
            {
                var currentPosition = new PixelPosition
                {
                    x = column,
                    y = currentPixel
                };
                if (!IsPositionValid(currentPosition))
                {
                    throw new ApplicationException("Pixel Position out of bands");
                };
                
                grid[currentPixel, column] = new Pixel
                {
                    position = currentPosition,
                    Colour = colour
                };
            }
        }

        public void FillPixel(string colour, PixelPosition pixelPosition)
        {
            if (!IsPositionValid(pixelPosition))
            {
                throw new ApplicationException("Pixel Position out of bands");
            };
            if (grid[pixelPosition.x, pixelPosition.y] != null)
            {
                grid[pixelPosition.x, pixelPosition.y].Colour = colour;
            }
            else
            {
                grid[pixelPosition.x, pixelPosition.y] = new Pixel
                {
                    position = new PixelPosition
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

            var validtouchingPositions = touchingPositions.Where(t => IsPositionValid(t)).Where(t => GetPixelColour(t)==currentColour).ToArray();
            
            var touchingPositionsNotYetSeen= new List<PixelPosition>();
            foreach (var touchingPosition in validtouchingPositions)
            {
                var repeated = pixels.Any(p => p.x == touchingPosition.x && p.y == touchingPosition.y);
                if (!repeated)
                {
                    touchingPositionsNotYetSeen.Add(touchingPosition);
                }
            }

            pixels.AddRange(touchingPositionsNotYetSeen);
            MorePixelsToCheck = touchingPositionsNotYetSeen.Any();

            while (MorePixelsToCheck)
            {


                foreach (var validtouchingPosition in touchingPositionsNotYetSeen)
                {
                    pixels.AddRange(GetDirectlyTouchingPixels(validtouchingPosition));

                }
            }

//            if (upPosition != null && grid[upPosition.x, upPosition.y] == currentColour)
//            {
//                pixels.Add(upPosition);
//                MorePixelsToCheck = true;
//            }
//
//            if (leftPosition != null && grid[leftPosition.x, leftPosition.y] == currentColour)
//            {
//                pixels.Add(leftPosition);
//                MorePixelsToCheck = true;
//            }
//
//            if (downPosition != null && grid[downPosition.x, downPosition.y] == currentColour)
//            {
//                pixels.Add(downPosition);
//                MorePixelsToCheck = true;
//            }
//
//            if (rightPosition != null && grid[rightPosition.x, rightPosition.y] == currentColour)
//            {
//                pixels.Add(rightPosition);
//                MorePixelsToCheck = true;
//            }
//
//            if (pixels.Count == 0)
//            {
//                MorePixelsToCheck = false;
//            }

            return pixels;
        }

        public string GetPixelColour(PixelPosition pixelPosition)
        {
            return grid[pixelPosition.y, pixelPosition.x]?.Colour;
        }
    }
}