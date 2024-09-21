using TabibApp.Application.Dtos;
using TabibApp.Application.Interfaces;

namespace TabibApp.Application.Implementation;

public class DoctorService:IDoctorService
{
 private readonly IDoctorRepository _doctorRepository;

        public DoctorService(IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        public  Task<IEnumerable<DoctorDto>> GetAllAsync(int pageNumber, int pageSize)
        {
            return  _doctorRepository.GetAll( pageNumber,  pageSize);
        }

        public  Task<DoctorDto> GetByIdAsync(string applicationUserId)
        {
            return  _doctorRepository.GetById(applicationUserId);
        }

        public  Task<IEnumerable<DoctorDto>> SearchDoctorsAsync(DoctorSearchCriteria criteria)
        {
            return  _doctorRepository.SearchDoctorsAsync(criteria);
        }

        public  Task<Doctor> AddAsync(Doctor doctor)
        {
            var addedDoctor =  _doctorRepository.Add(doctor);
             _doctorRepository.SaveChangesAsync(); 
            return addedDoctor;
        }

        public  Task<DoctorDto> UpdateAsync(DoctorDto doctorDto)
        {
            var updatedDoctor =  _doctorRepository.Update(doctorDto);
             _doctorRepository.SaveChangesAsync(); // Save changes after updating
            return updatedDoctor;
        }

        public  Task<bool> DeleteAsync(string id)
        {
            // Delete a doctor based on their id (applicationUserId)
            var isDeleted =  _doctorRepository.Delete(id);
             _doctorRepository.SaveChangesAsync(); // Save changes after deletion
            return isDeleted;
        }

        public  Task<int> SaveChangesAsync()
        {
            return  _doctorRepository.SaveChangesAsync();
        }
    }

