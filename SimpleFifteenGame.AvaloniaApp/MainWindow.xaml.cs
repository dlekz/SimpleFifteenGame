using Avalonia;
using Avalonia.Input;
using Avalonia.Threading;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleFifteenGame.AvaloniaApp
{
    public class MainWindow : Window
    {

        private NumElement[] Elements {set; get;}
        private List<string> Coords {set; get;}
        public MainWindow()
        {
            InitializeComponent();
            Coords = CoordList_Init();
            Elements = InitElements_Test(Coords);
            _button_content_Init();
            this.KeyDown += Window_KeyDown_Test;
        }
        ///<summery>
        /// Test this.KeyDown Event
        ///</summery>
        private void Window_KeyDown_Test(object sender, KeyEventArgs e) 
        {
            if(e.Key.ToString() == "Up") Task.Run(() => UpButton_Click());
            if(e.Key.ToString() == "Down") Task.Run(() => DownButton_Click());
            if(e.Key.ToString() == "Left") Task.Run(() => LeftButton_Click());
            if(e.Key.ToString() == "Right") Task.Run(() => RightButton_Click());
        }
        private void UpButton_Click()
        {
            var startCoord = Elements.First().Coord;
            var finCoord = startCoord;
            finCoord.y += 1;
            Sinchronize($"{startCoord.x}{startCoord.y}",$"{finCoord.x}{finCoord.y}");
        }

        private void DownButton_Click()
        {
            var startCoord = Elements.First().Coord;
            var finCoord = startCoord;
            finCoord.y -= 1;
            Sinchronize($"{startCoord.x}{startCoord.y}",$"{finCoord.x}{finCoord.y}");
        }

        private void LeftButton_Click()
        {
            var startCoord = Elements.First().Coord;
            var finCoord = startCoord;
            finCoord.x += 1;
            Sinchronize($"{startCoord.x}{startCoord.y}",$"{finCoord.x}{finCoord.y}");
        }
        private void RightButton_Click()
        {
            var startCoord = Elements.First().Coord;
            var finCoord = startCoord;
            finCoord.x -= 1;
            Sinchronize($"{startCoord.x}{startCoord.y}",$"{finCoord.x}{finCoord.y}");
        }
        private void Sinchronize(string startPos, string finPos)
        {
            if(!Coords.Contains(finPos)) return;
            string firstName = Elements.Where( el => $"{el.Coord.x}{el.Coord.y}" == finPos).First().Value;
            Elements.Where( el => $"{el.Coord.x}{el.Coord.y}" == finPos).First().Coord = 
                (int.Parse(startPos.Substring(0,1)),int.Parse(startPos.Substring(1,1)));
            string secondName = Elements.Where( el => $"{el.Coord.x}{el.Coord.y}" == startPos).First().Value;
            Elements.Where( el => $"{el.Coord.x}{el.Coord.y}" == startPos).First().Coord = 
                (int.Parse(finPos.Substring(0,1)),int.Parse(finPos.Substring(1,1)));

            Dispatcher.UIThread.InvokeAsync(()=>
            {
                this.FindControl<TextBlock>($"cell_{startPos}").Text = firstName;
                this.FindControl<TextBlock>($"cell_{finPos}").Text = secondName;
            });
        }
        ///<summery>
        /// Test Elements Init
        ///</summery>
        private NumElement[] InitElements_Test(List<string> coords)
        {
            List<string> workingCoords = coords.Select(el => el).ToList();
            List<NumElement> elements = new List<NumElement>();
            for(int i = 0; workingCoords.Count > 0; i++)
            { 
                string value = (i != 0) ? i.ToString() : "";
                string randomStr = CoordRandomizer(ref workingCoords);
                elements.Add(new NumElement(value, int.Parse(randomStr.Substring(0,1)), int.Parse(randomStr.Substring(1,1))));
            }
            return elements.ToArray();
        }
        private List<string> CoordList_Init()
        {
            return new List<string>(){"11","12","13","14",
                                    "21","22","23","24",
                                    "31","32","33","34",
                                    "41","42","43","44"};
        }
        private string CoordRandomizer(ref List<string> coords)
        {
            int index = (coords.Count() > 1) ? new Random().Next(0,coords.Count()-1) : 0;
            string result = coords[index];
            coords.RemoveAt(index);
            return result;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void _button_content_Init()
        {
            var buttonsGrid = this.FindControl<Grid>("buttonsGrid");
            foreach(var el in Elements)
            {
                if (el.Value == "") continue;
                var button = new Button();
                button.Classes.Add("content");
                int rowPos = el.Coord.x - 1;
                int colPos = el.Coord.y - 1;
                Grid.SetRow(button,rowPos);
                Grid.SetColumn(button,colPos);
                buttonsGrid.Children.Add(button);
                button.Content = el.Value;
            }
        }

    }
}