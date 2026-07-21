namespace Restaurant_Management_System.Services
{
    public interface IOrderService
    {
        public bool reserveTable(int tableNumber, int numberOfPeople);
        public bool CreateOrder(int tableNumber, List<string> items);
        public bool AddOrderItem(int orderId, string item);

        public  bool modifyOrder();
    }
}
