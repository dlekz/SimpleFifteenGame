namespace SimpleFifteenGame.AvaloniaApp
{   
    public class NumElement
    {
        public string Value {private set; get;}
        public string Position {set; get;}
        public NumElement(string value, string position)
        {
            Value = value;
            Position = position;
        }
    }
}