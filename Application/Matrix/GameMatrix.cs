using System;
using System.Collections.Generic;
using System.Linq;

namespace TwoByTwo.Application.Matrix
{
    public readonly struct GameMatrix
    {
        public MatrixCell[][] Cells { get; }
        private readonly int _size;

        public GameMatrix(int[][] cells)
        {
            var size = cells.Length;
            Cells = new MatrixCell[size][];
            _size = size;
            
            for (var row = 0; row < size; row++)
            {
                Cells[row] = new MatrixCell[size];
                for (var col = 0; col < size; col++)
                {
                    Cells[row][col].Value = cells[row][col];
                }
            }
        }

        public GameMatrix(int size)
        {
            Cells = new MatrixCell[size][];
            _size = size;
            
            for (var row = 0; row < size; row++)
            {
                Cells[row] = new MatrixCell[size];
                for (var col = 0; col < size; col++)
                {
                    Cells[row][col] = new MatrixCell(0);
                }
            }
            
            GenerateNewValues();
            GenerateNewValues();
        }
        
        public bool HasEmptyCell()
        {
            for (var row = 0; row < _size; row++)
            {
                for (var col = 0; col < _size; col++)
                {
                    if (Cells[row][col].Value == 0)
                        return true;
                }
            }
            return false;
        }

        public bool HasTurns()
        {
            for (var row = 0; row < _size - 1; row++)
            {
                for (var col = 0; col < _size - 1; col++)
                {
                    if (Cells[row][col].Value == Cells[row][col + 1].Value
                        || Cells[row][col].Value == Cells[row + 1][col].Value)
                        return true;
                }
            }

            for (var col = 0; col < _size - 1; col++)
            {
                if (Cells[_size - 1][col].Value == Cells[_size - 1][col + 1].Value)
                    return true;
            }
            
            return false;
        }
        
        public void GenerateNewValues()
        {
            var random = new Random();
            
            var value = random.Next(8);
            value = value > 6 ? 4 : 2;

            var emptyCells = new List<(int, int)>();

            for (var i = 0; i < _size; i++)
            {
                for (var j = 0; j < _size; j++)
                {
                    if (Cells[i][j].Value == 0)
                    {
                        emptyCells.Add((i, j));
                    }
                }
            }
            
            var (row, col) = emptyCells[random.Next(emptyCells.Count - 1)];

            Cells[row][col] = new MatrixCell(value) {Animation = "init"};
        }
        
        public bool ToBottom()
        {
            var matrixChanged = false;
            
            for (var col = 0; col < _size; col++)
            {
                var items = new MatrixCell[_size];
                
                for (var row = _size - 1; row >= 0; row--)
                {
                    items[_size - 1 - row].Value = Cells[row][col].Value;
                }

                var newCol = ShiftRow(items);
                
                for (var i = 0; i < items.Length; i++)
                {
                    if (items[i].Value == newCol[i].Value) 
                        continue;
                    
                    matrixChanged = true;
                    break;
                }
                
                for (var row = _size - 1; row >= 0; row--)
                {
                    Cells[row][col] = newCol[_size - 1 - row];
                }
            }

            return matrixChanged;
        }

        public bool ToTop()
        {
            var matrixChanged = false;
            
            for (var col = 0; col < _size; col++)
            {
                var items = new MatrixCell[_size];
                
                for (var row = 0; row < _size; row++)
                {
                    items[row].Value = Cells[row][col].Value;
                }

                var newCol = ShiftRow(items);
                
                for (var i = 0; i < items.Length; i++)
                {
                    if (items[i].Value == newCol[i].Value) 
                        continue;
                    
                    matrixChanged = true;
                    break;
                }
                
                for (var row = 0; row < _size; row++)
                {
                    Cells[row][col] = newCol[row];
                }
            }

            return matrixChanged;
        }
        
        public bool ToRight()
        {
            var matrixChanged = false;
            
            for (var row = 0; row < _size; row++)
            {
                var items = Cells[row].Reverse().ToArray();
                var newRow = ShiftRow(items);
                
                for (var i = 0; i < items.Length; i++)
                {
                    if (items[i].Value == newRow[i].Value) 
                        continue;
                    
                    matrixChanged = true;
                    break;
                }
                
                Cells[row] = newRow.Reverse().ToArray();
            }
            
            return matrixChanged;
        }
        
        public bool ToLeft()
        {
            var matrixChanged = false;
            
            for (var row = 0; row < _size; row++)
            {
                var items = Cells[row];
                var newRow = ShiftRow(items);

                for (var i = 0; i < items.Length; i++)
                {
                    if (items[i].Value == newRow[i].Value) 
                        continue;
                    
                    matrixChanged = true;
                    break;
                }
                
                Cells[row] = newRow;
            }
            
            return matrixChanged;
        }
        
        private static MatrixCell[] ShiftRow(MatrixCell[] items)
        {
            var result = new MatrixCell[items.Length];
            
            var resultIndex = 0;

            for (var i = 0; i < items.Length; i++)
            {
                if (items[i].Value == 0) continue;
    
                var slag = 0;
                var index = i;
                    
                while (slag == 0)
                {
                    index++;
                    if (index == items.Length) break;
                    slag = items[index].Value;
                }
    
                if (items[i].Value == slag)
                {
                    result[resultIndex].Value = items[i].Value * 2;
                    result[resultIndex].Animation = "increment";
                    items[index].Value = 0;
                }
                else
                {
                    result[resultIndex].Value = items[i].Value;
                }
                    
                i = index - 1;
                resultIndex++;
            }

            return result;
        }

        public int GetScore()
        {
            var score = 0;

            for (var row = 0; row < _size; row++)
            {
                for (var col = 0; col < _size; col++)
                {
                    var val = Cells[row][col].Value;
                    var log = Math.Log2(val);
                    score += val * (int) log;
                }
            }

            return score;
        }
    }
}