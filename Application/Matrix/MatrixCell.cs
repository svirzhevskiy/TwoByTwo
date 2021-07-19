namespace TwoByTwo.Application.Matrix
{
    public struct MatrixCell
    {
        private int _value;
        
        public int Value
        {
            get => _value;
            set
            {
                Animation = "";
                _value = value;
            }
        }

        public string Animation { get; set; }

        public MatrixCell(int value)
        {
            _value = 0;
            Animation = "";
            Value = value;
        }
    }
}