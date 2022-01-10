using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Minesweeper
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        private List<Tile> tileArray = new List<Tile>();
        private List<Button> buttonArray = new List<Button>();
        private SolidColorBrush lGray = new SolidColorBrush(Colors.LightGray);
        private SolidColorBrush lSGray = new SolidColorBrush(Colors.LightSlateGray);




        private int rows;
        private int columns;
        private int mineTotal = 0;
        private int mineTotalOrignal = 0;
        private string gameState { get; set; }
        private bool gameOn = true;


        public MainPage() => this.InitializeComponent();

        private void easyFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            restartGame();
            rows = 9;
            columns = 9;
            mineTotal = 10;
            mineTotalOrignal = 10;
            tileCreator();
            gridCreator();
        }
        private void mediumFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            restartGame();
            rows = 16;
            columns = 16;
            mineTotal = 40;
            mineTotalOrignal = 40;
            tileCreator();
            gridCreator();
        }

        private void hardFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            restartGame();
            rows = 16;
            columns = 30;
            mineTotal = 99;
            mineTotalOrignal = 99;
            tileCreator();
            gridCreator();
        }

        private void restartGame()
        {
            statusTextBlock.Text = "";
            gameOn = true;
            if (tileArray.Count() != 0)
            {
                tileArray.Clear();
            }
            if (buttonArray.Count() != 0)
            {
                buttonArray.Clear();
            }
            if (gameGrid.ColumnDefinitions.Count() != 0 && (gameGrid.RowDefinitions.Count() != 0))
            {
                gameGrid.Children.Clear();
            }
        }

        #region Tile Creator
        private void tileCreator()
        {
            populateTileList();
            createTileMines();
            createTileSurroundings();
        }

        private void populateTileList()
        {
            Tile t;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    t = new Tile(i, j);
                    tileArray.Add(t);
                }
            }
        }

        private void createTileMines()
        {
            Random rng = new Random();
            int tempMineCounter = mineTotal;

            while (tileArray.FindAll(x => x.IsMine).Count() < mineTotal)
            {
                int rowRNG = rng.Next(rows);
                int columnRNG = rng.Next(columns);

                if (!tileArray.Find(x => x.Row == rowRNG && x.Column == columnRNG).IsMine)
                {
                    tileArray.Find(x => x.Row == rowRNG && x.Column == columnRNG).IsMine = true;
                }
                tempMineCounter--;
            }
        }

        private void createTileSurroundings()
        {
            Tile t;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    t = tileArray.Find(x => x.Row == i && x.Column == j);

                    //Bellow of Cell - 1
                    if (i + 1 < rows)
                    {
                        t.Surroundings.Add(tileArray.Find(x => x.Row == (i + 1) && x.Column == j));
                    }
                    //Above of Cell - 2
                    if (i - 1 >= 0)
                    {
                        t.Surroundings.Add(tileArray.Find(x => x.Row == (i - 1) && x.Column == j));
                    }
                    //Right of Cell - 3
                    if (j + 1 < columns)
                    {
                        t.Surroundings.Add(tileArray.Find(x => x.Row == i && x.Column == (j + 1)));
                    }
                    //Left of Cell - 4
                    if (j - 1 >= 0)
                    {
                        t.Surroundings.Add(tileArray.Find(x => x.Row == i && x.Column == (j - 1)));
                    }
                    //Bottom Right Duagonal of Cell - 5
                    if (i + 1 < rows && j + 1 < columns)
                    {
                        t.Surroundings.Add(tileArray.Find(x => x.Row == (i + 1) && x.Column == (j + 1)));
                    }
                    //Top Right Digaonal of Cell - 6
                    if (i - 1 >= 0 && j + 1 < columns)
                    {
                        t.Surroundings.Add(tileArray.Find(x => x.Row == (i - 1) && x.Column == (j + 1)));
                    }
                    //Bottom Left Diagonal of Cell - 7
                    if (i + 1 < rows && j - 1 >= 0)
                    {
                        t.Surroundings.Add(tileArray.Find(x => x.Row == (i + 1) && x.Column == (j - 1)));
                    }
                    //Top Left Diagonal of Cell - 8
                    if (i - 1 >= 0 && j - 1 >= 0)
                    {
                        t.Surroundings.Add(tileArray.Find(x => x.Row == (i - 1) && x.Column == (j - 1)));
                    }
                }
            }
        }
        #endregion

        #region Grid Creator
        private void gridCreator()
        {
            mineCounterTextBlock.Text = $"Mine Counter: {mineTotal}";

            RowDefinition rowDef;
            ColumnDefinition colDef;

            for (int r = 0; r < rows; r++)
            {
                rowDef = new RowDefinition();
                gameGrid.RowDefinitions.Add(rowDef);
            }
            for (int c = 0; c < columns; c++)
            {
                colDef = new ColumnDefinition();
                gameGrid.ColumnDefinitions.Add(colDef);
            }

            for (int i = 0; i < (rows); i++)
            {
                for (int j = 0; j < (columns); j++)
                {
                    Button b = new Button();
                    b.Tapped += B_Tapped;
                    b.RightTapped += B_RightTapped;
                    buttonArray.Add(b);
                    gameGrid.Children.Add(b);
                    Grid.SetColumn(b, j);
                    Grid.SetRow(b, i);
                }
            }

            //Show Mines
            //    foreach (Button item in buttonArray)
            //    {
            //        if (tileArray.ElementAt(buttonArray.IndexOf(item)).IsMine)
            //        {
            //            item.Background = new SolidColorBrush(Colors.Red);
            //        }
            //    }
            //}
        }
        #endregion

        #region Events
        private void B_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            Button b = (Button)sender;
            int index = buttonArray.IndexOf(b);
            Tile t = tileArray.ElementAt(index);

            if (gameOn && !(t.IsClicked))
            {
                if (!(t.IsFlagged) && !(t.IsAmbiguous))
                {
                    b.Content = "F";
                    t.IsFlagged = true;
                    mineTotal--;
                    mineCounterTextBlock.Text = $"Mine Counter: {mineTotal}";
                }
                else if (t.IsFlagged && !(t.IsAmbiguous))
                {
                    b.Content = "?";
                    t.IsFlagged = false;
                    t.IsFlagged = false;
                    t.IsAmbiguous = true;
                    mineTotal++;
                    mineCounterTextBlock.Text = $"Mine Counter: {mineTotal}";
                }
                else
                {
                    b.Content = "";
                    //b.Background = lSGray;
                    t.IsFlagged = false;
                    t.IsAmbiguous = false;
                }

                winCheck();
            }
            if (t.IsClicked && t.Surroundings.Count(x => x.IsAmbiguous) == 0 && t.MinesAround != 0 && !t.IsMine && t.Surroundings.Count(x => x.IsFlagged) == t.MinesAround && gameOn)
            {
                foreach (Tile item in t.Surroundings)
                {
                    if (item.MinesAround != 0 && !item.IsFlagged)
                    {
                        buttonArray.ElementAt(tileArray.IndexOf(item)).Background = lGray;
                        buttonArray.ElementAt(tileArray.IndexOf(item)).Content = $"{item.MinesAround}";
                        item.IsClicked = true;
                    }
                    else if (item.IsFlagged)
                    {
                        buttonArray.ElementAt(tileArray.IndexOf(item)).Background = lSGray;
                    }
                    else
                    {
                        buttonArray.ElementAt(tileArray.IndexOf(item)).Background = lGray;
                        item.IsClicked = true;
                    }
                }
            }
            else if (t.IsClicked && t.Surroundings.Count(x => x.IsAmbiguous) == 0 && t.MinesAround != 0 && !t.IsMine && t.Surroundings.Count(x => x.IsMine) != 0)
            {
                foreach (Tile item in t.Surroundings)
                {
                    if (item.IsMine)
                    {
                        mineClick(buttonArray.ElementAt(tileArray.IndexOf(item)));
                    }
                }
            }

        }

        private void B_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Button b = (Button)sender;
            int index = buttonArray.IndexOf(b);
            Tile t = tileArray.ElementAt(index);

            if (gameOn && !(t.IsFlagged))
            {
                if (!(t.IsClicked) && !(t.IsMine))
                {
                    nonMineClick((Button)sender);
                }
                else if (!(t.IsClicked) && t.IsMine)
                {
                    mineClick((Button)sender);
                }

            }
        }
        #endregion

        public void floodFill(Tile t)
        {
            if (t.Surroundings.Any(x => x.MinesAround == 0))
            {
                foreach (Tile item in t.Surroundings)
                {
                    if (item.Row >= 0 && t.Row < rows && t.Column >= 0 && t.Column < columns && !(tileArray.ElementAt(tileArray.IndexOf(item)).IsClicked))
                    {

                        if (item.MinesAround == 0)
                        {
                            buttonArray.ElementAt(tileArray.IndexOf(item)).Background = lGray;
                            tileArray.ElementAt(tileArray.IndexOf(item)).IsClicked = true;
                            floodFill(item);
                        }
                        else if (item.MinesAround > 0)
                        {
                            buttonArray.ElementAt(tileArray.IndexOf(item)).Background = lGray;
                            tileArray.ElementAt(tileArray.IndexOf(item)).IsClicked = true;
                            buttonArray.ElementAt(tileArray.IndexOf(item)).Content = $"{item.MinesAround}";
                        }
                    }
                }
            }
        }

        private void winCheck()
        {
            if (tileArray.FindAll(x => x.IsClicked && !x.IsMine).Count() == (tileArray.Count()) - mineTotalOrignal)
            {
                gameState = "Game Won";
                statusTextBlock.Text = gameState;
                gameOn = false;
            }
        }

        private void nonMineClick(Button b)
        {
            int index = buttonArray.IndexOf(b);

            b.Background = lGray;
            if (tileArray.ElementAt(index).MinesAround != 0)
            {
                buttonArray.ElementAt(index).Content = $"{tileArray.ElementAt(index).MinesAround}";
                tileArray.ElementAt(index).IsClicked = true;
            }
            else if (tileArray.ElementAt(index).MinesAround == 0)
            {
                floodFill(tileArray.ElementAt(index));
            }
            tileArray.ElementAt(index).IsClicked = true;

            winCheck();
        }

        private void mineClick(Button b)
        {
            for (int i = 0; i < tileArray.Count(); i++)
            {
                if (tileArray.ElementAt(i).IsMine)
                {
                    buttonArray.ElementAt(i).Background = new SolidColorBrush(Colors.Black);
                }
                else if (tileArray.ElementAt(i).MinesAround != 0 && !(tileArray.ElementAt(i).IsMine))
                {
                    buttonArray.ElementAt(i).Background = lGray;
                    buttonArray.ElementAt(i).Content = $"{tileArray.ElementAt(i).MinesAround}";
                }
                else
                {
                    buttonArray.ElementAt(i).Background = lGray;
                }
            }

            gameState = "Game Over";
            statusTextBlock.Text = gameState;
            gameOn = false;
        }
    }
}
