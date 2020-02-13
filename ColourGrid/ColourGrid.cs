namespace ColourGridProject
{
    public class Grid
    {
        private string[,] grid;

        public Grid(int gridDimension)
        {
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

        public void GetAllAdjacentSameColourPixels(int[] pixelPosition)
        {
            var currentColour = grid[pixelPosition[0], pixelPosition[1]];
            var upColour = grid[pixelPosition[0], pixelPosition[1] - 1];
            var downColour = grid[pixelPosition[0], pixelPosition[1] +1];
            var leftColour = grid[pixelPosition[0]-1, pixelPosition[1] ];
            var rightColour = grid[pixelPosition[0]+11, pixelPosition[1] ];

        }

        public string GetPixelColour(int[] pixelPosition)
        {
            return grid[pixelPosition[0],pixelPosition[1]];
        }
    }
}