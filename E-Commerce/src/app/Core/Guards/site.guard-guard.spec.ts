import { TestBed } from '@angular/core/testing';
import { CanActivateFn } from '@angular/router';

import { siteGuardGuard } from './site.guard-guard';

describe('siteGuardGuard', () => {
  const executeGuard: CanActivateFn = (...guardParameters) => 
      TestBed.runInInjectionContext(() => siteGuardGuard(...guardParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeGuard).toBeTruthy();
  });
});
