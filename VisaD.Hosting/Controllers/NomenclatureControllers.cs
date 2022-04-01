using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VisaD.Application.InstitutionSpecialities.Dtos;
using VisaD.Application.InstitutionSpecialities.Services;
using VisaD.Application.Nomenclatures.Dtos;
using VisaD.Application.Nomenclatures.Services;
using VisaD.Data.Nomenclatures;
using VisaD.Data.Users;
using VisaD.Hosting.Controllers.Common;

namespace VisaD.Hosting.Controllers
{
	public class DurationTypeController : BaseNomenclatureController<DurationType, NomenclatureDto<DurationType>, NomenclatureFilterDto<DurationType>>
	{
		public DurationTypeController(INomenclatureService<DurationType> service)
			: base(service)
		{

		}
	}
	
	public class CurrencyTypeController : BaseNomenclatureController<CurrencyType, NomenclatureDto<CurrencyType>, NomenclatureFilterDto<CurrencyType>>
	{
		public CurrencyTypeController(INomenclatureService<CurrencyType> service)
			: base(service)
		{

		}
	}

	public class ApplicationFileTypeController : BaseNomenclatureController<ApplicationFileType, ApplicationFileTypeNomenclatureDto, NomenclatureFilterDto<ApplicationFileType>>
	{
		private readonly IApplicationFileTypeService applicationFileTypeService;

		public ApplicationFileTypeController(INomenclatureService<ApplicationFileType> service, IApplicationFileTypeService applicationFileTypeService)
			: base(service)
		{
			this.applicationFileTypeService = applicationFileTypeService;
		}

		[HttpGet("filtered")]
		public async Task<IEnumerable<ApplicationFileTypeNomenclatureDto>> GetApplicationFileTypes([FromQuery] string qualificationName)
			=> await this.applicationFileTypeService.GetApplicationFileTypes(qualificationName);

		[HttpGet("selectFile")]
		public async Task<ApplicationFileTypeNomenclatureDto> SelectApplicationFileType([FromQuery] string alias)
			=> await this.applicationFileTypeService.SelectApplicationFileType(alias);
	}

	public class EducationFormTypeController : BaseNomenclatureController<EducationFormType, NomenclatureDto<EducationFormType>, NomenclatureFilterDto<EducationFormType>>
	{
		public EducationFormTypeController(INomenclatureService<EducationFormType> service)
			: base(service)
		{

		}
	}

	public class CountryController : BaseNomenclatureController<Country, NomenclatureDto<Country>, CountryNomenclatureFilterDto>
	{
		public CountryController(INomenclatureService<Country> service)
			: base(service)
		{

		}
	}

	public class LanguageDegreeController : BaseNomenclatureController<LanguageDegree, NomenclatureDto<LanguageDegree>, NomenclatureFilterDto<LanguageDegree>>
	{
		public LanguageDegreeController(INomenclatureService<LanguageDegree> service)
			: base(service)
		{

		}
	}

	public class InstitutionController : BaseNomenclatureController<Institution, NomenclatureDto<Institution>, NomenclatureFilterDto<Institution>>
	{
		private readonly InstitutionService institutionService;

		public InstitutionController(INomenclatureService<Institution> service, InstitutionService institutionService)
			: base(service)
		{
			this.institutionService = institutionService;
		}

		[HttpGet("University")]
		public async Task<IEnumerable<NomenclatureDto<Institution>>> GetUniversities([FromQuery] NomenclatureFilterDto<Institution> filter)
			=> await this.institutionService.GetUniversities<NomenclatureFilterDto<Institution>, NomenclatureDto<Institution>>(filter);

		[HttpGet("ApplicationsUniversities")]
		public async Task<IEnumerable<NomenclatureDto<Institution>>> GetApplicationsUniversities([FromQuery] NomenclatureFilterDto<Institution> filter)
			=> await this.institutionService.GetApplicationsUniversities<NomenclatureFilterDto<Institution>, NomenclatureDto<Institution>>(filter);

		[HttpGet("Faculty")]
		public async Task<IEnumerable<NomenclatureDto<Institution>>> GetFaculties([FromQuery] NomenclatureFilterDto<Institution> filter)
			=> await this.institutionService.GetFaculties<NomenclatureFilterDto<Institution>, NomenclatureDto<Institution>>(filter);

		[HttpGet("FacultyByQualification")]
		public async Task<IEnumerable<NomenclatureDto<Institution>>> GetFacultiesByEducationalQualication([FromQuery] NomenclatureFilterDto<Institution> filter, [FromQuery] int educationalQualificationId, [FromQuery] bool isTrainee)
			=> await this.institutionService.GetFacultiesByEducationalQualification<NomenclatureFilterDto<Institution>, NomenclatureDto<Institution>>(filter, educationalQualificationId, isTrainee);

		[HttpGet("InstitutionByName")]
		public async Task<NomenclatureDto<Institution>> GetSingleInstitution([FromQuery] NomenclatureFilterDto<Institution> filter, [FromQuery] string institutionName)
			=> await this.institutionService.GetSingle<NomenclatureFilterDto<Institution>, NomenclatureDto<Institution>>(filter, institutionName);
	}

	public class SpecialityController : BaseNomenclatureController<Speciality, NomenclatureDto<Speciality>, NomenclatureFilterDto<Speciality>>
	{
		private readonly InstitutionSpecialitiesService institutionSpecialitiesService;

