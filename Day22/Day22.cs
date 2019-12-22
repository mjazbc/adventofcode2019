using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Util;

namespace Day22
{
    class Day22 : AdventPuzzle
    {
        public override string SolveFirstPuzzle()
        {
            var instructions = ParseInput();
            var deck = InitDeck(10007);

            var resultCard = deck.ToArray()[2020];

            foreach(var instruction in instructions)
            {
                switch (instruction.Item1)
                {
                    case Technique.NewStack:
                        ReverseList(ref deck); 
                        break;
                    case Technique.Cut:
                        Cut(instruction.Item2, ref deck);
                        break;
                    case Technique.Increment:
                        Increment(instruction.Item2, ref deck);
                        break;
                    default: throw new Exception();
                }

                Console.WriteLine(deck.ToList().IndexOf(resultCard));
            }

            return deck.ToList().IndexOf(resultCard).ToString();

            throw new NotImplementedException();
        }

        public void Increment(long step, ref LinkedList<long> deck)
        {
            var newDeck = new long[deck.Count];
            var current = deck.First;
            long idx = 0;

            while(current != null)
            {
                newDeck[idx] = current.Value;
                deck.RemoveFirst();
                current = deck.First;

                idx = (idx + step) % newDeck.Length;
            }

            deck = new LinkedList<long>(newDeck);
        }

        public void Cut(long cut, ref LinkedList<long> deck)
        { 
            if(cut < 0)
            {
                for(; cut < 0; cut++)
                {
                    var curr = deck.Last.Value;
                    deck.AddFirst(curr);
                    deck.RemoveLast();
                }
            }
            else if( cut > 0)
            {
                for (; cut > 0; cut--)
                {
                    var curr = deck.First.Value;
                    deck.AddLast(curr);
                    deck.RemoveFirst();
                }
            }
        }
   
        public void ReverseList(ref LinkedList<long> deck)
        {
            var newList = new LinkedList<long>();
            var current = deck.First;
            while(current != null)
            {
                newList.AddFirst(current.Value);
                current = current.Next;
            }

            deck = newList;
        }

        public LinkedList<long> InitDeck(long size)
        {
            var deck = new LinkedList<long>();
            for(long i = 0; i< size; i++)
                deck.AddLast(i);

            return deck;
        }
        public override string SolveSecondPuzzle()
        {
            throw new NotImplementedException();
        }

        private List<(Technique, long)> ParseInput()
        {
            var input = ReadInputArray<string>();
            var instructions = new List<(Technique, long)>();
            foreach(var line in input)
            {
                if (line.StartsWith("cut"))
                {
                    var num = line.Split(' ').Last();
                    instructions.Add((Technique.Cut, long.Parse(num)));
                }
                else if(line.StartsWith("deal with increment"))
                {
                    var num = line.Split(' ').Last();
                    instructions.Add((Technique.Increment, long.Parse(num)));
                }
                else if(line.StartsWith("deal longo new stack"))
                {
                    instructions.Add((Technique.NewStack, 0));
                }
            }

            return instructions;
        }
        private enum Technique
        {
            Cut,
            NewStack,
            Increment
        }
    }
}
