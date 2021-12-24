using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.ComponentModel;
using System.Numerics;
using System.Text;
using System.Collections.Immutable;

namespace AOC._2021
{
    public class Dec23
    {
        public class Amphipod
        {
            public readonly char type;
            public readonly int target_column;
            public readonly int cost_per_move;

            public Amphipod(char t)
            {
                type = t;

                switch (t)
                {
                    case 'A':
                        target_column = 2;
                        cost_per_move = 1;
                        break;

                    case 'B':
                        target_column = 4;
                        cost_per_move = 10;
                        break;

                    case 'C':
                        target_column = 6;
                        cost_per_move = 100;
                        break;

                    case 'D':
                        target_column = 8;
                        cost_per_move = 1000;
                        break;

                    default:
                        throw new ArgumentException(String.Format("Invalid amphipod character '{0}'", t));
                }
            }
        }

        static int[] k_home_columns = { 2, 4, 6, 8 };
        static int[] k_hallway_parking_spot_columns = { 0, 1, 3, 5, 7, 9, 10 };

        internal class Grid
        {
            public readonly int home_depth;
            private Amphipod[][] burrow_positions;

            // Create a burrow with no amphipods in it
            public Grid(int h)
            {
                home_depth = h;
                burrow_positions = new Amphipod[11][];

                foreach (int column in k_home_columns)
                {
                    burrow_positions[column] = new Amphipod[home_depth + 1];
                }

                foreach (int column in k_hallway_parking_spot_columns)
                {
                    burrow_positions[column] = new Amphipod[1];
                }
            }

            // Copy constructor. Sharing amphipod references doesn't matter.
            public Grid(Grid other) : this(other.home_depth)
            {
                for (int column = 0; column < burrow_positions.Length; column++)
                {
                    for (int row = 0; row < burrow_positions[column].Length; row++)
                    {
                        burrow_positions[column][row] = other.burrow_positions[column][row];
                    }
                }
            }

            // Place a single amphipod in the burrow.
            public void place_amphipod(char type, int row, int column)
            {
                burrow_positions[column][row] = new Amphipod(type);
            }

            // Try to send the amphipod in this spot to thier home
            private (bool, int) try_send_to_home(
                int start_column,
                int start_row)
            {
                Amphipod start = burrow_positions[start_column][start_row];

                // Assume if they start in a home, then nobody is above them.
                bool path_home_clear = true;

                int min_hallway_column = Math.Min(start_column, start.target_column);
                int max_hallway_column = Math.Max(start_column, start.target_column);

                // check if the path along the hallway is clear
                for (int path_column = min_hallway_column;
                    path_column <= max_hallway_column && path_home_clear;
                    path_column++)
                {
                    Amphipod hallway_position = burrow_positions[path_column][0];
                    if (hallway_position != null && hallway_position != start)
                    {
                        // a spot in the hallway isn't clear
                        path_home_clear = false;
                    }
                }

                int target_row = 0;

                // check to see if the home is clear or filled with friends
                Amphipod[] home = burrow_positions[start.target_column];

                for (int path_row = 1; path_row < home.Length && path_home_clear; path_row++)
                {
                    Amphipod home_position = home[path_row];

                    // Every time we find a clear home position, it's the best candidate for the target position.
                    if (home_position == null)
                    {
                        target_row = path_row;
                    }

                    // If we find someone in the home that shouldn't be there, then we can't move to this home right now.
                    if (home_position != null && home_position.type != start.type)
                    {
                        path_home_clear = false;
                    }
                }

                // If the path home was clear, move the amphipod and return the total cost.
                if (path_home_clear)
                {
                    int cost = (start_row + target_row + Math.Abs(start.target_column - start_column)) * start.cost_per_move;

                    burrow_positions[start.target_column][target_row] = start;
                    burrow_positions[start_column][start_row] = null;

                    return (true, cost);
                }

                return (false, 0);
            }

            // Try to send an amphipod in the hallway to their home
            private (bool, int) try_send_hallway_to_home()
            {
                // Look at each spot in the hallway.
                for (int start_column = 0; start_column < burrow_positions.Length; start_column++)
                {
                    Amphipod start = burrow_positions[start_column][0];

                    // If we found an amphibod in the hallway, check to see if their path home is clear
                    if (start != null)
                    {
                        // If we can move them home, return that cost.
                        (bool moved, int cost) = try_send_to_home(start_column, 0);

                        if (moved)
                        {
                            return (moved, cost);
                        }
                    }
                }

                return (false, 0);
            }

