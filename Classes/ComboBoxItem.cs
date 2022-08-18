namespace Indicon.Api.IpManager.Classes
{
    internal class ComboBoxItem
    {
        public string Text { get; set; }
        public object Tag { get; set; }
        public override string ToString()
        {
            return Text;
        }
    }
}
