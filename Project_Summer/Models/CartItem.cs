namespace Project_Summer.Models
{
    public class CartItem
    {
        public int IDhh { get; set; }
        public string Tenhh { get; set; }
        public string Hinh {  get; set; }
        public Double DonGia { get; set; }
        public int Soluong { get; set; }
        public double ThanhTien => Soluong * DonGia;

    }
}
