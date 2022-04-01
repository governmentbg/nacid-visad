import { NomenclatureDto } from 'src/modules/nomenclature/common/models/nomenclature-dto';
import { LanguageProficiencyDto } from './language-proficiency.dto';
import { TrainingLanguageDocumentDto } from './training-language-document.dto';

export class TrainingDto {
  languageProficiencies: LanguageProficiencyDto[];

  languageDepartment: string;
  languageTrainingDuration: number | null;

  trainingLanguageDocumentFile: TrainingLanguageDocumentDto;

  constructor() {
    const requiredPropficiency = new LanguageProficiencyDto();
    requiredPropficiency.language = new NomenclatureDto();
    requiredPropficiency.language.name = "Български";
    requiredPropficiency.language.id = 11;
    requiredPropficiency.required = true;
    this.languageProficiencies = [requiredPropficiency];
  }
}
