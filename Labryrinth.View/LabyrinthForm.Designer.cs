namespace Labyrinth.View
{
    partial class LabyrinthForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            newToolStripMenuItem = new ToolStripMenuItem();
            textBox1 = new TextBox();
            comboBox1 = new ComboBox();
            strtBtn = new Button();
            table = new TableLayoutPanel();
            timer1 = new System.Windows.Forms.Timer(components);
            label1 = new Label();
            button1 = new Button();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.BackColor = SystemColors.GradientActiveCaption;
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(800, 28);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { newToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(46, 24);
            fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            newToolStripMenuItem.Name = "newToolStripMenuItem";
            newToolStripMenuItem.Size = new Size(165, 26);
            newToolStripMenuItem.Text = "New Game";
            newToolStripMenuItem.Click += newGameBtn_Click;
            // 
            // textBox1
            // 
            textBox1.BackColor = Color.WhiteSmoke;
            textBox1.BorderStyle = BorderStyle.None;
            textBox1.Font = new Font("Segoe UI", 11F);
            textBox1.Location = new Point(29, 42);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(152, 25);
            textBox1.TabIndex = 1;
            textBox1.Text = "Choose table size!";
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "5x5", "10x10", "15x15" });
            comboBox1.Location = new Point(29, 107);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(89, 28);
            comboBox1.TabIndex = 2;
            comboBox1.SelectedIndex = 0;
            // 
            // strtBtn
            // 
            strtBtn.Location = new Point(150, 107);
            strtBtn.Name = "strtBtn";
            strtBtn.Size = new Size(94, 29);
            strtBtn.TabIndex = 3;
            strtBtn.Text = "Start";
            strtBtn.UseVisualStyleBackColor = true;
            strtBtn.Click += startBtn_Click;
            // 
            // table
            // 
            table.ColumnCount = 1;
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            table.Location = new Point(317, 42);
            table.Name = "table";
            table.RowCount = 1;
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            table.Size = new Size(471, 387);
            table.TabIndex = 4;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 11F);
            label1.Location = new Point(42, 205);
            label1.Name = "label1";
            label1.Size = new Size(80, 25);
            label1.TabIndex = 5;
            label1.Text = "00:00:00";
            // 
            // button1
            // 
            button1.BackColor = Color.WhiteSmoke;
            button1.BackgroundImageLayout = ImageLayout.None;
            button1.FlatAppearance.BorderColor = Color.WhiteSmoke;
            button1.FlatStyle = FlatStyle.Flat;
            button1.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            button1.Location = new Point(42, 230);
            button1.Margin = new Padding(0);
            button1.Name = "button1";
            button1.Size = new Size(56, 61);
            button1.TabIndex = 6;
            button1.Text = "⏸️";
            button1.UseVisualStyleBackColor = false;
            button1.Click += pauseBtn_MouseClick;
            // 
            // LabyrinthForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(800, 450);
            Controls.Add(button1);
            Controls.Add(label1);
            Controls.Add(table);
            Controls.Add(strtBtn);
            Controls.Add(comboBox1);
            Controls.Add(textBox1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "LabyrinthForm";
            Text = "Form1";
            Load += GameLoad;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private TextBox textBox1;
        private ComboBox comboBox1;
        private Button strtBtn;
        private TableLayoutPanel table;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.Timer timer1;
        private Label label1;
        private Button button1;
    }
}
