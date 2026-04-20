namespace ElipgoVideo.Commons.EventArgs
{
    public class MovingSeekBarEventArg : System.EventArgs
    {
        private int movingValue;

        private bool reset;

        public MovingSeekBarEventArg(int movingValue, bool reset)
        {
            this.movingValue = movingValue;
            this.reset = reset;
        }

        public int MovingValue
        {
            get { return this.movingValue; }
        }

        public bool Reeset
        {
            get { return this.reset; }
        }
    }
}