            // Keep trying to find amphipods in the hallway to send home if we can
            private int try_clear_hallway()
            {
                int cost = 0;

                // Loop for as long as we are able to move somebody home.
                bool amphipod_moved_home;
                do
                {
                    int move_cost = 0;

                    (amphipod_moved_home, move_cost) = try_send_hallway_to_home();

                    cost += move_cost;

                } while (amphipod_moved_home);

                return cost;
            }

            // Find the top amphipod in this home, but only if this home isn't solved yet.
            private (Amphipod, int) find_top_in_unfinished_home(int column)
            {
                Amphipod[] start_home = burrow_positions[column];

                int top_row = 0;
                Amphipod top = null;
                bool home_has_neighbors = false;

                // Find the top amphipod in this home
                for (int row = 1; row < start_home.Length; row++)
                {
                    Amphipod current = start_home[row];

                    if (current != null)
                    {
                        // Remember the top one we find
                        if (top == null)
                        {
                            top_row = row;
                            top = current;
                        }

                        // Remember if any aren't supposed to be there
                        if (current.target_column != column)
                        {
                            home_has_neighbors = true;
                        }
                    }
                }

                // Only return success if this home is not solved.
                if (!home_has_neighbors)
                {
                    return (null, 0);
                }
                else
                {
                    return (top, top_row);
                }
            }

            // Try to send an amphipod in a wrong home to their correct home
            private (bool, int) try_send_home_to_home()
            {
                // Look in each home
                foreach (int start_column in k_home_columns)
                {
                    Amphipod[] start_home = burrow_positions[start_column];

                    (Amphipod start, int start_row) = find_top_in_unfinished_home(start_column);

                    // If the top amphipod in this home was found, and isn't in his correct home...
                    if (start != null && start.target_column != start_column)
                    {
                        // If we can move them home, do so and return that cost.
                        (bool moved, int cost) = try_send_to_home(start_column, start_row);

                        if (moved)
                        {
                            return (moved, cost);
                        }
                    }
                }

                return (false, 0);
            }

            // Keep try finding amphipods in a wrong home to their correct home if we can
            private int try_clear_homes()
            {
                int cost = 0;

                // Loop for as long as we are able to move somebody home.
                bool amphipod_moved_home;
                do
                {
                    int move_cost = 0;

                    (amphipod_moved_home, move_cost) = try_send_home_to_home();

                    cost += move_cost;

                } while (amphipod_moved_home);

                return cost;
            }

            // return true if the burrow is solved
            private bool is_solved()
            {
                foreach (int column in k_home_columns)
                {
                    Amphipod[] home = burrow_positions[column];

                    for (int row = 1; row < home.Length; row++)
                    {
                        Amphipod current = home[row];

                        if (current == null || current.target_column != column)
                        {
                            return false;
                        }
                    }
                }

                return true;
            }

            // Get the range of columns an amphipode could move to from a home column.
            private (int, int) get_valid_parking_spots(int column)
            {
                int min_column = column + 1;

                while (min_column > 0 && burrow_positions[min_column - 1][0] == null)
                {
                    min_column--;
                }

                int max_column = column - 1;

                while (max_column < burrow_positions.Length - 1 && burrow_positions[max_column + 1][0] == null)
                {
                    max_column++;
                }

                // max < min is a valid failure condition.
                return (min_column, max_column);
            }

            // Get the character to display for a given row+column
            private char get_display_char(Amphipod[] column, int row)
            {
                return column[row] == null ? '.' : column[row].type;
            }

            // display for public use to display solution steps.
            public void display()
            {
                display(null, "");
            }

            // display for private use for 'pretty' detailed/discarded steps.
            private void display(string title, string depth_string)
            {
                Console.WriteLine();
                if (title != null)
                {
                    Console.WriteLine(depth_string + title);
                }
                Console.WriteLine(depth_string + "#############");
                Console.WriteLine(depth_string + "#{0}#", string.Join("", burrow_positions.Select(column => get_display_char(column, 0))));

                for (int row = 1; row <= home_depth; row++)
                {
                    if (row == 1) Console.Write(depth_string + "###");
                    else Console.Write(depth_string + "  #");

                    foreach (int column_index in k_home_columns)
                    {
                        Amphipod[] column = burrow_positions[column_index];

                        Console.Write("{0}#", get_display_char(column, row));
                    }

                    if (row == 1) Console.WriteLine("##");
                    else Console.WriteLine();
                }

                Console.WriteLine(depth_string + "  #########");
            }

