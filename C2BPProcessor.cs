using System.Collections.Generic;
using System.Drawing;
using System.Collections;

namespace iSpyApplication
{
    class C2BpProcessor
    {
        // First position rectangles more than half as wide as the bin.
        // Then position the remaining rectangles in two columns.
        public void AlgOneColumn(int binWidth, Rectangle[] rects)
        {
            // Find the wide rectangles.
            ArrayList wideCol = new ArrayList();
            ArrayList narrowCol = new ArrayList();
            for (int i = 0; i <= rects.Length - 1; i++)
            {
                if (rects[i].Width > binWidth / 2)
                    wideCol.Add(rects[i]);
                else
                    narrowCol.Add(rects[i]);
            }

            // Sort the wide rectangles by width.
            Rectangle[] wideArray = new Rectangle[wideCol.Count];
            for (int i = 0; i <= wideCol.Count - 1; i++)
            {
                wideArray[i] = (Rectangle)wideCol[i];
            }
            System.Array.Sort(wideArray, new WidthComparer());

            // Sort the narrow rectangles by height.
            Rectangle[] narrowArray = new Rectangle[narrowCol.Count];
            for (int i = 0; i <= narrowCol.Count - 1; i++)
            {
                narrowArray[i] = (Rectangle)narrowCol[i];
            }
            System.Array.Sort(narrowArray, new HeightComparer());

            // Arrange the wide rectangles.
            int x = 0;
            int y = 0;
            for (int i = 0; i <= wideArray.Length - 1; i++)
            {
                wideArray[i].X = x;
                wideArray[i].Y = y;
                y += wideArray[i].Height;
            }

            // Arrange the narrow rectangles.
            if (narrowArray.Length > 0)
            {
                int rowHeight = narrowArray[0].Height;
                for (int i = 0; i <= narrowArray.Length - 1; i++)
                {
                    if (x + narrowArray[i].Width <= binWidth)
                    {
                        // Add to this row.
                        narrowArray[i].X = x;
                        narrowArray[i].Y = y;
                    }
                    else
                    {
                        // Start a new row.
                        x = 0;
                        y += rowHeight;
                        rowHeight = narrowArray[i].Height;

                        narrowArray[i].X = x;
                        narrowArray[i].Y = y;
                    }
                    x += narrowArray[i].Width;
                }
            }

            // Combine the results into the rect array.
            System.Array.Copy(wideArray, rects, wideArray.Length);
            System.Array.Copy(narrowArray, 0, rects, wideArray.Length, narrowArray.Length);
        }

        // Sort rectangles by height.
        // Fill in by rows in a single column.
        public void AlgSortByHeight(int binWidth, Rectangle[] rects)
        {
            // Sort by height.
            System.Array.Sort(rects, new HeightComparer());

            // Fill in one column.
            SubAlgFillOneColumn(binWidth, rects);
        }

        // Sort rectangles by width.
        // Fill in by rows in a single column.
        public void AlgSortByWidth(int binWidth, Rectangle[] rects)
        {
            // Sort by height.
            System.Array.Sort(rects, new WidthComparer());

            // Fill in one column.
            SubAlgFillOneColumn(binWidth, rects);
        }

        // Sort rectangles by area.
        // Fill in by rows in a single column.
        public void AlgSortByArea(int binWidth, Rectangle[] rects)
        {
            // Sort by height.
            System.Array.Sort(rects, new AreaComparer());

            // Fill in one column.
            SubAlgFillOneColumn(binWidth, rects);
        }

        // Sort rectangles by squareness.
        // Fill in by rows in a single column.
        public void AlgSortBySquareness(int binWidth, Rectangle[] rects)
        {
            // Sort by height.
            System.Array.Sort(rects, new SquarenessComparer());

            // Fill in one column.
            SubAlgFillOneColumn(binWidth, rects);
        }

