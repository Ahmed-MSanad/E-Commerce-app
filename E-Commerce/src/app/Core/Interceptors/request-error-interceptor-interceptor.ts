import { HttpInterceptorFn } from '@angular/common/http';
import { catchError, throwError } from 'rxjs';
import Swal from 'sweetalert2';

export const requestErrorInterceptorInterceptor: HttpInterceptorFn = (req, next) => {
  return next(req).pipe(catchError((err)=>{

    console.log("interceptor ==> ", err.error.message);
    Swal.fire({
      title: "Error",
      text: err.error.message,
      icon: "error",
    });

    return throwError(() => err)
  }));
};