            // Try to solve the Grid
            public (bool, int) try_solve(bool pretty, List<Grid> solution_stack, string depth_string = "")
            {
                solution_stack.Add(new Grid(this));

                if (pretty)
                {
                    display("try_solve", depth_string);
                }

                int cost = 0;

                // First try to move any amphipods to their solved positions. This is always the best thing to do.
                int clear_cost = int.MaxValue;
                while (clear_cost > 0)
                {
                    clear_cost = 0;

                    clear_cost += try_clear_hallway();
                    clear_cost += try_clear_homes();

                    cost += clear_cost;
                }

                // if some amphipods were moved to their homes, note that in the solution stack.
                if (cost > 0)
                {
                    solution_stack.Add(new Grid(this));

                    if (pretty)
                    {
                        display(string.Format("moved some home (cost = {0})", cost), depth_string);
                    }
                }

                // If the burrow is solved, return.
                if (is_solved())
                {
                    if (pretty)
                    {
                        Console.WriteLine(depth_string + "solved!");
                    }

                    return (true, cost);
                }

                // I don't know what to do now but we have to move somebody out of a burrow.
                // So try using brute force on every possible move and use the best result we find.
                // ... turns out this still solves things in about a second. Yay!

                List<Grid> best_guess_stack = null;
                bool best_guess_solved = false;
                int best_guess_cost = int.MaxValue;

                // Loop through each home we could pull an amphipod out of
                foreach (int start_column in k_home_columns)
                {
                    (Amphipod start, int start_row) = find_top_in_unfinished_home(start_column);

                    // If the top amphipod in this home was found
                    if (start != null)
                    {
                        (int min_end_column, int max_end_column) = get_valid_parking_spots(start_column);

                        // Loop through each place we could move it to
                        foreach (int end_column in k_hallway_parking_spot_columns)
                        {
                            if (end_column >= min_end_column && end_column <= max_end_column)
                            {
                                Amphipod end = this.burrow_positions[end_column][0];

                                // If we find an empty spot to move it to
                                if (end == null)
                                {
                                    // Move the amphipod to that spot in the hallway and recurse.

                                    Grid guess = new Grid(this);

                                    guess.burrow_positions[end_column][0] = start;
                                    guess.burrow_positions[start_column][start_row] = null;

                                    List<Grid> guess_stack = new List<Grid>();

                                    (bool guess_solved, int guess_cost) = guess.try_solve(pretty, guess_stack, depth_string + "\t");

                                    // If this branch of recursion solved the burrow and it's the best cost we've found so far, save the results.
                                    if (guess_solved)
                                    {
                                        int initial_guess_cost = (start_row + Math.Abs(end_column - start_column)) * start.cost_per_move;

                                        guess_cost += initial_guess_cost;

                                        if (guess_cost < best_guess_cost)
                                        {
                                            best_guess_solved = true;
                                            best_guess_cost = guess_cost;
                                            best_guess_stack = guess_stack;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                // Some branch of recursion found a solution.
                if (best_guess_solved)
                {
                    if (pretty)
                    {
                        Console.WriteLine(depth_string + "a guess solved");
                    }

                    solution_stack.AddRange(best_guess_stack);

                    return (best_guess_solved, cost + best_guess_cost);
                }
                // No solution was found.
                else
                {
                    if (pretty)
                    {
                        Console.WriteLine(depth_string + "no guesses solved");
                    }

                    return (false, 0);
                }
            }
        }

        // Parse amphipod positions from input and put each one in the burrow.
        internal static void ParseInput(
            in string[] input,
            Grid borrow,
            in bool pretty)
        {
            int row = 1;
            for(int i=2; i< input.Length; i++)
            {
                var input_line = input[i].Substring(1);

                for (int column = 0; column < input_line.Length; column++)
                {
                    char type = input_line[column];
                    if (type >= 'A' && type <= 'D')
                    {
                        borrow.place_amphipod(type, row, column);
                    }
                }

                row++;

            }
        }

        public static int Day_23_Worker(
            int home_depth,
            string[] input,
            bool pretty)
        {
            Grid burrow = new Grid(home_depth);

            ParseInput(input, burrow, pretty);

            List<Grid> solution_stack = new List<Grid>();

            (bool solved, int cost) = burrow.try_solve(pretty, solution_stack);

            /*
            foreach (c_burrow solution_burrow in solution_stack)
            {
                solution_burrow.display();
            }
            */

            /*
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine();
            Console.WriteLine("Result = {0} {1}", solved, cost);
            Console.ResetColor();
            */

            return cost;
        }

        public static int Part_1(
            string[] input,
            bool pretty)
        {
            return Day_23_Worker(2, input, pretty);
        }

        public static int Part_2(
            string[] input,
            bool pretty)
        {
            return Day_23_Worker(4, input, pretty);
        }

        public int Puzzle1(string[] lines)
        {
            return Part_1(lines, false);
        }

        public int Puzzle2(string[] lines)
        {
            return Part_2(lines, false);
        }
    }
}
