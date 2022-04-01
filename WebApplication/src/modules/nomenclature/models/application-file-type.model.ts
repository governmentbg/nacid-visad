import { Nomenclature } from '../common/models/nomenclature.model';

export class ApplicationFileType extends Nomenclature {
	hasDate: boolean;
	isForBachelor: boolean;
	isForDoctor: boolean;
	isForMaster: boolean;
	description: string;
	alias: string;
	isForMasterWithSecondary: boolean;
}
