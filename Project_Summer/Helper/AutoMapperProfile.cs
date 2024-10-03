using AutoMapper;
using Project_Summer.DataContext;
using Project_Summer.Models;

namespace Project_Summer.Helper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<DangKyVM, KhachHang>();//ModelVM,Model 
            
        }
    }
}
