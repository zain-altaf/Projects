using System;
using static System.Console;

namespace Bme121
{
    static class Program
    {
        static bool[ , ] isTileThere; // boolean variable holding the places of the board
        static string nameA, nameB; // player names
        static int numRowsRequested, numColumnsRequested; // the number of rows and columns of the game board
        static int platformRowA, platformColumnA; // platform for platform A
        static int platformRowB, platformColumnB; // platform for platform B
        static int positionRowA, positionColA; // The position of pawn A
        static int positionRowB, positionColB; // The position of pawn B
        static int rowChangeA, colChangeA; // the pawn changing position for A
        static int pARowRemove, pAColRemove; // the tile removal requested by A
        static int rowChangeB, colChangeB; // the pawn changing position for B
        static int pBRowRemove, pBColRemove; // the tile removal requested by A
        static bool gameRunning = true; // This bool is basically holding whether the players can keep playing

        static void Main( ) //very basic main method that just loops the game board and the moves method
        {
          Console.Clear();
          WriteLine(" ---------------------- ");
          WriteLine("| Welcome to Isolation |");
          WriteLine(" ---------------------- ");
          WriteLine();
          Initialization();
          while(gameRunning == true)
          {
              DrawGameBoard();
              Moves();
          }

        }


//***********************************START OF INITIALIZATION****************************
        static void Initialization()
        {
          string[ ] letters = { "a","b","c","d","e","f","g","h","i","j","k","l",
          "m","n","o","p","q","r","s","t","u","v","w","x","y","z"}; // string letters required for platform

          /*int[] numbers = new int [26]; // might be necessary when converting between letters and numbers
          for (int i = 0; i < numbers.Length; i++)
          {
            numbers[i] = i;
          }*/

          //Player A name input
          Write( "Enter your name [default Player A]: " );
          nameA = ReadLine( );
          if( nameA.Length == 0 ) nameA = "Player A";
          WriteLine( "name: {0}", nameA );
          WriteLine();

          //Player B name input
          Write( "Enter your name [default Player B]: " );
          nameB = ReadLine( );
          if( nameB.Length == 0 ) nameB = "Player B";
          WriteLine( "name: {0}", nameB );
          WriteLine();

          // error checking with a while loop and boolean variable for the number of rows
         bool rcRequested = false;

         while ( rcRequested == false )
         {
             Write ( "Enter a number for the number of rows [4-26] or press ENTER for 6 rows: " );
             string rowpossible = ReadLine( );
             if ( rowpossible.Length == 0 ) numRowsRequested = 6;
             else numRowsRequested = int.Parse( rowpossible );
             if ( numRowsRequested < 4 || numRowsRequested > 26 )
             {
                 WriteLine( "The number of rows must be between 4 and 26. " );
                 WriteLine();
             }
             else rcRequested = true;
         }

         // error checking with a while loop and boolean variable for the number of columns
          rcRequested = false;
         while ( rcRequested == false )
         {
             Write( "Enter a number for the number of columns [4-26] or press ENTER for 8 columns: " );
             string colpossible = ReadLine( );
             if ( colpossible.Length == 0 ) numColumnsRequested = 8;
             else numColumnsRequested = int.Parse( colpossible );
             if ( numColumnsRequested < 4 || numColumnsRequested > 26 )
             {
                 WriteLine( "The number of columns must be between 4 and 26. " );
                 WriteLine();
             }
             else rcRequested = true;
         }

         // storing the board and validating all tiles to be set to true for ease of setting false
          isTileThere = new bool [numRowsRequested, numColumnsRequested];
          for(int k = 0; k < numRowsRequested; k++)
          {
            for(int t = 0; t < numColumnsRequested; t++)
            {
              isTileThere[k,t] = true;
            }
          }

          // Start of initialization of the players platforms
          WriteLine();

          // checker for invalid letters entered
          bool checker = false;

          while(checker == false)
          {
            // input from user for the row platform for A
            Write( "Enter 2 Letters for Pawn A's Platform or Press Enter: " );
            string response = ReadLine( );

            // if the inputer presses enter it just defaults the position
            if(response.Length == 0)
            {
              platformRowA = positionRowA = (int) Math.Ceiling((numRowsRequested-1)/2.0);
              platformColumnA = 0;
              WriteLine("The deafault row and column is {0}, {1}", letters[platformRowA], letters[platformColumnA] );
              WriteLine();
              checker = true;
            }

            //If the user enters too many or too less letters
            else if(response.Length != 2)
            {
              WriteLine("You didn't enter 2 letters: ");
            }

            //if the user enters the valid number of letters this checks if it is within bounds
            else
            {
              platformRowA = Array.IndexOf(letters, response.Substring(0,1));
              platformColumnA = Array.IndexOf(letters,response.Substring(1,1));

              if (platformRowA > isTileThere.GetLength(0) -1 || platformColumnA > isTileThere.GetLength(1) -1)
              {
                WriteLine( "You entered values out of bounds " );
              }

              else
              {
                checker = true;
              }

            }
          }

          checker = false;
          while(checker == false)
          {
            // input from user for the row platform for B
            Write( "Enter 2 Letters for Pawn B's Platform or Press Enter: " );
            string response = ReadLine( );

            // if the inputer presses enter it just defaults the position
            if(response.Length == 0)
            {
              platformRowB = positionRowB = (int) Math.Ceiling((numRowsRequested-1)/2.0);
              platformColumnB = isTileThere.GetLength(1)-1;
              WriteLine("The deafault row and column is {0}, {1}", letters[platformRowB], letters[platformColumnB] );
              WriteLine();
              checker = true;
            }

            //If the user enters too many or too less letters
            else if(response.Length != 2)
            {
              WriteLine("You didn't enter two letters");
            }

            //if the user enters the valid number of letters this checks if it is within bounds and in the case of B it checks that it doesn't end up on A
            else
            {
              platformRowB = Array.IndexOf(letters, response.Substring(0,1));
              platformColumnB = Array.IndexOf(letters,response.Substring(1,1));

              if(platformRowB > isTileThere.GetLength(0) -1 || platformColumnB > isTileThere.GetLength(1)-1
                    || (platformRowB == platformRowA && platformColumnB == platformColumnA))
              {
                WriteLine("You are out of bounds or you entered the same platform as {0}", nameA);
              }

              else checker = true;
            }
          }

          //This gives a position to the platform and position of the pawn
          positionRowA = platformRowA;
          positionColA = platformColumnA;
          positionRowB = platformRowB;
          positionColB = platformColumnB;

        } // end of the initialization method



//***********************************START OF GAMEBOARD METHOD****************************
        static void DrawGameBoard( )
        {
            Console.Clear();
            const string h  = "\u2500"; // horizontal line
            const string v  = "\u2502"; // vertical line
            const string tl = "\u250c"; // top left corner
            const string tr = "\u2510"; // top right corner
            const string bl = "\u2514"; // bottom left corner
            const string br = "\u2518"; // bottom right corner
            const string vr = "\u251c"; // vertical join from right
            const string vl = "\u2524"; // vertical join from left
            const string hb = "\u252c"; // horizontal join from below
            const string ha = "\u2534"; // horizontal join from above
            const string hv = "\u253c"; // horizontal vertical cross
            //const string sp = " ";   // space
            //const string pa = "A";      // pawn A
            //const string pb = "B";      // pawn B
            const string bb = "\u25a0"; // block
            const string fb = "\u2588"; // left half block
            //const string lh = "\u258c"; // left half block
            //const string rh = "\u2590"; // right half block

            string[ ] letters = { "a","b","c","d","e","f","g","h","i","j","k","l",
                "m","n","o","p","q","r","s","t","u","v","w","x","y","z"};


            Write("   ");
            for(int p = 0; p < isTileThere.GetLength(1); p++)
            {
                Write("  {0} ", letters[p]);
            }

            WriteLine();
            Write( "   " );
            for( int c = 0; c < isTileThere.GetLength( 1 ); c ++ )
            {
                if( c == 0 ) Write( tl );
                Write( "{0}{0}{0}", h );
                if( c == isTileThere.GetLength( 1 ) - 1 ) Write( "{0}", tr );
                else                                Write( "{0}", hb );
            }
            WriteLine( );

            // Draw the board rows.
            for( int r = 0; r < isTileThere.GetLength( 0 ); r ++ )
            {
                Write( " {0} ", letters[ r ] );

                // Draw the row contents.
                for( int c = 0; c < isTileThere.GetLength( 1 ); c ++ )
                {
                    if( c == 0 ) Write( v );

                    // In the drawing row contents I made sure to start doing the display of the players pawns, platforms, tiles and removed tiles
                    if( isTileThere[ r, c ] )
                    {
                      if( r == positionRowA && c == positionColA) Write(" A " + v); // This displays player A pawn
                      else if( r == positionRowB && c == positionColB) Write(" B " + v); // This displays player B pawn
                      else if( r == platformRowA && c == platformColumnA) Write(" {0} {1}", bb, v); // this displays player A's platform
                      else if( r == platformRowB && c == platformColumnB) Write(" {0} {1}", bb, v ); // this displays player B's platform
                      else Write(" {0} {1}", fb, v); // this displays the tile
                    }

                    else if (isTileThere[pARowRemove,pAColRemove] == false) // this ensures to check that if the tile is false on a particular place then display blank spaces
                    {
                        Write("   " + v);
                    }
                    else if (isTileThere[pBRowRemove, pAColRemove] == false) // This is checking for B removal of any tiles
                    {
                      Write("   " + v);
                    }
                }
                WriteLine( );

                // Draw the boundary after the row.
                if( r != isTileThere.GetLength( 0 ) - 1 )
                {
                    Write( "   " );
                    for( int c = 0; c < isTileThere.GetLength( 1 ); c ++ )
                    {

                        if( c == 0 ) Write( vr ); // would I need to modify my if to say if it doesn't have a pawn or a
                        Write( "{0}{0}{0}", h );
                        if( c == isTileThere.GetLength( 1 ) - 1 ) Write( "{0}", vl );
                        else                                Write( "{0}", hv );
                    }
                    WriteLine( );
                }
                else
                {
                    Write( "   " );
                    for( int c = 0; c < isTileThere.GetLength( 1 ); c ++ )
                    {
                        if( c == 0 ) Write( bl );
                        Write( "{0}{0}{0}", h );
                        if( c == isTileThere.GetLength( 1 ) - 1 ) Write( "{0}", br );
                        else                                Write( "{0}", ha );
                    }
                    WriteLine( );
                }
            }
        } // end of game board method

//***********************************START OF MOVES METHOD********************************


