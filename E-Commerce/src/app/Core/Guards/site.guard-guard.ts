import { isPlatformBrowser } from '@angular/common';
import { inject, PLATFORM_ID } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

export const siteGuardGuard: CanActivateFn = (route, state) => {
  const _router = inject(Router);
  const _platform_id = inject(PLATFORM_ID);
  
  if(isPlatformBrowser(_platform_id)){
    if(localStorage.getItem("token") !== null){
      return true;
    }
    else{
      _router.navigate(['/Login']);
      return false;
    }
  }

  return true;
};
