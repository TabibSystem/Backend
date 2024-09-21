using TabibApp.Application.Dtos;
using TabibApp.Application.Interfaces;

namespace TabibApp.Application.Implementation;

public class ClinicService: IClinicService
{
    private readonly IClinicRepository _clinicRepository;
    private readonly IAppointmentService _appointmentService;

    public ClinicService(IClinicRepository clinicRepository,IAppointmentService appointmentService)
    {
        _clinicRepository = clinicRepository;
        _appointmentService = appointmentService;
    }
    public async Task<ClinicDto> CreateClinicAsync(ClinicDto clinicDto)
    {
          var ID =await _clinicRepository.GetDoctorIdByUserIdAsync(clinicDto.DoctorId);
          if (ID is null)
          {
              return null;
          }
        var clinic = new Clinic
        {
            Name = clinicDto.Name,
            PhoneNumber = clinicDto.PhoneNumber,
            Description = clinicDto.Description,
            Examination = clinicDto.Examination,
            OpeningTime = clinicDto.OpeningTime,
            ClosingTime = clinicDto.ClosingTime,
            DoctorId = ID.Value,
            GovernorateId = clinicDto.GovernorateId,
            ClinicAddress = new ClinicAddress
            {
                BuildingNumber = clinicDto.ClinicAddress.BuildingNumber,
                StreetName = clinicDto.ClinicAddress.StreetName,
                Floor = clinicDto.ClinicAddress.Floor,
                ApartmentNumber = clinicDto.ClinicAddress.ApartmentNumber,
                Latitude = clinicDto.ClinicAddress.Latitude,
                Longitude = clinicDto.ClinicAddress.Longitude
            }
        };

         await _clinicRepository.CreateClinicAsync(clinic);
         await SaveChangesAsync();
         var createdClinicDto = new ClinicDto
         {
             Id = clinic.Id,
             Name = clinic.Name,
             PhoneNumber = clinic.PhoneNumber,
             Description = clinic.Description,
             Examination = clinic.Examination,
             OpeningTime = clinic.OpeningTime,
             ClosingTime = clinic.ClosingTime,
             DoctorId = clinicDto.DoctorId,
             
             GovernorateId = clinic.GovernorateId,
             ClinicAddress = new ClinicAddressDto()
             {
                 BuildingNumber = clinic.ClinicAddress.BuildingNumber,
                 StreetName = clinic.ClinicAddress.StreetName,
                 Floor = clinic.ClinicAddress.Floor,
                 ApartmentNumber = clinic.ClinicAddress.ApartmentNumber,
                 Latitude = clinic.ClinicAddress.Latitude,
                 Longitude = clinic.ClinicAddress.Longitude
             }
         };
         return createdClinicDto;
    }

    public async Task SaveScheduleAsync(IEnumerable<TimeSlot> timeSlots)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> ClinicExistsAsync(string name, string phoneNumber)
    {
        var exists = await _clinicRepository.ClinicExistsAsync(name, phoneNumber);
        return exists;
    }

    public async Task<ClinicDto?> GetClinicById(string id)
    {
        
        var clinic= await _clinicRepository.GetClinicById(id);
        if (clinic is null)
            return null;
        var Id= await _clinicRepository.GetDoctorIdByUserIdAsync(id);
        if (Id is null)
            return null;
        var clinicDto = new ClinicDto
        {
            Id = clinic.Id,
            Name = clinic.Name,
            PhoneNumber = clinic.PhoneNumber,
            Description = clinic.Description,
            Examination = clinic.Examination,
            OpeningTime = clinic.OpeningTime,
            ClosingTime = clinic.ClosingTime,
            
            ClinicAddressId = clinic.ClinicAddressId,
            ClinicAddress = new ClinicAddressDto
            {
               
                BuildingNumber = clinic.ClinicAddress.BuildingNumber,
                StreetName = clinic.ClinicAddress.StreetName,
                Floor = clinic.ClinicAddress.Floor,
                ApartmentNumber = clinic.ClinicAddress.ApartmentNumber,
                Latitude = clinic.ClinicAddress.Latitude,
                Longitude = clinic.ClinicAddress.Longitude
            },
            DoctorId = clinic.Id.ToString(),
            GovernorateId = clinic.GovernorateId
        };
        return clinicDto;

    }

    public  Task<IEnumerable<Clinic>> GetClinicsByDoctorIdAsync(string applicationUserId)
    {
        return  _clinicRepository.GetClinicsByDoctorIdAsync(applicationUserId);
    }

    public  Task<Clinic?> UpdateClinicAsync(int clinicId, ClinicDto updatedClinicDto)
    {
        var updatedClinic = new Clinic
        {
            Name = updatedClinicDto.Name,
            PhoneNumber = updatedClinicDto.PhoneNumber,
            Description = updatedClinicDto.Description,
            Examination = updatedClinicDto.Examination,
            OpeningTime = updatedClinicDto.OpeningTime,
            ClosingTime = updatedClinicDto.ClosingTime,
           
            GovernorateId = updatedClinicDto.GovernorateId,
            ClinicAddress = new ClinicAddress
            {
                BuildingNumber = updatedClinicDto.ClinicAddress.BuildingNumber,
                StreetName = updatedClinicDto.ClinicAddress.StreetName,
                Floor = updatedClinicDto.ClinicAddress.Floor,
                ApartmentNumber = updatedClinicDto.ClinicAddress.ApartmentNumber,
                Latitude = updatedClinicDto.ClinicAddress.Latitude,
                Longitude = updatedClinicDto.ClinicAddress.Longitude
            }
        };

        
           return  _clinicRepository.UpdateClinicAsync(clinicId, updatedClinic);
           
    }


    public  Task<bool> DeleteClinicAsync(int id)
    {

        return  _clinicRepository.DeleteClinicAsync(id);
    }

    public  Task<int> SaveChangesAsync()
    {
        return  _clinicRepository.SaveChangesAsync();
    }
}