        static void Moves() //Make move method, given player
        {

          string[ ] letters = { "a","b","c","d","e","f","g","h","i","j","k","l",
              "m","n","o","p","q","r","s","t","u","v","w","x","y","z"}; // including the letters in order to convert the player responses

          bool isValid = false; // this is the boolean I used to check for both player A and B

          while(!isValid) // this while loop will check that all the entering of values are true or it won't break the loop
          {
            WriteLine();
            WriteLine("{0}, it's your turn", nameA); // displaying which players turn it is
            WriteLine();

            Write( "Please enter a 4 letter move, move to [ab] remove [cd] : "); //prompt, get move from user
            string move = ReadLine( );

            while (move.Length != 4) // to avoid unhandeled exceptions I made a while loop that makes sure the player enters a new 4 letter move
            {
              Write("That is not a 4 letter move re-enter a 4 letter move: ");
              move = ReadLine();
            }

            rowChangeA = Array.IndexOf(letters, move.Substring(0,1)); // This is the first part of the move
            colChangeA = Array.IndexOf(letters, move.Substring(1,1)); // this is the second part
            pARowRemove = Array.IndexOf(letters,move.Substring(2,1)); // this is the removal rows
            pAColRemove = Array.IndexOf(letters,move.Substring(3,1)); // this is for the removal of column

            // This while loop is making sure that the move that the player wants to make is within the board bounds
            while(rowChangeA > isTileThere.GetLength(0) - 1 || colChangeA > isTileThere.GetLength(1) -1)
            {
              WriteLine("Please enter a value be within board: ");
              move = ReadLine();

              rowChangeA = Array.IndexOf(letters, move.Substring(0,1)); // This is the first part of the move
              colChangeA = Array.IndexOf(letters, move.Substring(1,1)); // this is the second part
              pARowRemove = Array.IndexOf(letters,move.Substring(2,1)); // this is the removal rows
              pAColRemove = Array.IndexOf(letters,move.Substring(3,1)); // this is for the removal of column
            }

            //This branch of if else's is checking if the move is logically correct

            //this checks that A does move and doesn't stay on the same tile
            if (rowChangeA == positionRowA && colChangeA == positionColA)
            {
              WriteLine("You are on this tile");
            }

            //this checks that A doesn't move onto B
            else if(rowChangeA == positionRowB && colChangeA == positionColB)
            {
              WriteLine("Pawn B is on this Tile");
            }

            //making sure that the player is not moving onto a false tile
            else if(isTileThere[rowChangeA,colChangeA] == false)
            {
              WriteLine("You can't move to a removed tile");
            }

            //This is making sure that the player is not moving more than 1 in the column and row direction
            else if((positionRowA - 1 > rowChangeA || rowChangeA > positionRowA + 1) || (positionColA - 1 > colChangeA || colChangeA > positionColA + 1) )
            {
              WriteLine("Your cannot move more than 1 space in the row and column direction");
            }

            /* I combined this else if statement to check all possibilities of A not removing itself, it's platform, B's platform,
            not removing the row and column that A moves into, it doesn't remove pawn B, and finally checking that it is a removal of an existent tile on the board */
            else if ((pARowRemove == positionRowA && pAColRemove == positionColA) ||
                    (pARowRemove == platformRowA && pAColRemove == platformColumnA) || (pARowRemove == platformRowB && pAColRemove == platformColumnB) ||
                    (pARowRemove == rowChangeA && pAColRemove == colChangeA) || (pARowRemove == positionRowB && pAColRemove == positionColB) ||
                    (pARowRemove > isTileThere.GetLength(0) - 1 || pAColRemove > isTileThere.GetLength(1) - 1) ||
                    (isTileThere[pARowRemove,pAColRemove] == false))
            {
              WriteLine("cannot remove tile");
            }

            // This else statement basically says that if all the conditions are met then update the position of pawn A and remove the tile specified and then exit the loop
            else
            {
              positionRowA = rowChangeA;
              positionColA = colChangeA;
              isTileThere[pARowRemove, pAColRemove] = false;
              isValid = true;
            }
          }

          // It wasn't updating without a draw board so I drew it
          DrawGameBoard();


          isValid = false;

          while(!isValid)
          {

              WriteLine();
              WriteLine("{0}, it's your turn", nameB);//Showing it is B's turn
              WriteLine();

              Write( "Please enter a 4 letter move, move to [ab] remove [cd] : "); //prompt, get move from user
              string move = ReadLine( );

              while (move.Length != 4)
              {
                Write("That is not a 4 letter move re-enter: ");
                move = ReadLine();
              }

              rowChangeB = Array.IndexOf(letters, move.Substring(0,1)); // This is the first part of the move
              colChangeB = Array.IndexOf(letters, move.Substring(1,1)); // this is the second part
              pBRowRemove = Array.IndexOf(letters,move.Substring(2,1)); // this is the removal rows
              pBColRemove = Array.IndexOf(letters,move.Substring(3,1)); // this is for the removal of column

              // checking if the move is in board bounds
              while(rowChangeB > isTileThere.GetLength(0) - 1 || colChangeB > isTileThere.GetLength(1) -1)
              {
                Write("Must be within board: ");
                move = ReadLine();

                rowChangeB = Array.IndexOf(letters, move.Substring(0,1)); // This is the first part of the move
                colChangeB = Array.IndexOf(letters, move.Substring(1,1)); // this is the second part
                pBRowRemove = Array.IndexOf(letters,move.Substring(2,1)); // this is the removal rows
                pBColRemove = Array.IndexOf(letters,move.Substring(3,1)); // this is for the removal of column
              }

              // following A, this also checks all the possible logical issues
              if (rowChangeB == positionRowB && colChangeB == positionColB)
              {
                WriteLine("You are on this tile");
              }

              else if(rowChangeB == positionRowA && colChangeB == positionColA)
              {
                WriteLine("Pawn A is on this Tile");
              }

              else if(isTileThere[rowChangeB,colChangeB] == false)
              {
                WriteLine("You can't move to a removed tile");
              }

              else if((positionRowB - 1 > rowChangeB || rowChangeB > positionRowB + 1) || (positionColB - 1 > colChangeB || colChangeB > positionColB + 1) )
              {
                WriteLine("Your cannot move more than 1 space in the row and column direction");
              }

              else if ((pBRowRemove == positionRowB && pBColRemove == positionColB)||
                      (pBRowRemove == platformRowB && pBColRemove == platformColumnB) || (pBRowRemove == platformRowA && pBColRemove == platformColumnA) ||
                      (pBRowRemove == rowChangeB && pBColRemove == colChangeB) || (pBRowRemove == positionRowA && pBColRemove == positionColA) ||
                      (pBRowRemove > isTileThere.GetLength(0) - 1 || pBColRemove > isTileThere.GetLength(1) - 1) ||
                      (isTileThere[pBRowRemove,pBColRemove] == false))
              {
                WriteLine("cannot remove tile");
              }

              // if all conditions are met than update the move of B and update the tile removal and exit the loop
              else
              {
                positionRowB = rowChangeB;
                positionColB = colChangeB;
                isTileThere[pBRowRemove, pBColRemove] = false;
                isValid = true;
              }
        }
    }// method ending
  }
}
