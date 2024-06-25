using AutoMapper;
using Profiles.Contracts.DTOs.Doctor;
using Profiles.Contracts.DTOs.Patient;
using Profiles.Contracts.DTOs.Receptionist;
using Profiles.Domain.Entities;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        #region Doctor

        CreateMap<Doctor, DoctorResponseDTO>()
            .ForPath(dest => dest.Office.OfficeId, opt => opt.MapFrom(src => src.OfficeId))
            .ForMember(dest => dest.Experience, 
            opt => opt.MapFrom(src => DateTime.Now.AddYears(1).AddYears(-src.CareerStartYear.Year).Year));

        CreateMap<DoctorCreateDTO, Doctor>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (Status)src.Status));

        CreateMap<DoctorUpdateDTO, Doctor>();

        #endregion

        #region Patient

        CreateMap<PatientCreateDTO, Patient>();
        CreateMap<Patient, PatientResponseDTO>()
            .ForPath(dest => dest.PhoneNumber, 
            opt => opt.MapFrom(src => src.Account.PhoneNumber));

        CreateMap<PatientUpdateDTO, Patient>()
            .ForPath(dest => dest.Account.PhoneNumber,
            opt => opt.MapFrom(src => src.PhoneNumber));

        #endregion

        #region Receptionist

        CreateMap<ReceptionistCreateDTO, Receptionist>();

        CreateMap<Receptionist, ReceptionistResponseDTO>()
            .ForPath(dest => dest.Office.OfficeId, opt => opt.MapFrom(src => src.OfficeId));

        CreateMap<ReceptionistUpdateDTO, Receptionist>();

        #endregion
    }
}
