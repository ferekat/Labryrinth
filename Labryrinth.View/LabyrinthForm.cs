using Labyrinth.Model;
using Labyrinth.Persistence;
using System.Drawing;
using System.Security.Policy;
using System.Text.RegularExpressions;
using System;
using System.Diagnostics;

namespace Labyrinth.View
{
    /// <summary>
    /// Labirintus ablak típusa
    /// </summary>
    public partial class LabyrinthForm : Form
    {
        #region Private Fields
        private System.Windows.Forms.Timer timer;
        private TimeSpan elapsedTime;
        private TimeSpan paused_time;
        private bool isPaused;
        private Button[,] _buttonGrid;
        private LabyrinthModel _model;
        #endregion

        #region Consturctor
        /// <summary>
        /// Labirintus ablak létrehozása.
        /// </summary>
        public LabyrinthForm()
        {
            InitializeComponent();
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000;
            timer.Tick += Timer_Tick!;
            elapsedTime = TimeSpan.Zero;

            Label timeLabel = new Label()
            {
                Location = new System.Drawing.Point(30, 170),
                Size = new System.Drawing.Size(200, 30),
                Font = new System.Drawing.Font("Arial", 14),
                Text = "Elapsed time:"
            };
            this.Controls.Add(timeLabel);

            _buttonGrid = null!;
            _model = new LabyrinthModel(5);
            _model.FieldChange += new EventHandler<FieldChangeEventArgs>(Model_FieldChange);
            _model.GameWon += new EventHandler<GameWonEventArgs>(Model_GameWon);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Új játék kezdése.
        /// </summary>
        private void NewGame()
        {
            table.ColumnCount = 0;
            table.RowCount = 0;
            table.Visible = false;
            strtBtn.Enabled = true;
            comboBox1.Enabled = true;
            elapsedTime = TimeSpan.Zero;
            label1.Text = $"{elapsedTime:hh\\:mm\\:ss}";
            button1.Enabled = false;
            button1.Text = "⏸️";
            isPaused = false;
            timer.Stop();
        }
        /// <summary>
        /// Tábla létrehozása.
        /// </summary>
        /// <param name="size">A játéktábla mérete.</param>
        private void CreateTable(int size)
        {
            table.Controls.Clear();
            table.Visible = true;
            table.RowCount = table.ColumnCount = size;
            table.Width = Width / 2;
            table.Height = table.Width - 40;
            SizeChanged += LabyrinthForm_SizeChanged;

            _buttonGrid = new Button[size, size];
            int buttonWidth = table.Width / size;
            int buttonHeight = table.Height / size;

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    _buttonGrid[i, j] = new GridButton(i, j)
                    {
                        Location = new Point(buttonWidth * i, buttonHeight * j),
                        Size = new Size(buttonWidth, buttonHeight),
                        Font = new Font(FontFamily.GenericSansSerif, Height / 10, FontStyle.Bold),
                        Dock = DockStyle.Fill,
                        BackColor = Color.Black,
                        BackgroundImageLayout = ImageLayout.Stretch
                    };
                    table.Controls.Add(_buttonGrid[i, j], j, i);
                }
            }

