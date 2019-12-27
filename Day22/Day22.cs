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
            long size = 10007;
            long resultIdx = 2019;

            resultIdx = ShuffleDeck(instructions, size, resultIdx);

            return resultIdx.ToString();
        }

        public override string SolveSecondPuzzle()
        {
            var instructions = ParseInput();
            long size = 119315717514047;
            long resultIdx = 2020;

            HashSet<long> idxs = new HashSet<long>();
            for(long i = 0; i < 100000; i++) {

                resultIdx = ShuffleDeck(instructions, size, resultIdx);
                if (idxs.Contains(resultIdx))
                    Console.WriteLine(i + " " + resultIdx);
                else
                    idxs.Add(resultIdx);
            }

            return resultIdx.ToString();
        }

        private long ShuffleDeck(List<(Technique, long)> instructions, long size, long resultIdx)
        {
            foreach (var instruction in instructions)
            {
                switch (instruction.Item1)
                {
                    case Technique.NewStack:
                        ReverseList(size, ref resultIdx);
                        break;
                    case Technique.Cut:
                        Cut(instruction.Item2, size, ref resultIdx);
                        break;
                    case Technique.Increment:
                        Increment(instruction.Item2, size, ref resultIdx);
                        break;
                    default: throw new Exception();
                }
            }

            return resultIdx;
        }

        public void Increment(long step, long size, ref long cardIdx)
        {
            cardIdx = (cardIdx * step) % size;
        }

        public void Cut(long cut, long size, ref long cardIdx)
        { 
            cardIdx = cardIdx - cut;
            if (cardIdx < 0)
                cardIdx = size + cardIdx;
            if (cardIdx >= size)
                cardIdx = cardIdx - size;
            
        }

        public void ReverseList(long deckSize, ref long cardIdx )
        {
            cardIdx = deckSize - 1 - cardIdx;
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
                else if(line.StartsWith("deal into new stack"))
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
