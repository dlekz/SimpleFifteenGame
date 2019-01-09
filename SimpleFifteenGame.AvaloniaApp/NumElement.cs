namespace SimpleFifteenGame.AvaloniaApp
{   
    public class NumElement
    {
        public string Value {private set; get;}
         [System.Obsolete] public string Position {set; get;}
         public (int x, int y) Coord {set;get;}
        [System.Obsolete] public NumElement(string value, string position)
        {
            Value = value;
            Position = position;
        }
        public NumElement(string value, int x, int y)
        {
            Value = value;
            Coord = (x, y);
        }
    }
}