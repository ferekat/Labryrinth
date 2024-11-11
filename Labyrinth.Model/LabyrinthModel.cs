using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.Drawing;
using System.Reflection.Emit;
using System.Text;
using Labyrinth.Persistence;

namespace Labyrinth.Model
{
    public class LabyrinthModel
    {
        #region Private Fields
        private int size;
        private Field[,] _gameTable;
        private Field[,]? _visibleTable;
        private int _x;
        private int _y;
        private int steps;
        private int startRow;
        private int startCol;
        private FileReader _file;
        private Dictionary<(int, int), List<(int, int)>> wallEffects = new()
        {
            { (-1, 0), new List<(int, int)> { (-2, -1), (-2, 0), (-2, 1) } },
            { (-1, 1), new List<(int, int)> { (-2, 1), (-2, 2), (-1, 2) } },
            { (0, 1), new List<(int, int)> { (-1, 2), (0, 2), (1, 2) } },
            { (1, 1), new List<(int, int)> { (1, 2), (2, 1), (2, 2) } },
            { (1, 0), new List<(int, int)> { (2, -1), (2, 0), (2, 1) } },
            { (1, -1), new List<(int, int)> { (1, -2), (2, -2), (2, -1) } },
            { (0, -1), new List<(int, int)> { (-1, -2), (0, -2), (1, -2) } },
            { (-1, -1), new List<(int, int)> { (-1, -2), (-2, -2), (-2, -1) } }
        };
        #endregion

        #region Public Properties
        public int Size { get { return size; } }
        public int Steps {  get { return steps; } }
        public Field[,] GameTable { get { return _gameTable; } }
        public Field[,] VisibleTable { get { return _visibleTable!; } }
        public int X { get { return _x; } }
        public int Y { get { return _y; } }
        public int StartRow { get { return startRow; } }
        public int StartCol { get { return startCol; } }
        public Dictionary<(int,int),List<(int,int)>> WallEffects { get { return wallEffects; } }
        #endregion

        #region Events
        public event EventHandler<GameWonEventArgs>? GameWon;
        public event EventHandler<FieldChangeEventArgs>? FieldChange;
        #endregion

        #region Constructor
        public LabyrinthModel(int size)
        {
            this.size = size;
            _gameTable = new Field[size, size];
            steps = 0;
            _file = new FileReader(size);

            NewGame();
        }
        #endregion

        #region Public Methods
        public void NewGame()
        {
            _gameTable = new Field[size, size]; 
            steps = 0;
            string[] fileContent = _file.ReadFile(size);

            if (fileContent != null)
            {
                for (int i = 0; i < size; i++)
                {
                    int[] numbers = fileContent[i].Split(' ').Select(int.Parse).ToArray();

                    for (int j = 0; j < size; j++)
                    {
                        switch (numbers[j])
                        {
                            case 0:
                                _gameTable[i, j] = Field.Grass;
                                break;
                            case 1:
                                _gameTable[i, j] = Field.Wall; 
                                break;
                            case 2:
                                _gameTable[i,j] = Field.Trophy; 
                                break;
                            case 3:
                                _gameTable[i, j] = Field.Character;
                                _x = i;
                                _y = j;
                                break;
                            default:
                                throw new FileLoadException("The file is corrupted!");
                        }
                    }
                }
            }
            else { throw new FileLoadException("The file is empty!"); }
            _visibleTable = SetSubMatrix();
        }

