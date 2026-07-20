namespace Restaurant_Management_System.Services
{
    public interface IMenuService
    {
        public void AddMenuItem(string name, decimal price);
        public void UpdateRecipe(string name, string newRecipe);
        public void deactivateItem(string itemId);
        public void getMenuByCategory(string category);
    }
}
