using Microsoft.EntityFrameworkCore;
using Profiles.Domain.Entities;

namespace Profiles.Infrastructure.Repositories;

public static class RepositoryExtentions
{
    public static IQueryable<Doctor> FilterDoctorsBySpecialization(this IQueryable<Doctor> doctors, Guid? specializationId)
        => specializationId is null
        ? doctors
        : doctors.Where(d => d.SpecializationId.Equals(specializationId));

    public static IQueryable<Doctor> Search(this IQueryable<Doctor> doctors, string searchFullName)
    {
        if (string.IsNullOrWhiteSpace(searchFullName))
        {
            return doctors;
        }

        var loverCaseLastName = searchFullName.Trim().ToLower();

        return doctors.Where(d => (d.FirstName + " " + d.LastName + " " + d.MiddleName).ToLower()
                      .Contains(loverCaseLastName));
    }

    public static IQueryable<Patient> Search(this IQueryable<Patient> patients, string searchFullName)
    {
        if (string.IsNullOrWhiteSpace(searchFullName))
        {
            return patients;
        }

        var loverCaseLastName = searchFullName.Trim().ToLower();

        return patients.Where(d => (d.FirstName + " " + d.LastName + " " + d.MiddleName).ToLower()
                      .Contains(loverCaseLastName));
    }
}
