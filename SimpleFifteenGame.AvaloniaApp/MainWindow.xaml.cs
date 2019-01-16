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
        private List<(int x,int y)> _coords {set; get;}

        public MainWindow()
        {
            InitializeComponent();
            this.KeyDown += Window_KeyDown;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            _coords = CoordList();
            Elements = InitElements(_coords);
            _button_content_Init();
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key.ToString() == "Up") Task.Run(() => UpButton_Click());
            if(e.Key.ToString() == "Down") Task.Run(() => DownButton_Click());
            if(e.Key.ToString() == "Left") Task.Run(() => LeftButton_Click());
            if(e.Key.ToString() == "Right") Task.Run(() => RightButton_Click());
        }
        private void UpButton_Click()
        {
            var finPos = Elements.First().Coord;
            var startPos = finPos;
            startPos.y += 1;
            _synchronize_buttons(startPos.x,startPos.y,finPos.x,finPos.y);
        }

        private void DownButton_Click()
        {
            var finPos = Elements.First().Coord;
            var startPos = finPos;
            startPos.y -= 1;
            _synchronize_buttons(startPos.x,startPos.y,finPos.x,finPos.y);
        }

        private void LeftButton_Click()
        {
            var finPos = Elements.First().Coord;
            var startPos = finPos;
            startPos.x += 1;
            _synchronize_buttons(startPos.x,startPos.y,finPos.x,finPos.y);
        }
        private void RightButton_Click()
        {
            var finPos = Elements.First().Coord;
            var startPos = finPos;
            startPos.x -= 1;
            _synchronize_buttons(startPos.x,startPos.y,finPos.x,finPos.y);
        }
        ///<summery>
        /// synchronization buttons position with collection of ellements
        ///</summery>
        ///<param name="x0">x coordinate for current element</param>
        ///<param name="y0">x coordinate for current element</param>
        ///<param name="x">x coordinate for 0 element</param>
        ///<param name="y">y coordinate for 0 element</param>

        private void _synchronize_buttons(int x0, int y0, int x, int y) 
        {
            Elements.Where( el => el.Coord == (x0,y0)).First().Coord = (x,y);
            Elements.Where( el => el.Coord == (x,y)).First().Coord = (x0,y0);

            Dispatcher.UIThread.InvokeAsync( ()=> 
            {
                var grid = this.FindControl<Grid>("buttonsGrid");
                foreach(AvaloniaObject el in grid.Children) 
                {
                    if(Grid.GetColumn(el) == x0 && Grid.GetRow(el) == y0)
                    {
                        Grid.SetRow(el,y);
                        Grid.SetColumn(el,x);
                    }
                }
                
            });
        }
        ///<summery>
        /// init collection of elements
        ///</summery>
        ///<param name="coords">list of coordinates</params>
        ///<returns>current collection of elements</returns>
        private NumElement[] InitElements(List<(int x, int y)> coords) 
        {
            var workingCoords = coords.Select(el => el).ToList();
            List<NumElement> elements = new List<NumElement>();
            
            for(int i = 0; workingCoords.Count > 0; i++)
            { 
                string value = (i != 0) ? i.ToString() : "";
                var randomCoord = CoordRandomizer(ref workingCoords);
                elements.Add(new NumElement(value, randomCoord.x, randomCoord.y));
            }
            return elements.ToArray();
        }
        ///<summery>
        /// init elements collection
        ///</summery>
        private List<(int x, int y)> CoordList(){
            return new List<(int x, int y)>(){
                (0,0),(1,0),(2,0),(3,0),
                (0,1),(1,1),(2,1),(3,1),
                (0,2),(1,2),(2,2),(3,2),
                (0,3),(1,3),(2,3),(3,3),
            };
        }
        private (int x, int y) CoordRandomizer(ref List <(int x, int y)> coords)
        {
            int index = (coords.Count() > 1) ? new Random().Next(0, coords.Count() - 1) : 0;
            var point = coords[index];
            coords.RemoveAt(index);
            return point;
        }

        private void _button_content_Init()
        {
            var buttonsGrid = this.FindControl<Grid>("buttonsGrid");
            foreach(var el in Elements)
            {
                if (el.Value == "") continue;
                var button = new Button();
                button.Classes.Add("content");
                Grid.SetRow(button,el.Coord.y);
                Grid.SetColumn(button,el.Coord.x);
                buttonsGrid.Children.Add(button);
                button.Content = el.Value;
            }
        }

    }
}