        // Fill in by rows in a single column.
        public void SubAlgFillOneColumn(int binWidth, Rectangle[] rects)
        {
            // Make lists of positioned and not positioned rectangles.
            List<Rectangle> notPositioned = new List<Rectangle>();
            List<Rectangle> positioned = new List<Rectangle>();
            for (int i = 0; i <= rects.Length - 1; i++)
                notPositioned.Add(rects[i]);

            // Arrange the rectangles.
            int x = 0;
            int y = 0;
            int rowHgt = 0;
            while (notPositioned.Count > 0)
            {
                // Find the next rectangle that will fit on this row.
                int nextRect = -1;
                for (int i = 0; i <= notPositioned.Count - 1; i++)
                {
                    if (x + notPositioned[i].Width <= binWidth)
                    {
                        nextRect = i;
                        break;
                    }
                }

                // If we didn't find a rectangle that fits, start a new row.
                if (nextRect < 0)
                {
                    y += rowHgt;
                    x = 0;
                    rowHgt = 0;
                    nextRect = 0;
                }

                // Position the selected rectangle.
                Rectangle rect = notPositioned[nextRect];
                rect.X = x;
                rect.Y = y;
                x += rect.Width;
                if (rowHgt < rect.Height) rowHgt = rect.Height;

                // Move the rectangle into the positioned list.
                positioned.Add(rect);
                notPositioned.RemoveAt(nextRect);
            }

            // Prepare the results.
            for (int i = 0; i <= positioned.Count - 1; i++)
                rects[i] = positioned[i];
        }

        // Start the recursion.
        public void AlgFillByStripes(int binWidth, Rectangle[] rects)
        {
            // Sort by height.
            Rectangle[] bestRects = (Rectangle[])rects.Clone();
            System.Array.Sort(bestRects, new HeightComparer());

            // Make variables to track and record the best solution.
            bool[] isPositioned = new bool[bestRects.Length];
            int numUnpositioned = bestRects.Length;

            // Fill by stripes.
            int maxY = 0;
            for (int i = 0; i <= rects.Length - 1; i++)
            {
                // See if this rectangle is positioned.
                if (!isPositioned[i])
                {
                    // Start a new stripe.
                    numUnpositioned -= 1;
                    isPositioned[i] = true;
                    bestRects[i].X = 0;
                    bestRects[i].Y = maxY;

                    FillBoundedArea(
                        bestRects[i].Width, binWidth, maxY,
                        maxY + bestRects[i].Height,
                        ref numUnpositioned, ref bestRects, ref isPositioned);

                    if (numUnpositioned == 0) break;
                    maxY += bestRects[i].Height;
                }
            }

            // Save the best solution.
            System.Array.Copy(bestRects, rects, rects.Length);
        }

        // Fill the unbounded area, trying to the smallest maximum Y coordinate.
        // Set the following for the best solution we find:
        //       xmin, xmax, etc.    - Bounds of the rectangle we are trying to fill.
        //       num_unpositioned    - The number of rectangles not yet positioned in this solution.
        //                             Used to control the recursion.
        //       rects()             - All rectangles for the problem, some positioned and others not. 
        //                             Initially this is the partial solution we are working from.
        //                             At end, this is the best solution we could find.
        //       is_positioned()     - Indicates which rectangles are positioned in this solution.
        //       max_y               - The largest Y value for this solution.
        private void FillUnboundedArea(
         int xmin, int xmax, int ymin,
         ref int numUnpositioned, ref Rectangle[] rects, ref bool[] isPositioned)
        {
            if (numUnpositioned <= 0) return;

            // Save a copy of the solution so far.
            int bestNumUnpositioned = numUnpositioned;
            Rectangle[] bestRects = (Rectangle[])rects.Clone();
            bool[] bestIsPositioned = (bool[])isPositioned.Clone();

            // Currently we have no solution for this area.
            int bestMaxy = int.MaxValue;

            // Loop through the available rectangles.
            for (int i = 0; i <= rects.Length - 1; i++)
            {
                // See if (this rectangle is not yet positioned and will fit.
                if (!isPositioned[i] &&
                    rects[i].Width <= xmax - xmin)
                {
                    // It will fit. Try it.
                    // **************************************************
                    // Divide the remaining area horizontally.
                    int test1NumUnpositioned = numUnpositioned - 1;
                    Rectangle[] test1Rects = (Rectangle[])rects.Clone();
                    bool[] test1IsPositioned = (bool[])isPositioned.Clone();
                    test1Rects[i].X = xmin;
                    test1Rects[i].Y = ymin;
                    test1IsPositioned[i] = true;

                    // Fill the area on the right.
                    FillBoundedArea(xmin + rects[i].Width, xmax, ymin, ymin + rects[i].Height,
                        ref test1NumUnpositioned, ref test1Rects, ref test1IsPositioned);
                    // Fill the area on the bottom.
                    FillUnboundedArea(xmin, xmax, ymin + rects[i].Height,
                        ref test1NumUnpositioned, ref test1Rects, ref test1IsPositioned);

                    // Learn about the test solution.
                    int test1Maxy =
                        MaxY(test1Rects, test1IsPositioned);

                    // See if (this is better than the current best solution.
                    if ((test1NumUnpositioned == 0) && (test1Maxy < bestMaxy))
                    {
                        // The test is better. Save it.
                        bestMaxy = test1Maxy;
                        bestRects = test1Rects;
                        bestIsPositioned = test1IsPositioned;
                        bestNumUnpositioned = test1NumUnpositioned;
                    }

                    // **************************************************
                    // Divide the remaining area vertically.
                    int test2NumUnpositioned = numUnpositioned - 1;
                    Rectangle[] test2Rects = (Rectangle[])rects.Clone();
                    bool[] test2IsPositioned = (bool[])isPositioned.Clone();
                    test2Rects[i].X = xmin;
                    test2Rects[i].Y = ymin;
                    test2IsPositioned[i] = true;

                    // Fill the area on the right.
                    FillUnboundedArea(xmin + rects[i].Width, xmax, ymin,
                        ref test2NumUnpositioned, ref test2Rects, ref test2IsPositioned);
                    // Fill the area on the bottom.
                    FillUnboundedArea(xmin, xmin + rects[i].Width, ymin + rects[i].Height,
                        ref test2NumUnpositioned, ref test2Rects, ref test2IsPositioned);

                    // Learn about the test solution.
                    int test2Maxy =
                        MaxY(test2Rects, test2IsPositioned);

                    // See if (this is better than the current best solution.
                    if ((test2NumUnpositioned == 0) && (test2Maxy < bestMaxy))
                    {
                        // The test is better. Save it.
                        bestMaxy = test2Maxy;
                        bestRects = test2Rects;
                        bestIsPositioned = test2IsPositioned;
                        bestNumUnpositioned = test2NumUnpositioned;
                    }
                } // End trying this rectangle.
            } // End looping through the rectangles.

            // Return the best solution we found.
            isPositioned = bestIsPositioned;
            numUnpositioned = bestNumUnpositioned;
            rects = bestRects;
        }

