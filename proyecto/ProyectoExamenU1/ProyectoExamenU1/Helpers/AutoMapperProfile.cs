using AutoMapper;
using ProyectoExamenU1.Database.Entities;
using ProyectoExamenU1.Dtos.Permitions;

namespace ProyectoExamenU1.Helpers
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            Permition();
            //MapsForPosts();
        }

        private void Permition()
        {
            CreateMap<PermitionApplicationEntity, ApplicationPermitionDto>();
            CreateMap<ApplicationPerrmitionCreateDto, PermitionApplicationEntity>();
            CreateMap<ApplicationPermitionEditDto, PermitionApplicationEntity>();
        }
    }
}
