import { Nomenclature } from '../common/models/nomenclature.model';

export class DiplomaType extends Nomenclature {
	isNacidVerificationRequired: boolean;
	alias: string;
}