            SetVisibleFields(size, _buttonGrid, _model.VisibleTable);
            table.RowStyles.Clear();
            table.ColumnStyles.Clear();
            for (int i = 0; i < size; i++)
            {
                table.RowStyles.Add(new RowStyle(SizeType.Percent, 100F / size));
            }
            for (int j = 0; j < size; j++)
            {
                table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F / size));
            }
        }
        /// <summary>
        /// A látható gombok megjelenítése.
        /// </summary>
        /// <param name="size">A játéktábla mérete.</param>
        /// <param name="buttonGrid">A gombrács.</param>
        /// <param name="table">A játéktábla.</param>
        private void SetVisibleFields(int size, Button[,] buttonGrid, Field[,] table)
        {
            for(int i = 0; i< table.GetLength(0); i++)
            {
                for(int j = 0;j< table.GetLength(1); j++)
                {
                    switch(table[i,j])
                    {
                        case Field.Character:
                            buttonGrid[_model.StartRow+i, _model.StartCol+j].BackgroundImage = Labyrinth.View.Resource1.character;
                            break;
                        case Field.Grass:
                            buttonGrid[_model.StartRow + i, _model.StartCol + j].BackgroundImage = Labyrinth.View.Resource1.grass;
                            break;
                        case Field.Wall:
                            buttonGrid[_model.StartRow + i, _model.StartCol + j].BackgroundImage = Labyrinth.View.Resource1.wall;
                            break;
                        case Field.Trophy:
                            buttonGrid[_model.StartRow + i, _model.StartCol + j].BackgroundImage = Labyrinth.View.Resource1.trophy;
                            break;
                        case Field.Invisble:
                            buttonGrid[_model.StartRow + i, _model.StartCol + j].BackgroundImage = null;
                            buttonGrid[_model.StartRow + i, _model.StartCol + j].BackColor = Color.Black;
                            break;

                    }
                    buttonGrid[_model.StartRow + i, _model.StartCol + j].BackgroundImageLayout = ImageLayout.Stretch;
                }
            }
        }
        #endregion

        #region Form Event Handlers
        /// <summary>
        /// Ablak betöltésének eseménykezelése.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GameLoad(object? sender, EventArgs e)
        {
            timer.Stop();
            button1.Enabled = false;
            _model.NewGame();
        }
        /// <summary>
        /// Ablakméret megváltozásának eseménykezelése.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LabyrinthForm_SizeChanged(object? sender, EventArgs e)
        {
            table.Width = Width / 2;
            table.Height = table.Width - 40;
        }
        /// <summary>
        /// Másodpercek változásának eseménykezelése.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Tick(object sender, EventArgs e)
        {
            elapsedTime = elapsedTime.Add(TimeSpan.FromSeconds(1));
            label1.Text = $"{elapsedTime:hh\\:mm\\:ss}";
        }
        #endregion

        #region Button Event Handlers
        /// <summary>
        /// Szünet gomb eseménykezelése.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pauseBtn_MouseClick(object? sender, EventArgs e)
        {
            if (!isPaused)
            {
                paused_time = elapsedTime;
                timer.Stop();
                label1.Text = $"{paused_time:hh\\:mm\\:ss}";
                isPaused = true;
                button1.Text = "▶️";
                foreach(Button b in _buttonGrid)
                {
                    b.Enabled = false;
                }
            }
            else
            {
                elapsedTime.Add(paused_time);
                timer.Start();
                isPaused = false;
                button1.Text = "⏸️";
                foreach (Button b in _buttonGrid)
                {
                    b.Enabled = true;
                }
            }

        }
        /// <summary>
        /// Start gomb eseménykezelése.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void startBtn_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null)
            {
                return;
            }
            string size = comboBox1.SelectedItem.ToString()!;
            int gridSize = int.Parse(size.Split('x')[0]);

            _model = new LabyrinthModel(gridSize);
            _model.NewGame();
            CreateTable(gridSize);
            button1.Enabled = true;
            strtBtn.Enabled = false;
            comboBox1.Enabled = false;
            _model.GameWon += new EventHandler<GameWonEventArgs>(Model_GameWon);
            _model.FieldChange += new EventHandler<FieldChangeEventArgs>(Model_FieldChange);

            for (int i = 0; i < _model.Size; i++)
            {
                for (int j = 0; j < _model.Size; j++)
                {
                    _buttonGrid[i, j].MouseClick += buttonGrid_MouseClick;
                }
            }
            timer.Start();
        }
        /// <summary>
        /// Új játék gomb eseménykezelése.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newGameBtn_Click(object sender, EventArgs e)
        {
            NewGame();
        }
        #endregion

        #region Button Grid Event Handlers
        /// <summary>
        /// Gombrács eseménykezelése.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonGrid_MouseClick(object? sender, EventArgs e)
        {
            if (sender is GridButton button)
            {
                try
                {
                    _model.Step(button._x, button._y);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
            }
        }
        #endregion
        
        #region Model Event Handlers
        /// <summary>
        /// Játék végének eseménykezelése.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Model_GameWon(object? sender, GameWonEventArgs e)
        {
            elapsedTime = elapsedTime.Add(TimeSpan.FromSeconds(0));
            timer.Stop();
            MessageBox.Show($"You have won in {elapsedTime} seconds!");
            _model.NewGame();
            NewGame();
        }
        /// <summary>
        /// Modell mezőváltozásának eseménykezelése.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Model_FieldChange(object? sender, FieldChangeEventArgs e)
        {
            switch (e.Field)
            {
                case Field.Character:
                    for (int i = 0; i < _model.Size; i++)
                    {
                        for (int j = 0; j < _model.Size; j++)
                        {
                            _buttonGrid[i, j].BackgroundImage = null;
                            _buttonGrid[i, j].BackColor = Color.Black;
                        }
                    }
                    SetVisibleFields(_model.Size, _buttonGrid, _model.VisibleTable);
                    break;
                case Field.Grass:
                    _buttonGrid[e.X, e.Y].BackgroundImage = Labyrinth.View.Resource1.grass;
                    break;
                case Field.Trophy:
                    _buttonGrid[e.X, e.Y].BackgroundImage = Labyrinth.View.Resource1.grass;
                    break;
            }
        }
        #endregion
    }
}