        // Use rectangles to fill the given sub-area.
        // Set the following for the best solution we find:
        //       xmin, xmax, etc.    - Bounds of the rectangle we are trying to fill.
        //       num_unpositioned    - The number of rectangles not yet positioned in this solution.
        //                             Used to control the recursion.
        //       rects()             - All rectangles for the problem, some positioned and others not. 
        //                             Initially this is the partial solution we are working from.
        //                             At end, this is the best solution we could find.
        //       is_positioned()     - Indicates which rectangles are positioned in this solution.
        //       max_y               - The largest Y value for this solution.
        private void FillBoundedArea(
            int xmin, int xmax, int ymin, int ymax,
            ref int numUnpositioned, ref Rectangle[] rects, ref bool[] isPositioned)
        {
            // See if every rectangle has been positioned.
            if (numUnpositioned <= 0) return;

            // Save a copy of the solution so far.
            int bestNumUnpositioned = numUnpositioned;
            Rectangle[] bestRects = (Rectangle[])rects.Clone();
            bool[] bestIsPositioned = (bool[])isPositioned.Clone();

            // Currently we have no solution for this area.
            double bestDensity = 0;

            // Some rectangles have not been positioned.
            // Loop through the available rectangles.
            for (int i = 0; i <= rects.Length - 1; i++)
            {
                // See if this rectangle is not position and will fit.
                if ((!isPositioned[i]) &&
                    (rects[i].Width <= xmax - xmin) &&
                    (rects[i].Height <= ymax - ymin))
                {
                    // It will fit. Try it.
                    // **************************************************
                    // Divide the remaining area horizontally.
                    int test1NumUnpositioned = numUnpositioned - 1;
                    Rectangle[] test1Rects = (Rectangle[])rects.Clone();
                    bool[] test1IsPositioned = (bool[])isPositioned.Clone();
                    test1Rects[i].X = xmin;
                    test1Rects[i].Y = ymin;
                    test1IsPositioned[i] = true;

                    // Fill the area on the right.
                    FillBoundedArea(xmin + rects[i].Width, xmax, ymin, ymin + rects[i].Height,
                        ref test1NumUnpositioned, ref test1Rects, ref test1IsPositioned);
                    // Fill the area on the bottom.
                    FillBoundedArea(xmin, xmax, ymin + rects[i].Height, ymax,
                        ref test1NumUnpositioned, ref test1Rects, ref test1IsPositioned);

                    // Learn about the test solution.
                    double test1Density =
                        SolutionDensity(
                            xmin + rects[i].Width, xmax, ymin, ymin + rects[i].Height,
                            xmin, xmax, ymin + rects[i].Height, ymax,
                            test1Rects, test1IsPositioned);

                    // See if this is better than the current best solution.
                    if (test1Density >= bestDensity)
                    {
                        // The test is better. Save it.
                        bestDensity = test1Density;
                        bestRects = test1Rects;
                        bestIsPositioned = test1IsPositioned;
                        bestNumUnpositioned = test1NumUnpositioned;
                    }

                    // **************************************************
                    // Divide the remaining area vertically.
                    int test2NumUnpositioned = numUnpositioned - 1;
                    Rectangle[] test2Rects = (Rectangle[])rects.Clone();
                    bool[] test2IsPositioned = (bool[])isPositioned.Clone();
                    test2Rects[i].X = xmin;
                    test2Rects[i].Y = ymin;
                    test2IsPositioned[i] = true;

                    // Fill the area on the right.
                    FillBoundedArea(xmin + rects[i].Width, xmax, ymin, ymax,
                        ref test2NumUnpositioned, ref test2Rects, ref test2IsPositioned);
                    // Fill the area on the bottom.
                    FillBoundedArea(xmin, xmin + rects[i].Width, ymin + rects[i].Height, ymax,
                        ref test2NumUnpositioned, ref test2Rects, ref test2IsPositioned);

                    // Learn about the test solution.
                    double test2Density =
                        SolutionDensity(
                            xmin + rects[i].Width, xmax, ymin, ymax,
                            xmin, xmin + rects[i].Width, ymin + rects[i].Height, ymax,
                            test2Rects, test2IsPositioned);

                    // See if this is better than the current best solution.
                    if (test2Density >= bestDensity)
                    {
                        // The test is better. Save it.
                        bestDensity = test2Density;
                        bestRects = test2Rects;
                        bestIsPositioned = test2IsPositioned;
                        bestNumUnpositioned = test2NumUnpositioned;
                    }
                } // End trying this rectangle.
            } // End looping through the rectangles.

            // Return the best solution we found.
            isPositioned = bestIsPositioned;
            numUnpositioned = bestNumUnpositioned;
            rects = bestRects;
        }

