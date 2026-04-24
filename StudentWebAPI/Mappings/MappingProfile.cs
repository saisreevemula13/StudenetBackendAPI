using AutoMapper;
using StudentWebAPI.DTO;
using StudentWebAPI.Model;

namespace StudentWebAPI.Mappings
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Student, StudentDto>();
            CreateMap<CreateStudentDTO, Student>();
            CreateMap<UpdateStudentDto, Student>()
            .ForAllMembers(opt =>
             opt.Condition((src, dest, srcMember) => srcMember != null));


            CreateMap<Event, EventResponseDTO>();

            CreateMap<CreateEventDTO, Event>();

            CreateMap<UpdateEventDTO, Event>()
                .ForAllMembers(opt =>
                    opt.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Registration, RegistrationResponseDTO>()
                .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Student.Name))
                .ForMember(dest => dest.EventTitle, opt => opt.MapFrom(src => src.Event.Title)); 
            CreateMap<RegistrationCreateDTO, Registration>(); 
            CreateMap<RegistrationUpdateDTO, Registration>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
