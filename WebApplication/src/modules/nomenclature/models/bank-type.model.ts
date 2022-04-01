import { Nomenclature } from '../common/models/nomenclature.model';

export class Bank extends Nomenclature {
	iban_Code: string;
	bic: string;
}
