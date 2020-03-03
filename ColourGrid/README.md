# Painting Pixels on a grid

## Summary

This application allows you to create a grid of pixels with a set of dimensions
- Method FillPixel allows you to Fill a pixel with a colour
- Method FillRow allows filling a Row given a start and end
- Method FillColumn allows filling a Column given a start and end
- Method FloodBlockWithColour allows Flood to re-colour all those adjacent same coloured pixels. 
    If there is a block of pixels that are yellow then you can change this block of colours to be white
- Method GetPixelColour allows you retrieve colour of pixel at given position

## Unit Tests
The unit tests have been written using Nunit 
You should be able to see example usage of methods within the unit tests

## Other Considerations
- Use of Dictionaries could improve performance