        // Find the largest Y coordinate in the solution.
        private int MaxY(Rectangle[] rects, bool[] isPositioned)
        {
            int maxY = 0;
            for (int i = 0; i <= rects.Length - 1; i++)
                if (isPositioned[i] && (maxY < rects[i].Bottom)) maxY = rects[i].Bottom;
            return maxY;
        }


        // Find the density of the rectangles in the given areas for this solution.
        private double SolutionDensity(
            int xmin1, int xmax1, int ymin1, int ymax1,
            int xmin2, int xmax2, int ymin2, int ymax2,
            Rectangle[] rects, bool[] isPositioned)
        {
            Rectangle rect1 = new Rectangle(xmin1, ymin1, xmax1 - xmin1, ymax1 - ymin1);
            Rectangle rect2 = new Rectangle(xmin2, ymin2, xmax2 - xmin2, ymax2 - ymin2);
            int areaCovered = 0;
            for (int i = 0; i <= rects.Length - 1; i++)
            {
                if (isPositioned[i] &&
                    (rects[i].IntersectsWith(rect1) ||
                     rects[i].IntersectsWith(rect2)))
                {
                    areaCovered += rects[i].Width * rects[i].Height;
                }
            }

            double denom = rect1.Width * rect1.Height + rect2.Width * rect2.Height;
            if (System.Math.Abs(denom) < 0.001) return 0;

            return areaCovered / denom;
        }

        // Start the recursion.
        public void AlgRecursiveDivision(int binWidth, Rectangle[] rects)
        {
            // Sort by height.
            Rectangle[] bestSolution = (Rectangle[])rects.Clone();
            System.Array.Sort(rects, new HeightComparer());

            // Make variables to track and record the best solution.
            bool[] isPositioned = new bool[rects.Length];
            int numUnpositioned = rects.Length;

            // Perform the recursion.
            FillUnboundedArea(0, binWidth, 0, ref numUnpositioned, ref bestSolution, ref isPositioned);

            // Save the best solution.
            System.Array.Copy(bestSolution, rects, rects.Length);
        }
    }
}
