namespace VisaD.Application.InstitutionSpecialities.Dtos
{
    public class SpecialityDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ViewOrder { get; set; }
        public bool IsActive { get; set; }

        public int? EducationalQualificationId { get; set; }

    }
}
