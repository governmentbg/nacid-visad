import { Nomenclature } from '../common/models/nomenclature.model';

export class SchoolYear extends Nomenclature {
  fromYear: number;
  toYear: number;
  isSchoolYear: boolean;
}
