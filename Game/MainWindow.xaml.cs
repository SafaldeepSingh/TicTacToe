using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Game
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        String human;
        String comp;
        String currentPlayer;
        Label[,] board;
        private object move;
        private Label ai;
        Dictionary<String,sbyte> scores;

        public MainWindow()
        {
            InitializeComponent();
            this.human = "O";
            this.comp = "X";
            this.scores = new Dictionary<String, sbyte>();
            this.scores.Add("X", 1);
            this.scores.Add("O", -1);
            this.scores.Add("Tie", 0);

            this.board = new Label[,] {
                            { this.b11, this.b12, this.b13},
                            { this.b21, this.b22, this.b23 },
                            { this.b31, this.b32, this.b33 }
                        };
            this.CompMove();
            this.currentPlayer = human;
        }

        private void CompMove()
        {
            sbyte bestScore = SByte.MinValue;
            int[] move = new int[] { 0, 0 };
            for(int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if(this.board[i,j].Content == null)
                    {
                        this.board[i, j].Content = comp;
                        sbyte score = this.Minimax(0, false);
                        this.board[i, j].Content = null;
                        if (score > bestScore)
                        {
                            bestScore = score;
                            move = new int[]{ i, j };
                        }
                    }
                }

            }
            this.board[move[0], move[1]].Content = comp;
            this.currentPlayer = human;
            this.DisplayResult();
        }
        private String GetWinner()
        {
            String winner = null;
            for(int i = 0; i < 3; i++)
            {
                //horizontal
                if (equals3(this.board[i,0].Content, this.board[i,1].Content, this.board[i,2].Content))
                {
                    winner = this.board[i,0].Content.ToString();
                    break;
                }
                //vertical
                if (equals3(this.board[0, i].Content, this.board[1, i].Content, this.board[2, i].Content))
                {
                    winner = this.board[0, i].Content.ToString();
                    break;
                }

            }
            //diagonals
            if (equals3(this.board[0, 0].Content, this.board[1,1].Content, this.board[2,2].Content))
            {
                winner = this.board[0,0].Content.ToString();
            }
            if (equals3(this.board[2,0].Content, this.board[1,1].Content, this.board[0,2].Content))
            {
                winner = this.board[2,0].Content.ToString();
            }
            sbyte openSpots = 0;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (this.board[i,j].Content == null)
                    {
                        openSpots++;
                    }
                }
            }
            if (winner == null && openSpots == 0)
            {
                return "Tie";
            }
            else
            {
                return winner;
            }


        }
        private sbyte Minimax(int depth,bool isMaximizing)
        {
            String result = GetWinner();
            if (result != null)
            {
                return this.scores[result];
            }
            if (isMaximizing)
            {
                sbyte bestScore = SByte.MinValue;
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (this.board[i, j].Content == null)
                        {
                            this.board[i, j].Content = comp;
                            sbyte score = this.Minimax(depth + 1, false);
                            this.board[i, j].Content = null;
                            bestScore = Math.Max(score, bestScore);
                        }
                    }

                }
                return bestScore;
            }
            else
            {
                sbyte bestScore = SByte.MaxValue;
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (this.board[i, j].Content == null)
                        {
                            this.board[i, j].Content = human;
                            sbyte score = this.Minimax(depth+1, true);
                            this.board[i, j].Content = null;
                            bestScore = Math.Min(score, bestScore);
                        }
                    }

                }
                return bestScore;
            }
        }
        private bool equals3(Object p1, Object p2, Object p3)
        {
            //if (p1 == null || p2 == null || p3 == null)
            //    return false;
            //return p1.ToString().CompareTo(p2.ToString()) == 0 && p2.ToString().CompareTo(p3.ToString()) == 0;
            return p1 == p2 && p2 == p3 && p1 != null;
        }
        private bool DisplayResult() {
            String winner = GetWinner();
            if (winner != null)
            {
                String msg = (winner==human?"You":"Computer")+" win";
                this.lbl_result.Content = msg;
                return true;
            }
            return false;
        }
        private void b11_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (this.currentPlayer== human)
            {
                Label box = (Label)sender;

                if (box.Content == null)
                {
                    box.Content = human;
                    this.currentPlayer = comp;
                    if(!this.DisplayResult())
                        this.CompMove();

                }


            }
        }
    }
}
