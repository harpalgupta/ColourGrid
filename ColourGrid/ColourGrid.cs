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

    }
}