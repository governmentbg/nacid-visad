import { CommitState } from '../infrastructure/enums/commit-state.enum';
import { PartState } from '../infrastructure/enums/part-state.enum';
import { ApplicationLotResultType } from './application/enums/application-lot-result-type.enum';
import { ReportType } from './application/enums/report-type.enum';
import { UserStatus } from './user/enums/user-status.enum';

export const PartStateEnumLocalization = {
  [PartState.modified]: 'Променено',
  [PartState.erased]: 'Заличено'
};

export const CommitStateEnumLocalization = {
  [CommitState.initialDraft]: 'Първоначална чернова',
  [CommitState.modification]: 'Върнато за редакция',
  [CommitState.actual]: 'Изпратено за вписване',
  [CommitState.actualWithModification]: 'Неодобрено',
  [CommitState.history]: 'Неодобрено',
  [CommitState.deleted]: 'Изтрито',
  [CommitState.commitReady]: 'Готово за вписване',
  [CommitState.approved]: 'Одобрено',
  [CommitState.annulled]: 'Анулирано',
  [CommitState.refusedSign]: 'Отказано подписване',
};

export const ApplicationLotResultTypeEnumLocalization = {
  [ApplicationLotResultType.certificate]: 'Издадено удостоверение',
  [ApplicationLotResultType.rejection]: 'Отказано вписване',
  [ApplicationLotResultType.actual]: 'Изпратено за вписване',
  [ApplicationLotResultType.modification]: 'Върнато за редакция',
  [ApplicationLotResultType.deleted]: 'Изтрито',
  [ApplicationLotResultType.approved]: 'Одобрено',
  [ApplicationLotResultType.annulled]: 'Анулирано',
  [ApplicationLotResultType.refusedSign]: 'Отказано подписване'
};

export const UserStatusEnumLocalization = {
  [UserStatus.active]: 'Активен',
  [UserStatus.inactive]: 'Неактивиран',
  [UserStatus.deactivated]: 'Деактивиран',
};

export const ReportTypeEnumLocalization = {
  [ReportType.defaultReport]: "Общ брой заявления",
  [ReportType.reportByInstitution]: "По висше училище",
  [ReportType.reportByNationality]: "По гражданство",
  [ReportType.reportByCountry]: "По месторождение",
  [ReportType.reportByEducationalQualification]: "По ОКС",
  [ReportType.reportByCandidateWithMoreThanOneCertificate]: "С повече от 1 издадено удостоверение",
}
