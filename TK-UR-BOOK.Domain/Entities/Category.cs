namespace TK_UR_BOOK.Domain.Entities
{
    public class Category : BaseEntity <int>
    {
        public string Name { get; private set; }
        public string Description { get; private set; }

        private readonly List<Book> _books = new();
        public IReadOnlyCollection<Book> Books => _books.AsReadOnly();

        private Category()
        {
        }
        public Category(string name, string description = "")
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Category name cannot be null or empty", nameof(name));
            Name = name;
            Description = description;
        }
        public void UpdateDescription(string newDescription)
        {
            Description = newDescription;
        }
    }
    
}