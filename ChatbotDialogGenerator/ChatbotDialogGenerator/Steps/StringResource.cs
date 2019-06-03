namespace ChatbotDialogGenerator.Steps
{
    public class StringResource
    {
        public StringResource(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; }
        public string Value { get; }

        public override string ToString()
        {
            return Name;
        }
    }
}