        public void Step(int x, int y)
        {
            switch (_gameTable[x, y])
            {
                case Field.Wall:
                    throw new ArgumentException("You cannot step there: It is a wall.");

                case Field.Grass:
                    if ((x == _x && Math.Abs(y - _y) == 1) || (y == _y && Math.Abs(x - _x) == 1))
                    {
                        _gameTable[_x, _y] = Field.Grass;
                        _visibleTable = SetSubMatrix();
                        OnFieldChanged(_x, _y, Field.Grass);
                        _x = x;
                        _y = y;
                        steps++;
                        _gameTable[_x, _y] = Field.Character;
                        _visibleTable = SetSubMatrix();
                        OnFieldChanged(_x, _y, Field.Character);
                        break;
                    }
                    else
                    {
                        throw new ArgumentException("You cannot step there: The field is not next to you");
                    }

                case Field.Trophy:
                    if ((x == _x && Math.Abs(y - _y) == 1) || (y == _y && Math.Abs(x - _x) == 1))
                    {
                        steps++;
                        _gameTable[x, y] = Field.Character;
                        _visibleTable = SetSubMatrix();
                        OnFieldChanged(x, y, Field.Character);
                        _gameTable[_x, _y] = Field.Trophy;
                        _visibleTable = SetSubMatrix();
                        OnFieldChanged(_x, _y, Field.Trophy);
                        _x = x;
                        _y = y;
                        GameOver();
                        break;
                    }
                    else
                    {
                        throw new ArgumentException("You cannot step there: The field is not next to you");
                    }

                case Field.Character:
                    throw new ArgumentException("You cannot step there: You are standing there!");

                default:
                    throw new Exception("Ivalid file content");
            }
        }
        public int SubMatrixSize(int coordinate)
        {
            if(coordinate == 0 || coordinate == size-1)
            {
                return 3;
            }
            else if(coordinate == 1 || coordinate == size - 2)
            {
                return 4;
            }
            else
            {
                return 5;
            }
        }
        public Field[,] SetSubMatrix()
        {
            int rows = SubMatrixSize(_x);
            int cols = SubMatrixSize(_y);
            Field[,] subMatrix = new Field[rows, cols];
            startRow = SetStartCoordinate(_x);
            startCol = SetStartCoordinate(_y);
            for(int i = 0; i < rows; i++)
            {
                for(int j = 0; j < cols; j++)
                {
                    subMatrix[i,j] = _gameTable[startRow+i,startCol+j];
                }
            }
            int characterX = _x - startRow;
            int characterY = _y - startCol;
            for (int i=  characterX- 2; i <= characterX + 2; i++)
            {
                for (int j= characterY - 2; j <= characterY + 2; j++)
                {
                    if (IsWall(i+startRow, j+startCol, _gameTable, size))
                    {
                        foreach (var wallEffect in wallEffects)
                        {
                            var wallPosition = wallEffect.Key;
                            var affectedCoordinates = wallEffect.Value;

                            if (characterX + wallPosition.Item1 == i && characterY + wallPosition.Item2 == j)
                            {
                                foreach (var coordinate in affectedCoordinates)
                                {
                                    int NeighborX = characterX + coordinate.Item1;
                                    int NeighborY = characterY + coordinate.Item2;
                                    HideCell(NeighborX, NeighborY, subMatrix, subMatrix.GetLength(0), subMatrix.GetLength(1));
                                }
                            }
                        }


                    }
                }
            }
            CheckDiagonalWalls(_x,_y,characterX, characterY,_gameTable, subMatrix, rows,cols,size);
            return subMatrix;
        }
        private bool IsWall(int x, int y, Field[,] table, int size)
        {
            return x >= 0 && x < size && y >= 0 && y < size && table[x, y] == Field.Wall;
        }
        private void CheckDiagonalWalls(int ogx, int ogy, int x, int y, Field[,] table, Field[,] subTable, int Rowsize, int Colsize, int size)
        {
            if (IsWall(ogx - 1, ogy, table, size) && IsWall(ogx, ogy - 1, table, size))
            {
                HideCell(x - 1, y - 1, subTable, Rowsize, Colsize);
                HideCell(x - 2, y - 2, subTable, Rowsize, Colsize);
            }
            if (IsWall(ogx - 1, ogy, table, size) && IsWall(ogx, ogy + 1, table, size))
            {
                HideCell(x - 1, y + 1, subTable, Rowsize, Colsize);
                HideCell(x - 2, y + 2, subTable, Rowsize, Colsize);
            }
            if (IsWall(ogx, ogy - 1, table, size) && IsWall(ogx + 1, ogy, table, size))
            {
                HideCell(x + 1, y - 1, subTable, Rowsize, Colsize);
                HideCell(x + 2, y - 2, subTable, Rowsize, Colsize);
            }
            if (IsWall(ogx, ogy + 1, table, size) && IsWall(ogx + 1, ogy, table, size))
            {
                HideCell(x + 1, y + 1, subTable, Rowsize, Colsize);
                HideCell(x + 2, y + 2, subTable, Rowsize, Colsize);
            }
        }
        private void HideCell(int x, int y, Field[,] table, int rowSize, int colSize)
        {
            if (x >= 0 && x < rowSize && y >= 0 && y < colSize)
            {
                table[x, y] = Field.Invisble;
                OnFieldChanged(x, y, Field.Invisble);
            }
        }
        private int SetStartCoordinate(int CharacterCoordinate)
        {
            for (int i = -2; i <= 0; i++)
            {
                int Neighbor = CharacterCoordinate + i;
                if (Neighbor >= 0)
                {
                    return Neighbor;
                }
            }
            return CharacterCoordinate;
        }
        #endregion

        #region Event Handlers
        private void GameOver()
        {
            GameWon?.Invoke(this, new GameWonEventArgs(steps));
        }

        private void OnFieldChanged(int x, int y, Field field)
        {
            if(FieldChange != null)
            {
                FieldChange(this, new FieldChangeEventArgs(x, y, field));
            }
        }
        #endregion
    }
}
