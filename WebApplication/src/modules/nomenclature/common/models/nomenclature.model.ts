export class Nomenclature {
  id: number;
  name: string;
  viewOrder: number | null;
  isActive: boolean;

  // for view purposes only
  isEditMode: boolean;
  originalObject: Nomenclature;
}
