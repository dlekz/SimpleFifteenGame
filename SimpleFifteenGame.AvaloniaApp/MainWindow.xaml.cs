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
            _content_Init();
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
            string startPos = Elements.First().Position;
            int[] startCoords = startPos.ToArray()
                                .Select(el => int.Parse(el.ToString())).ToArray();
            startCoords[0] += 1;
            string finPos = $"{startCoords[0]}{startCoords[1]}";

            Sinchronize(startPos,finPos);
        }

        private void DownButton_Click()
        {
            string startPos = Elements.First().Position;
            int[] startCoords = startPos.ToArray()
                                .Select(el => int.Parse(el.ToString())).ToArray();
            startCoords[0] -= 1;
            string finPos = $"{startCoords[0]}{startCoords[1]}";

            Sinchronize(startPos,finPos);
        }

        private void LeftButton_Click()
        {
            string startPos = Elements.First().Position;
            int[] startCoords = startPos.ToArray()
                                .Select(el => int.Parse(el.ToString())).ToArray();
            startCoords[1] += 1;
            string finPos = $"{startCoords[0]}{startCoords[1]}";

            Sinchronize(startPos,finPos);
        }
        private void RightButton_Click()
        {
            string startPos = Elements.First().Position;
            int[] startCoords = startPos.ToArray()
                                .Select(el => int.Parse(el.ToString())).ToArray();
            startCoords[1] -= 1;
            string finPos = $"{startCoords[0]}{startCoords[1]}";

            Sinchronize(startPos,finPos);
        }
        private void Sinchronize(string startPos, string finPos)
        {
            if(!Coords.Contains(finPos)) return;
            string firstName = Elements.Where( el => el.Position == finPos).First().Value;
            Elements.Where( el => el.Position == finPos).First().Position = startPos;
            string secondName = Elements.Where( el => el.Position == startPos).First().Value;
            Elements.Where( el => el.Position == startPos).First().Position = finPos;

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
                elements.Add(new NumElement(value,CoordRandomizer(ref workingCoords)));
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
        private void _content_Init()
        {
            foreach(var el in Elements)
            {
                this.FindControl<TextBlock>($"cell_{el.Position}").Text = el.Value;
            }
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
    }
}