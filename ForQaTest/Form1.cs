using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;

namespace ForQaTest
{
    public partial class Form1 : Form
    {
        private Game game;
        private PictureBox[,] pbs = new PictureBox[3, 3];
        private Image Cross;
        private Image Nought;

        public Form1()
        {
            InitializeComponent();

            Init();

            game = new Game();
            Build(game);
        }

        void Init()
        {
            Cross = Image.FromStream(new WebClient().OpenRead("https://cdn.onlinewebfonts.com/svg/download_26540.png"));
            Nought = Image.FromStream(new WebClient().OpenRead("https://cdn.onlinewebfonts.com/svg/download_441992.png"));

            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                {
                    pbs[i, j] = new PictureBox { Parent = this, Size = new Size(100, 100), Top = i * 100, Left = j * 100, BorderStyle = BorderStyle.FixedSingle, Tag = new Point(i, j), Cursor = Cursors.Hand, SizeMode = PictureBoxSizeMode.StretchImage };
                    pbs[i, j].Click += pb_Click;
                }
            new Button { Parent = this, Top = 320, Text = "Сначала" }.Click += delegate { game = new Game(); Build(game); };
}
        private void Build(Game game)
        {
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    pbs[i, j].Image = game.Items[i, j] == FieldState.Cross ? Cross : (game.Items[i, j] == FieldState.Nought ? Nought : null);
        }
        void pb_Click(object sender, EventArgs e)
        {
            game.MakeMove((Point)(sender as Control).Tag);
            Build(game);

            if (game.Winned)
                MessageBox.Show(string.Format("{0} победил!", game.CurrentPlayer == 0 ? "Крестик" : "Кружок"));
        }
    }

    class Game
    {
        public FieldState[,] Items = new FieldState[3, 3];
        public int CurrentPlayer = 0;
        public bool Winned;

        public void MakeMove(Point p)
        {
            if (Items[p.X, p.Y] != FieldState.Empty)
                return;

            if (Winned)
                return;

            Items[p.X, p.Y] = CurrentPlayer == 0 ? FieldState.Cross : FieldState.Nought;
            if (CheckWinner(FieldState.Cross) || CheckWinner(FieldState.Nought))
            {
                Winned = true;
                return;
            }

            CurrentPlayer ^= 1;
        }

        private bool CheckWinner(FieldState state)
        {
            for (int i = 0; i < 3; i++)
            {
                if (Items[i, 0] == state && Items[i, 1] == state && Items[i, 2] == state)
                    return true;
                if (Items[0, i] == state && Items[1, i] == state && Items[2, i] == state)
                    return true;
            }

            if (Items[0, 0] == state && Items[1, 1] == state && Items[2, 2] == state)
                return true;

            if (Items[0, 2] == state && Items[1, 1] == state && Items[2, 0] == state)
                return true;

            return false;
        }
    }

    enum FieldState
    {
        Empty,
        Cross,
        Nought
    }
}


