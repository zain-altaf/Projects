#nullable enable
using System;
using static System.Console;
using MediCal;

namespace Bme121
{
    partial class LinkedList
    {
        // Method used to indicate a target Drug object when searching.

        public static bool IsTarget( Drug data )
        {
            return data.Name.StartsWith( "FOSAMAX", StringComparison.OrdinalIgnoreCase );
        }

        // Method used to compare two Drug objects when sorting.
        // Return is -/0/+ for a<b/a=b/a>b, respectively.

        public static int Compare( Drug a, Drug b )
        {
            return string.Compare( a.Name, b.Name, StringComparison.OrdinalIgnoreCase );
        }

        // Method used to add a new Drug object to the linked list in sorted order.


        public void InsertInOrder( Drug newData )
        {

            Node? newNode = new Node ( newData );

            if ( newData == null ) throw new ArgumentNullException ( nameof ( newData ) );

            // linked list is empty
            if ( Count == 0 )
            {
                Tail = newNode;
                Head = newNode;
                Count++;
                return;
            }

            //linked list is storing something
            else
            {
                Node? previousNode = null;
                Node? currNode = Head;

                // going through the list
                for ( int i = 0; i < Count; i++ )
                {
                    //WriteLine("{0} {1} {2}", newNode.Data, currNode.Data, Compare(newNode.Data, currNode!.Data));
                    if(Compare(newNode.Data, currNode!.Data) <= 0 )
                    {
                        // this is if the new node is going to go ahead of the current one inserted in the list
                        if(currNode == Head)
                        {
              							Node? oldHead = Head;
              							Head = newNode;
              							newNode.Next = oldHead;
              							Count++;
              							return;
                        }

                        // If A = B ie you are going through the list and have not reached the end yet
                        else
                        {
              							previousNode!.Next = newNode;
              							newNode.Next = currNode;
              							Count++;
              							return;
                        }

                        //once done the iteration there needs to be a change in the current and previous node
                    }

                    previousNode = currNode;
                    currNode = currNode.Next;
                }

                Node? oldTail = Tail;
                Tail = newNode;
                newNode.Next = null;
                oldTail!.Next = newNode;
                Count++;
                return;

                // Return is -/0/+ for a<b/a=b/a>b, respectively.
            }

        }
    }

    static class Program
    {
        // Method to test operation of the linked list.

        static void Main( )
        {
            Drug[ ] drugArray = Drug.ArrayFromFile( "RXQT1503-100.txt" );

            LinkedList drugList = new LinkedList( );
            foreach( Drug d in drugArray ) drugList.InsertInOrder( d );

            WriteLine( "drugList.Count = {0}", drugList.Count );
            foreach( Drug d in drugList.ToArray( ) ) WriteLine( d );
        }
    }
}
