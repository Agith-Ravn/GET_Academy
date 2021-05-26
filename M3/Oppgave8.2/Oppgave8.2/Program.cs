﻿using System;
using System.Linq;
using System.Threading;
using Oppgave8._2.TrePåRad;

namespace Oppgave8._2
{
    class Program
    {
        static void Main(string[] args)
        {
            //Lager objekter av class? 
            var boardModel = new BoardModel();

            while (true)
            {
                BoardView.Show(boardModel);
                Console.Write("Skriv inn hvor du vil sette kryss (f.eks. \"a2\"): ");
                var position = Console.ReadLine();
                boardModel.SetCross(position);
                BoardView.Show(boardModel);

                Thread.Sleep(2000);
                boardModel.SetRandomCircle();
            }
        }
    }

    class BoardModel
    {
        //Deklarert tomme variabler?
        public bool IsGameStopped { get; private set; } //Bool
        public CellContent[] Content { get; private set; }
        private readonly Random _random = new Random();
        private Combination[] _winningCombinations;

        public BoardModel()
        {
            Content = new CellContent[9];
            _winningCombinations = new[]
            {
                new Combination(Content, 0, 1, 2),
                new Combination(Content, 3, 4, 5),
                new Combination(Content, 6, 7, 8),
                new Combination(Content, 0, 3, 6),
                new Combination(Content, 1, 4, 7),
                new Combination(Content, 2, 5, 8),
                new Combination(Content, 0, 4, 8),
                new Combination(Content, 2, 4, 6),
            };
        }

        public CellContent IsWinning()
        {
            return _winningCombinations.Select(c => c.IsWinning()).FirstOrDefault(x => x != CellContent.None);
        }

        public void SetCross(string positionStr)
        {
            var col = positionStr[0] == 'a' ? 0 : positionStr[0] == 'b' ? 1 : 2;
            var row = Convert.ToInt32(positionStr[1].ToString()) - 1;
            var position = row * 3 + col;
            Content[position] = CellContent.Cross;
        }

        public void SetRandomCircle()
        {
            var randomIndex = _random.Next(0, 8);
            while (Content[randomIndex] != CellContent.None)
            {
                randomIndex = _random.Next(0, 8);
            }
            Content[randomIndex] = CellContent.Circle;
        }
    }

    class BoardView
    {
        public static void Show(BoardModel boardModel)
        {
            Console.Clear();
            var winningSymbol = boardModel.IsWinning();
            var content = boardModel.Content;
            Console.WriteLine("  a b c");
            Console.WriteLine(" ┌─────┐");
            ShowOneLine(0, content);
            ShowOneLine(3, content);
            ShowOneLine(6, content);
            Console.WriteLine(" └─────┘");
            if (winningSymbol != CellContent.None)
            {
                var symbol = winningSymbol == CellContent.Circle ? "Datamaskinen" : "Du";
                Console.WriteLine("\n" + symbol + " har vunnet!");
                Environment.Exit(0);
            };

        }

        private static void ShowOneLine(int startIndex, CellContent[] content)
        {
            var lineNo = 1 + startIndex / 3;
            Console.Write(lineNo + "│");
            for (var i = startIndex; i < startIndex + 3; i++)
            {
                if (i > startIndex) Console.Write(' ');
                //if(content[i]==CellContent.None)
                //    Console.Write(" ");
                //else if (content[i] == CellContent.Cross)
                //    Console.Write("×");
                //else
                //    Console.Write("o");

                //char c;
                //if (content[i] == CellContent.None)
                //    c = ' ';
                //else if (content[i] == CellContent.Cross)
                //    c = '×';
                //else
                //    c = 'o';

                var isBlank = content[i] == CellContent.None;
                var isCross = content[i] == CellContent.Cross;
                Console.Write(isBlank ? ' ' : isCross ? '×' : 'o');
            }
            Console.WriteLine("│");
        }
    }

    class Combination
    {
        private readonly int _index1;
        private readonly int _index2;
        private readonly int _index3;
        private readonly CellContent[] _cells;

        public Combination(CellContent[] cells, int index1, int index2, int index3)
        {
            _cells = cells;
            _index3 = index3;
            _index2 = index2;
            _index1 = index1;
        }

        public CellContent IsWinning()
        {
            if (_cells[_index1] == _cells[_index2]
                && _cells[_index1] == _cells[_index3])
            {
                return _cells[_index1];
            }
            return CellContent.None;
        }
    }


    namespace TrePåRad
    {
        enum CellContent
        {
            None = 0, // Default
            Cross,
            Circle
        }
    }
}

