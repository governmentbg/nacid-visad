import { AttachedFile } from '@abb/angular-lib';

export class MedicalCertificateDto {
	id: number;
	issuedDate: Date;
	description: string;
	file: AttachedFile;
}
