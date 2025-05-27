using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SolitaireKlondike
{
    public partial class MainWindow : Window
    {
        private List<Card> deck;
        private readonly List<StackPanel> tableaus = new();
        private readonly List<StackPanel> foundations = new();
        private Stack<Card> stock = new();
        private Stack<Card> waste = new();
        private Border stockPlaceholder, wastePlaceholder;
        private Point dragStartPoint;
        private Border draggedCard;
        private StackPanel sourceStack;
        private Point mouseOffset;
        private List<Border> draggedCards = new();

        public MainWindow()
        {
            InitializeComponent();
            InitGame();
        }

        private void InitGame()
        {
            GameCanvas.Children.Clear();
            CreateButtons();
            InitPiles();
            CreateDeck();
            ShuffleDeck();
            DealCards();
        }

        private void CreateButtons()
        {
            Button restart = new() { Content = "Начать заново", Width = 120, Height = 40 };
            restart.Click += (s, e) => InitGame();
            Canvas.SetLeft(restart, 20);
            Canvas.SetTop(restart, 20);
            GameCanvas.Children.Add(restart);

        }

        private void InitPiles()
        {
            tableaus.Clear();
            foundations.Clear();

            for (int i = 0; i < 7; i++)
            {
                StackPanel sp = new() { Orientation = Orientation.Vertical, Margin = new Thickness(0) };
                Canvas.SetLeft(sp, 50 + i * 150);
                Canvas.SetTop(sp, 250);
                GameCanvas.Children.Add(sp);
                tableaus.Add(sp);
            }

            for (int i = 0; i < 4; i++)
            {
                StackPanel sp = new() { Orientation = Orientation.Vertical, Margin = new Thickness(0) };
                Canvas.SetLeft(sp, 700 + i * 100);
                Canvas.SetTop(sp, 100);
                GameCanvas.Children.Add(sp);
                foundations.Add(sp);
            }

            stockPlaceholder = CreatePlaceholder();
            Canvas.SetLeft(stockPlaceholder, 20);
            Canvas.SetTop(stockPlaceholder, 100);
            stockPlaceholder.MouseLeftButtonDown += StockPlaceholder_MouseLeftButtonDown;
            GameCanvas.Children.Add(stockPlaceholder);

            wastePlaceholder = CreatePlaceholder();
            Canvas.SetLeft(wastePlaceholder, 120);
            Canvas.SetTop(wastePlaceholder, 100);
            GameCanvas.Children.Add(wastePlaceholder);
        }

        private Border CreatePlaceholder()
        {
            return new Border
            {
                Width = 80,
                Height = 100,
                CornerRadius = new CornerRadius(5),
                BorderBrush = Brushes.White,
                BorderThickness = new Thickness(1),
                Background = Brushes.Transparent
            };
        }

        private void CreateDeck()
        {
            deck = new();
            foreach (CardSuit suit in Enum.GetValues(typeof(CardSuit)))
                for (int rank = 1; rank <= 13; rank++)
                    deck.Add(new Card(suit, rank));
        }

        private void ShuffleDeck()
        {
            Random rng = new();
            deck = deck.OrderBy(_ => rng.Next()).ToList();
            stock = new Stack<Card>(deck);
        }

        private void DealCards()
        {
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j <= i; j++)
                {
                    Card card = stock.Pop();
                    card.IsFaceUp = (j == i);
                    var visual = CreateCardVisual(card);
                    tableaus[i].Children.Add(visual);
                }
            }
            UpdateStockVisual();
        }

        private void UpdateStockVisual()
        {
            wastePlaceholder.Child = null;
            if (waste.Count > 0)
                wastePlaceholder.Child = CreateCardVisual(waste.Peek());

            stockPlaceholder.Background = stock.Count > 0 ? Brushes.White : Brushes.Transparent;
        }

        private void StockPlaceholder_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (stock.Count > 0)
            {
                var card = stock.Pop();
                card.IsFaceUp = true;
                waste.Push(card);
                UpdateStockVisual();
            }
            else
            {
                while (waste.Count > 0)
                {
                    var card = waste.Pop();
                    card.IsFaceUp = false;
                    stock.Push(card);
                }
                UpdateStockVisual();
            }
        }

        private Border CreateCardVisual(Card card)
        {
            Border border = new()
            {
                Width = 80,
                Height = 100,
                CornerRadius = new CornerRadius(5),
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(1),
                Background = card.IsFaceUp ? GetCardColor(card.Suit) : Brushes.Gray,
                Tag = card,
                Margin = new Thickness(0, -60, 0, 0)
            };

            if (card.IsFaceUp)
            {
                TextBlock text = new()
                {
                    Text = GetCardText(card),
                    Margin = new Thickness(5),
                    FontWeight = FontWeights.Bold,
                    Foreground = Brushes.Black
                };
                border.Child = text;
                border.MouseLeftButtonDown += Card_MouseLeftButtonDown;
                border.MouseMove += Card_MouseMove;
                border.MouseLeftButtonUp += Card_MouseLeftButtonUp;
            }

            return border;
        }

        private void Card_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            draggedCard = sender as Border;
            dragStartPoint = e.GetPosition(GameCanvas);
            sourceStack = draggedCard?.Parent as StackPanel;
            if (draggedCard != null && sourceStack != null)
            {
                mouseOffset = e.GetPosition(draggedCard);
                draggedCards.Clear();

                int index = sourceStack.Children.IndexOf(draggedCard);
                for (int i = index; i < sourceStack.Children.Count; i++)
                {
                    if (sourceStack.Children[i] is Border cardBorder)
                    {
                        draggedCards.Add(cardBorder);
                    }
                }

                foreach (var card in draggedCards)
                {
                    Point globalPos = card.TranslatePoint(new Point(0, 0), GameCanvas);
                    if (card.Parent is Panel parentPanel)
                        parentPanel.Children.Remove(card);
                    GameCanvas.Children.Add(card);
                    Canvas.SetLeft(card, globalPos.X);
                    Canvas.SetTop(card, globalPos.Y + 60 * draggedCards.IndexOf(card));
                }

                draggedCard.CaptureMouse();
            }
        }

        private void Card_MouseMove(object sender, MouseEventArgs e)
        {
            if (draggedCard != null && e.LeftButton == MouseButtonState.Pressed)
            {
                Point position = e.GetPosition(GameCanvas);
                for (int i = 0; i < draggedCards.Count; i++)
                {
                    Canvas.SetLeft(draggedCards[i], position.X - mouseOffset.X);
                    Canvas.SetTop(draggedCards[i], position.Y - mouseOffset.Y + i * 60);
                }
            }
        }

        private void Card_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (draggedCard != null)
            {
                bool placed = false;

                foreach (var tableau in tableaus.Concat(foundations))
                {
                    if (IsOverlapping(draggedCard, tableau))
                    {
                        foreach (var card in draggedCards)
                        {
                            GameCanvas.Children.Remove(card);
                            tableau.Children.Add(card);
                        }
                        placed = true;
                        break;
                    }
                }

                if (!placed && sourceStack != null)
                {
                    foreach (var card in draggedCards)
                    {
                        GameCanvas.Children.Remove(card);
                        sourceStack.Children.Add(card);
                    }
                }
            }

            draggedCard?.ReleaseMouseCapture();
            draggedCard = null;
            draggedCards.Clear();
            sourceStack = null;

            CheckVictory();
        }

        private void CheckVictory()
        {
            int count = foundations.Sum(f => f.Children.Count);
            if (count == 52)
                MessageBox.Show("Поздравляем! Вы выиграли!", "Победа", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private bool IsOverlapping(FrameworkElement element, FrameworkElement target)
        {
            Point elPos = element.TranslatePoint(new Point(0, 0), GameCanvas);
            Point targetPos = target.TranslatePoint(new Point(0, 0), GameCanvas);
            Rect r1 = new(elPos, new Size(element.ActualWidth, element.ActualHeight));
            Rect r2 = new(targetPos, new Size(target.ActualWidth, target.ActualHeight));
            return r1.IntersectsWith(r2);
        }

        private string GetCardText(Card card)
        {
            string rank = card.Rank switch
            {
                1 => "A",
                11 => "J",
                12 => "Q",
                13 => "K",
                _ => card.Rank.ToString()
            };
            string suit = card.Suit switch
            {
                CardSuit.Hearts => "♥",
                CardSuit.Diamonds => "♦",
                CardSuit.Clubs => "♣",
                CardSuit.Spades => "♠",
                _ => "?"
            };
            return $"{rank}{suit}";
        }

        private Brush GetCardColor(CardSuit suit)
        {
            return suit switch
            {
                CardSuit.Hearts or CardSuit.Diamonds => Brushes.LightPink,
                CardSuit.Clubs or CardSuit.Spades => Brushes.LightGray,
                _ => Brushes.White
            };
        }

        private void SaveGame()
        {
            using FileStream fs = new("save.dat", FileMode.Create);
            BinaryFormatter bf = new();
            bf.Serialize(fs, new GameState(deck, stock.ToList(), waste.ToList()));
        }

        private void LoadGame()
        {
            if (File.Exists("save.dat"))
            {
                using FileStream fs = new("save.dat", FileMode.Open);
                BinaryFormatter bf = new();
                var state = (GameState)bf.Deserialize(fs);
                deck = state.Deck;
                stock = new Stack<Card>(state.Stock);
                waste = new Stack<Card>(state.Waste);
                InitGame();
            }
        }
    }

    [Serializable]
    public class Card
    {
        public CardSuit Suit;
        public int Rank;
        public bool IsFaceUp;

        public Card(CardSuit suit, int rank)
        {
            Suit = suit;
            Rank = rank;
            IsFaceUp = false;
        }
    }

    public enum CardSuit { Hearts, Diamonds, Clubs, Spades }

    [Serializable]
    public class GameState
    {
        public List<Card> Deck;
        public List<Card> Stock;
        public List<Card> Waste;

        public GameState(List<Card> deck, List<Card> stock, List<Card> waste)
        {
            Deck = deck;
            Stock = stock;
            Waste = waste;
        }
    }
}