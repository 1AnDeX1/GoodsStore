namespace GoodsStore.Dto
{
    public class ProductsDto
    {
        public int ProductID { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public string? Image { get; set; }
    }
}
