import { HttpErrorResponse } from '@angular/common/http';
import { ToastrService } from 'ngx-toastr';
import { DomainError } from '../models/domain-error.model';

export const handleDomainError = (error: any, handledErrors: { code: string, text: string, timeout?: number }[], toastrService: ToastrService) => {
  if (error instanceof HttpErrorResponse) {
    if (error.status === 422) {
      const data = error.error as DomainError;

      for (let i = 0; i <= data.errorMessages.length - 1; i++) {
        const errorCode = data.errorMessages[i].domainErrorCode;

        const handledError = handledErrors.find(e => e.code === errorCode);
        if (handledError) {
          toastrService.error(handledError.text, null, { timeOut: (handledError.timeout === null || handledError.timeout === undefined) ? 5000 : handledError.timeout });
        }
      }
    }
  }
};
