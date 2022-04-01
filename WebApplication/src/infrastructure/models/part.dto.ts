import { PartState } from '../enums/part-state.enum';

export class PartDto<TDto> {
  id: number;
  entity: TDto;
  state: PartState;
}
