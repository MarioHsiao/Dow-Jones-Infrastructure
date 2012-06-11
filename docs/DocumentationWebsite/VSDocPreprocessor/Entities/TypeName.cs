namespace VSDocPreprocessor.Entities
{
    public class TypeName
    {
        public string Namespace { get; private set; }
        public string Name { get; private set; }
        public string FullName { get; private set; }

        public TypeName(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                return;

            if (fullName.StartsWith("T:"))
                fullName = fullName.Substring(2);

            fullName = fullName.Trim();

            FullName = fullName;

            var lastDot = fullName.LastIndexOf('.');

            if(lastDot > 0)
            {
                Namespace = fullName.Substring(0, lastDot);
                Name = fullName.Substring(lastDot + 1);
            }
            else
            {
                Name = fullName;
            }
        }

        public static implicit operator TypeName(string fullName)
        {
            return new TypeName(fullName);
        }
    }
}
