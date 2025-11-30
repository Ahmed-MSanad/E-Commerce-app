import { NgxSpinnerService } from 'ngx-spinner';
import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { finalize } from 'rxjs';

export const loadingInterceptor: HttpInterceptorFn = (req, next) => {
  const _ngxSpinner = inject(NgxSpinnerService);

  const spinnerType = req.method == 'GET' ? 'ball-pulse' : 'cog';

  _ngxSpinner.show(undefined, {
    type: spinnerType,
    size: 'medium',
    color: '#007bff',
    bdColor: 'rgba(0, 0, 0, 0.3)',
    fullScreen: true
  });

  return next(req).pipe(finalize(() => {
    _ngxSpinner.hide();
  }));
};