		public SpecialityController(INomenclatureService<Speciality> service, InstitutionSpecialitiesService institutionSpecialitiesService)
			: base(service)
		{
			this.institutionSpecialitiesService = institutionSpecialitiesService;
		}

		[HttpGet("InstitutionSpecialities")]
		public async Task<IEnumerable<SpecialityInformationDto>> GetInstitutionSpecialities([FromQuery] SpecialityFilterDto filter)
			=> await this.institutionSpecialitiesService.GetInstitutionSpecialities(filter);

		[HttpGet("SpecialityByUniversity")]
		public async Task<IEnumerable<SpecialityInformationDto>> GetSpecialitiesByUniversity([FromQuery] SpecialityFilterDto filter)
			=> await this.institutionSpecialitiesService.SpecialitiesByUniversity(filter);

		[HttpGet("InstitutionSpecialitiesByQualification")]
		public async Task<IEnumerable<SpecialityInformationDto>> GetInstitutionSpecialities([FromQuery] SpecialityFilterDto filter, [FromQuery] int educationalQualificationId)
			=> await this.institutionSpecialitiesService.GetInstitutionSpecialitiesByEducationalQualification(filter, educationalQualificationId);
	}

	public class EducationalQualificationController
			: BaseNomenclatureController<EducationalQualification, EducationalQualificationNomenclatureDto, NomenclatureFilterDto<EducationalQualification>>
	{
		public EducationalQualificationController(INomenclatureService<EducationalQualification> service)
			: base(service)
		{

		}
	}

	public class RoleController
		: BaseNomenclatureController<Role, RoleNomenclatureDto, NomenclatureFilterDto<Role>>
	{
		public RoleController(INomenclatureService<Role> service)
			: base(service)
		{

		}
	}

	public class TrainingTypeController
		: BaseNomenclatureController<TrainingType, NomenclatureDto<TrainingType>, NomenclatureFilterDto<TrainingType>>
	{
		public TrainingTypeController(INomenclatureService<TrainingType> service)
			: base(service)
		{

		}
	}

	public class SchoolYearController
		: BaseNomenclatureController<SchoolYear, NomenclatureDto<SchoolYear>, SchoolYearNomenclatureFilterDto>
	{
		private readonly ISchoolYearService schoolYearService;

		public SchoolYearController(INomenclatureService<SchoolYear> service, ISchoolYearService schoolYearService)
			: base(service)
		{
			this.schoolYearService = schoolYearService;
		}

		[HttpGet("Filtered")]
		public Task<IEnumerable<SchoolYear>> SelectSchoolYears(CancellationToken cancellationToken)
			=> this.schoolYearService.SelectSchoolYearsAsync(cancellationToken);

		[HttpGet("default")]
		public Task<SchoolYear> GetDefaultYears(CancellationToken cancellationToken)
			=> this.schoolYearService.GetDefaultSchoolYearsAsync(cancellationToken);
	}

	public class DiplomaTypeController
		: BaseNomenclatureController<DiplomaType, DiplomaTypeNomenclatureDto, NomenclatureFilterDto<DiplomaType>>
	{
		public DiplomaTypeController(INomenclatureService<DiplomaType> service)
			: base(service)
		{

		}

		[HttpGet("getDiplomaType")]
		public async Task<DiplomaType> GetDiplomaType([FromQuery] string alias)
			=> await this.nomenclatureService.GetSingleOrDefaultNomenclatureAsync(x => x.Alias == alias);
	}

	public class LanguageController
		: BaseNomenclatureController<Language, NomenclatureDto<Language>, NomenclatureFilterDto<Language>>
	{
		private readonly InstitutionSpecialitiesService institutionSpecialitiesService;

		public LanguageController(INomenclatureService<Language> service, InstitutionSpecialitiesService institutionSpecialitiesService)
			: base(service)
		{
			this.institutionSpecialitiesService = institutionSpecialitiesService;
		}

		[HttpGet("InstitutionSpecialityLanguages")]
		public async Task<IEnumerable<NomenclatureDto<Language>>> GetInstitutionSpecialityLanguages([FromQuery] SpecialityFilterDto filter, [FromQuery] int? specialityId)
			=> await this.institutionSpecialitiesService.GetInstitutionSpecialityLanguages(filter.EntityId, specialityId);
	}

	public class BankController
		: BaseNomenclatureController<Bank, NomenclatureDto<Bank>, NomenclatureFilterDto<Bank>>
	{
		private readonly IBankService bankService;

		public BankController(INomenclatureService<Bank> service, IBankService bankService)
			: base(service)
		{
			this.bankService = bankService;
		}

		[HttpGet("getBank")]
		public async Task<Bank> GetBankByIbanCode([FromQuery] string iban)
		{
			iban = iban.ToUpper();
			this.bankService.ValidateIban(iban);

			return await this.nomenclatureService.GetSingleOrDefaultNomenclatureAsync(x => x.IBAN_CODE == iban.Substring(4, 4));
		}
	}

	public class RegulationController
		: BaseNomenclatureController<Regulation, NomenclatureDto<Regulation>, NomenclatureFilterDto<Regulation>>
	{
		public RegulationController(INomenclatureService<Regulation> service)
			: base(service)
		{

		}
	}
}
